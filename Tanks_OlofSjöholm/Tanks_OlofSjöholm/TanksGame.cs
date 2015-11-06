using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using com.nuggeta.game.core.ngdl.nobjects;
using com.nuggeta.game.core.api.handlers;
using Nugetta;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tanks_OlofSjöholm {

	class TanksGame {

       public enum GameState{
            PLAYING,
            PAUSED
        }
        private GameState gameState;


        private Player player1;
        private Player player2;

        private List<GameObject> immovableObjects = new List<GameObject>();
        private List<GameObject> movableObjects = new List<GameObject>();

        private List<Projectile> projectiles = new List<Projectile>();

        private List<Player> players = new List<Player>();
        private List<Wall> wallList = new List<Wall>();
        private List<Floor> floorList = new List<Floor>();
		private List<PowerUp> powerUpList = new List<PowerUp>();

        private List<OnlineObject> onlineObjects = new List<OnlineObject>();

        private ParticleEffect particleEngine;
		private List<ParticleEffect> particleEffectList = new List<ParticleEffect>();
        private Random rand = new Random();
        private NGame nGame;
        private Map map;

        private Controller controller;


        private class JSONValue
        {
            
            public String Name;
            public Object Value;
            public type Type;

            public static String getValueKey(){
                return "Value";
            }

            public static String getNameKey() {
                return "Name";
            }

            public static String getTypeKey() {
                return "Type";
            }

            public override String ToString() {
                return Value.ToString();
            }

            public enum type
            {
                DOUBLE,
                FLOAT,
                INT,
                STRING,
                VECTOR2,
                BOOLEAN
            }
            
            public void putDouble(Double val){
                Value = val;
                Type = type.DOUBLE;
            }

            public void putFloat(float val) {
                Value = val;
                Type = type.FLOAT;
            }

            public void putInt(int val) {
                Value = val;
                Type = type.INT;
            }

            public void putString(String val) {
                Value = val;
                Type = type.STRING;
            }

            public void putVector2(Vector2 val) {
                Value = val;
                Type = type.VECTOR2;
            }
            public void putBool(Boolean val) {
                Value = val;
                Type = type.BOOLEAN;
            }

        }

        private class ExampleValue
        {
            public Vector2 position;
            public Vector2 direction;
            public Vector2 turretDirection;
        }



        public void SetGameState(GameState gameState){
            this.gameState = gameState;
        }

		public TanksGame() {
            gameState = GameState.PLAYING;
			LoadContent();
            particleEngine = new ParticleEffect();
		}

        public TanksGame(Controller controller) {
            //Initialize
            gameState = GameState.PLAYING;
            LoadContent();
            particleEngine = new ParticleEffect();
            this.controller = controller;
            player1.PlayerID = controller.PlayerID;
        }

        private void gameMessagesProtocol(JObject obj) {
            //String name = obj[JSONValue.getNameKey()].ToString();
            //JSONValue.type type = obj[JSONValue.getNameKey()].ToObject<JSONValue.type>();

            player2.Position = obj["position"].ToObject<Vector2>();
            player2.Direction = obj["direction"].ToObject<Vector2>();
            player2.TurretDirection = obj["turretDirection"].ToObject<Vector2>();

            //switch (name) {
            //    case "POSITION":

            //        player2.Position = obj[JSONValue.getValueKey()].ToObject<Vector2>();

            //        break;
            //    case "TURRETANGLE":

            //        player2.TurretDirection = obj[JSONValue.getValueKey()].ToObject<Vector2>();
            //        player2.TurretDirection.Normalize();

            //        break;

            //    case "DIRECTION":

            //        player2.Direction = obj[JSONValue.getValueKey()].ToObject<Vector2>();
            //        player2.Direction.Normalize();


            //        break;

            //    default:
            //        break;
            //}

            //player2.Position = obj[JSONValue.getValueKey()].ToObject<Vector2>();
        }

        private String KEY_POSITION = "POSITION";
        private String KEY_DIRECTION = "DIRECTION";
        private String KEY_TURRET_DIRECTION = "TURRET_DIRECTION";
        private String KEY_ALIVE = "ALIVE";

        private JObject getGameUpdates() {
            JObject obj = new JObject();
            obj[KEY_POSITION] = JToken.FromObject(player1.Position);
            obj[KEY_DIRECTION] = JToken.FromObject(player1.Direction);
            obj[KEY_TURRET_DIRECTION] = JToken.FromObject(player1.TurretDirection);
            return obj;
        }

        public void OnlineUpdate(JObject obj, String ownerID) {
            player2.PlayerID = ownerID;
            //player2.Position = obj[KEY_POSITION].ToObject<Vector2>();
            //player2.Direction = obj[KEY_DIRECTION].ToObject<Vector2>();
            //player2.TurretDirection = obj[KEY_TURRET_DIRECTION].ToObject<Vector2>();

            JsonConvert.PopulateObject(obj.ToString(), player2);

        }

        private void sendGameMessages() {

            if (!player1.Updated) {
                return;
            }

            player1.Updated = false;

            addToOnlineUpdates(Fields.Position, player1.Position);
            addToOnlineUpdates(Fields.Direction, player1.Direction);
            addToOnlineUpdates(Fields.TurretDirection, player1.TurretDirection);



            controller.UpdateObject(getData(), this.GetType());

            //controller.UpdateObject(getGameUpdates(), this.GetType());

            //JSONValue val = new JSONValue();

            //ExampleValue val = new ExampleValue();
            //val.position = player1.Position;
            //val.direction = player1.Direction;
            //val.turretDirection = player1.TurretDirection;

            //String output = JsonConvert.SerializeObject(val);

            /////////////////handler.SendGameMessage(output, nGame.getId());

            //output = JsonConvert.SerializeObject(val);
            //handler.SendGameMessage(output, nGame.getId());

            //sendGameMessage("POSITION", player1.Position);
            ////sendGameMessage("TURRETANGLE", player1.TurretDirection);
            //sendGameMessage("DIRECTION", player1.Direction);
        }

        private void sendGameMessage(String name, Vector2 value) {
            JSONValue val;
            String output;

            val = new JSONValue();
            val.Name = name;
            val.putVector2(value);

            output = JsonConvert.SerializeObject(val);
            //handler.SendGameMessage(output, nGame.getId());
        }

        private void LoadContent() {

            if (GamePad.GetState(PlayerIndex.One).IsConnected) {
                player1 = new Player(new Vector2(0, 0), Player.Controller.XBOX_ANALOG);
                player2 = new Player(new Vector2(Settings.ScreenWidth - Settings.TankSize.X, Settings.ScreenHeight - Settings.TankSize.Y), Player.Controller.PC_Full);
            }
            else {
                player1 = new Player(new Vector2(0, 0), Player.Controller.PC_Player1);
                player2 = new Player(new Vector2(Settings.ScreenWidth - Settings.TankSize.X, Settings.ScreenHeight - Settings.TankSize.Y), Player.Controller.PC_Player2);
            }

			players.Add(player1);
			player2.Direction = new Vector2(-1, 0);
			player2.TurretDirection = new Vector2(-1, 0);
			player2.Color = Color.Red;
			players.Add(player2);

            

			map = new Map();
			map.LoadMap();

            int[,] currentMap = map.Map1;

            for (int y = 0; y < currentMap.GetLength(0); y++)
                for (int x = 0; x < currentMap.GetLength(1); x++) {
                    if (currentMap[y, x] == 1) {
                        wallList.Add(new Wall(new Vector2(x * Settings.TileSize, y * Settings.TileSize), Wall.Type.INDESTRUCTABLE));
                    }
                    else if (currentMap[y, x] == 2) {
                        wallList.Add(new Wall(new Vector2(x * Settings.TileSize, y * Settings.TileSize), Wall.Type.DESTRUCTABLE));
                    }

                    floorList.Add(new Floor(new Vector2(x * Settings.TileSize, y * Settings.TileSize)));
				}
			

		}

        ///////////////// ONLINE DATA
        Dictionary<String, JToken> dictionary = new Dictionary<String,JToken>();

        public void populateData(Newtonsoft.Json.Linq.JObject obj) {
            Newtonsoft.Json.JsonConvert.PopulateObject(obj.ToString(), this);
        }

        //public Newtonsoft.Json.Linq.JObject getData() {
        //    Newtonsoft.Json.Linq.JObject obj = new Newtonsoft.Json.Linq.JObject();
        //    obj[Fields.Position.ToString()] = Newtonsoft.Json.Linq.JToken.FromObject("posPos");
        //    obj[Fields.Direction.ToString()] = Newtonsoft.Json.Linq.JToken.FromObject("postDir");
        //    return obj;
        //}

        public JObject getData() {
            JObject obj = new JObject();
            foreach (var item in dictionary) {
                obj[item.Key] = item.Value;
            }
            clearData();
            return obj;
        }

        private void addToOnlineUpdates(Fields key, Object val) {
            dictionary[key.ToString()] = JToken.FromObject(val);
        }

        private void clearData() {
            dictionary.Clear();
        }

        enum Fields
        {
            Position,
            Direction,
            TurretDirection,
            COUNT
        }
        /////////////////////////////////

        public static object Lock = new object();
		
		public void Update(GameTime gameTime) {

            if(gameState == GameState.PAUSED)
                return;

            Player p = player1;
			p.Update(gameTime);
			TileCollision(p);//Move sker här
            ScreenBorderCollision(p);
			
			PowerUpCollision(p);
			

			UpdateGameCondition(); //kollar om någon dör
			particleEngine.Update(gameTime);

			foreach (ParticleEffect pe in particleEffectList)
				pe.Update(gameTime);


            lock (Lock) {

                ProjectileCollision();

                foreach (OnlineObject o in onlineObjects)
                    (o as Projectile).Update(gameTime);

            }

            sendGameMessages();

            clearTrash();
		}

        private void clearTrash() {
            onlineObjects.RemoveAll(o => !(o as GameObject).Active);
            projectiles.RemoveAll(o => !(o as GameObject).Active);
        }

		private void PowerUpCollision(Player p) {

			foreach (PowerUp pu in powerUpList) {

				if (p.Intersects(pu)) {
					pu.Hit(p);
					
					powerUpList.Remove(pu);
					break;
				}

			}

		}

		private void UpdateGameCondition() {

            if (player1.CurrentHealth <= 0) {
                Settings.Player2Score++;
                Game1.Execute = Game1.GameCommands.RESETGAME;
            }
            else if (player2.CurrentHealth <= 0) {
                Settings.Player1Score++;
                Game1.Execute = Game1.GameCommands.RESETGAME;
            }

		}

		private void ScreenBorderCollision(Player p) {

			if (p.Position.X + p.Size.X * Settings.Scale > Settings.ScreenWidth)
				p.Position.X = Settings.ScreenWidth - p.Size.X * Settings.Scale;
			if (p.Position.X < 0)
				p.Position.X = 0;
			if (p.Position.Y + p.Size.Y * Settings.Scale > Settings.ScreenHeight)
				p.Position.Y = Settings.ScreenHeight - p.Size.Y * Settings.Scale;
			if (p.Position.Y < 0)
				p.Position.Y = 0;

		}

        private void ProjectileCollision() {

			//Bullet Player Collision
            foreach (Projectile proj in projectiles)
				foreach (Player player in players) {
                    if (proj.getOwner() == player.PlayerID)
                        continue;

					if ( proj.Intersects(player.Circle)) {

						player.Hit(proj.Damage);
						if (proj is GrenadeMissile)
							particleEffectList.Add(((GrenadeMissile)proj).smokeTrail);
                        proj.Active = false;
						return;
					}
				}
			

			//Tiles
            foreach (Projectile proj in projectiles) 
                foreach (Wall w in wallList) {

                    if (proj.Intersects(w.Rectangle)) {
                        w.Hit();
                        if (w.isDestroyed()) {

                            particleEngine.newParticleEffect(25, w.CenterPosition, 12, 5f, rand, Settings.TextureWoodWall);
							if (Game1.Random.NextDouble() > 0.5)
							powerUpList.Add(new PowerUp(w.Position + new Vector2((Settings.TileSize - Settings.PowerUpSize)/2, (Settings.TileSize - Settings.PowerUpSize)/2)
								,PowerUp.RandomizePowerUp()));
                            wallList.Remove(w);

                        }
                        proj.Active = false;
						if (proj is GrenadeMissile)
							particleEffectList.Add(((GrenadeMissile)proj).smokeTrail);
                        return;
                    }

                }
            
            //Remove when out of screen
            foreach (Projectile proj in projectiles) {
                if(!proj.Intersects(new Rectangle(0, 0, Settings.ScreenWidth, Settings.ScreenHeight))){
                    proj.Active = false;
                }
            }

        }

        public void AddGameObject(OnlineObject obj) {
            onlineObjects.Add(obj);
            if (obj is Projectile) {
                projectiles.Add(obj as Projectile);
            }
        }

        private void TileCollision(Player p) {

            p.Position.X += p.Velocity.X;

            foreach (Wall t in wallList) {
                if (t.Intersects(p))
                    if (p.Velocity.X > 0) {
                        p.Position.X = t.Position.X - p.Size.X;
                        Vector2 temp = new Vector2(p.Direction.X, p.Direction.Y * 1.05f);
                        p.TurretTurn(Calc.AngleBetween(p.Direction, temp));
                        p.TurretDirection.Normalize();
                        p.Direction = temp;
                        p.Direction.Normalize();
                    }

                    else {
                        p.Position.X = t.Position.X + t.Size.X;
                        Vector2 temp = new Vector2(p.Direction.X, p.Direction.Y * 1.05f);
                        p.TurretTurn(Calc.AngleBetween(p.Direction, temp));
                        p.TurretDirection.Normalize();
                        p.Direction = temp;
                        p.Direction.Normalize();
                    }
            }

            p.Position.Y += p.Velocity.Y;

            foreach (Wall t in wallList) {
                if (t.Intersects(p))
                    if (p.Velocity.Y > 0) {
                        p.Position.Y = t.Position.Y - p.Size.Y;
                        Vector2 temp = new Vector2(p.Direction.X * 1.05f, p.Direction.Y);
                        p.TurretTurn(Calc.AngleBetween(p.Direction, temp));
                        p.TurretDirection.Normalize();
                        p.Direction = temp;
                        p.Direction.Normalize();
                    }
                    else {
                        p.Position.Y = t.Position.Y + t.Size.Y;
                        Vector2 temp = new Vector2(p.Direction.X * 1.05f, p.Direction.Y);
                        p.TurretTurn(Calc.AngleBetween(p.Direction, temp));
                        p.TurretDirection.Normalize();
                        p.Direction = temp;
                        p.Direction.Normalize();
                    }
            }




        }

		public void Draw(SpriteBatch spriteBatch) {

            foreach (Floor t in floorList)
                t.Draw(spriteBatch);

			foreach (Wall t in wallList)
				t.Draw(spriteBatch);

			foreach (PowerUp p in powerUpList)
				p.Draw(spriteBatch);

			foreach (Player p in players)
				p.Draw(spriteBatch);

            particleEngine.Draw(spriteBatch);

			foreach (ParticleEffect pe in particleEffectList)
				pe.Draw(spriteBatch);

            foreach (OnlineObject o in onlineObjects)
                (o as Projectile).Draw(spriteBatch);
		}

	}
}
