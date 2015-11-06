using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tanks_OlofSjöholm;

using com.nuggeta.game.core.ngdl.nobjects;
using com.nuggeta.game.core.api.handlers;
using com.nuggeta.ngdl.nobjects;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Nugetta;

class Controller
{

    private TanksGame tankGame = null;
    private NugettaHandler nugetta;
    private String gameID;
    public String PlayerID { get { return nugetta.Player.getId(); } }

    public Controller (TanksGame tankGame){
        this.tankGame = tankGame;
        nugetta = MyNugettaHandler.getInstance();
    }

    public void Update(GameTime gameTime) {
        if (tankGame != null) {
            tankGame.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        if (tankGame != null) {
            tankGame.Draw(spriteBatch);
        }
    }

    public void SpawnObject(Type type, Vector2 vec, Vector2 pos) {
        //Spara konstruktorn
        ConstructorInfo constructor = type.GetConstructor(new Type[] { 
            vec.GetType(),
            pos.GetType()
        });

        //Så här kör man konstuktorn
        constructor.Invoke(new object[] { vec, pos });

        //Skicka 
        JObject o = new JObject();
        o[vec.GetType()] = JToken.FromObject(vec);
        o[pos.GetType()] = JToken.FromObject(vec);

        nugetta.SendGameMessage(o.ToString(), "");
    }

    private String KEY_OBJECT_TYPE = "TYPE";
    private String KEY_MSG_TYPE = "MSGTYPE";
    private String KEY_JOBJECT = "OBJECT";
    private String KEY_OWNER = "OWNER";

    enum MSG_TYPE
    {
        UPDATE,
        SPAWN
    }

    public void SpawnObject(OnlineObject ob) {
        //Set the owner
        ob.setOwner(PlayerID);

        //insert all json objects
        JObject obj = new JObject(
            new JProperty(KEY_OBJECT_TYPE, ob.GetType().ToString()), //obj type
            new JProperty(KEY_MSG_TYPE, MSG_TYPE.SPAWN.ToString()), // msg type
            new JProperty(KEY_OWNER, PlayerID), //the player owner
            new JProperty(KEY_JOBJECT, ob.getJSONObject())  //the jobject
        );

        //set the msg content and send
        NRawGameMessage msg = new NRawGameMessage();
        msg.setContent(obj.ToString());
        SendGameMessage(msg);

        //add it locally
        addInGameObject(ob);
    }

    public void UpdateObject(JObject obj, Type type) {

        //insert all json objects
        JObject root = new JObject(
            new JProperty(KEY_OBJECT_TYPE, type.ToString()), //obj type
            new JProperty(KEY_MSG_TYPE, MSG_TYPE.UPDATE.ToString()), // msg type
            new JProperty(KEY_OWNER, PlayerID), //the player owner
            new JProperty(KEY_JOBJECT, obj)  //the jobject
        );

        //set the msg content and send
        NRawGameMessage msg = new NRawGameMessage();
        msg.setContent(root.ToString());
        SendGameMessage(msg);

    }

    private void addInGameObject(OnlineObject o) {

        lock (TanksGame.Lock) {

            tankGame.AddGameObject(o);

        }

    }

    private void SendGameMessage(NRawGameMessage msg) {
        nugetta.SendGameMessage(gameID, msg);
    }

    private void recieveGameMessage(String json) {
        JObject obj = JObject.Parse(json);
        if (obj[KEY_MSG_TYPE].ToString() == MSG_TYPE.SPAWN.ToString()) {

            String cls = obj[KEY_OBJECT_TYPE].ToString();
            OnlineObject spawnedObj;

            Console.WriteLine("Game Server Spawn " + cls);

            if (cls == ClassNames.RegularMissile) {

                spawnedObj = new RegularMissile(Vector2.Zero, Vector2.Zero).CreateFromGameMessage(
                                obj[KEY_JOBJECT].ToObject<JObject>(),
                                obj[KEY_OWNER].ToString())
                                as OnlineObject;

                addInGameObject((RegularMissile)(spawnedObj));

            }

        }
        else if (obj[KEY_MSG_TYPE].ToString() == MSG_TYPE.UPDATE.ToString()) {

            String cls = obj[KEY_OBJECT_TYPE].ToString();

            if (cls == ClassNames.TanksGame) {

                tankGame.OnlineUpdate(
                    obj[KEY_JOBJECT].ToObject<JObject>(), 
                    obj[KEY_OWNER].ToString());

            }

        }

    }

    public void JoinGame(String gameID) {
        nugetta.JoinGame(gameID, (JoinGameResponse response) =>
        {
            if (response.getJoinGameStatus() == JoinGameStatus.GAME_NOT_FOUND) {
                Console.WriteLine("Step 1 : Game not found, provide a valid game id");
            }
            else
                if (response.getJoinGameStatus() == JoinGameStatus.ACCEPTED) {
                    Console.WriteLine("Step 1 : Join Game successful");
                    this.gameID = gameID;
                    startGame(response.getGame());     
                }
                else {
                    Console.WriteLine("Step 1 : Failed to join Game ");
                }
        });
    }

    private void startGame(NGame game) {

        nugetta.ListenToMessages((NRawGameMessage message) =>
        {
            recieveGameMessage(message.getContent());
        });

        tankGame = new TanksGame(this);
    }

    public void ExitGame() {

    }

    public void PauseGame() {

    }

    public void ResumeGame() {

    }

}

class MyController{

    private static Controller controller = null;

    public static Controller getInstance(){
        return controller;
    }

    public static void StartUp(TanksGame tankGame){
        controller = new Controller(tankGame);
    }

}

class ClassNames
{

    public static String Projectile { get { return typeof(Projectile).ToString(); } }

    public static String RegularMissile { get { return typeof(RegularMissile).ToString(); } }

    public static String TanksGame { get { return typeof(TanksGame).ToString(); } }

}