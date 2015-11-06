using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using com.nuggeta;
using com.nuggeta.api;
using com.nuggeta.game.core.ngdl.nobjects;
using com.nuggeta.network.plug;
using com.nuggeta.game.core.api.handlers;
using com.nuggeta.ngdl.nobjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Nugetta
{
    class NugettaHandler : NSample
    {
        public NugettaHandler(String url)
            : base(url)
		{

		}

        public NPlayer Player;

        override public void run() {
            nuggetaPlug.setStartResponseHandler((StartResponse startresponse) =>
            {
                if (startresponse.getStartStatus() == StartStatus.READY) {
                    sampleIO.connected();
                    sampleIO.log("Hello NUGGETA!");
                    Player = startresponse.getPlayer();
                }
                else {
                    sampleIO.log("Nuggeta start failed");
                }
            });
            nuggetaPlug.start();
        }

        public void FindGames() {
            findGames();
        }

        virtual protected void findGames() {
            NGame game = new NGame();
            game.setName("My Game");
            NGameCharacteristics gameCharacteristics = new NGameCharacteristics();
            gameCharacteristics.setAutoStop(true);
            game.setGameCharacteristics(gameCharacteristics);
            sampleIO.log("Step 1 : Find games / Useful to build a games lobby.");
            NuggetaQuery nuggetaQuery = new NuggetaQuery();
            
            gameApi.getGamesRequest(nuggetaQuery, (GetGamesResponse getgamesresponse) =>
            {
                List<NGame> games = getgamesresponse.getGames();
                int gamesCount = games.Count;
                if (gamesCount > 0) {
                    sampleIO.log("Step 1 : Found " + gamesCount + " games. Select one an join it with the 'JoinGameSample'");
                    for (int i = 0; i < gamesCount; i++) {
                        NGame findGame = games[i];
                        sampleIO.log("Game name/id :  " + findGame.getId());
                    }
                }
                else {
                    sampleIO.log("Step 1 : No game Found. Create one and relaunch the sample to see what happens");
                }
            });

            
        }

        public void SendGameMessage(String msg, String gameID){
            NRawGameMessage rawGameMessage = new NRawGameMessage();
            rawGameMessage.setContent(msg);
            gameApi.sendMessageToGame(gameID, rawGameMessage);
        }

        public void SendGameMessage(String gameID, NRawGameMessage msg) {
            gameApi.sendMessageToGame(gameID, msg);
        }

        public void FindGames(GetGamesResponseHandler handler) {
            NuggetaQuery nugettaQuery = new NuggetaQuery();
            gameApi.getGamesRequest(nugettaQuery, handler);
        }

        private String gameID;

        public void JoinGame(String gameID, JoinGameResponseHandler handler) {
            gameApi.joinGameRequest(gameID, handler);
            this.gameID = gameID;
        }

        public void ListenToMessages(NRawGameMessageHandler handler) {
            nuggetaPlug.addNRawGameMessageHandler(handler);
            nuggetaPlug.start();
        }

        public void SpawnObject(Object obj){
            JObject o = new JObject();
            o["OBJECT"] = JToken.FromObject(obj);
            o["TYPE"] = "SPAWN";
            String output = o.ToString();
            SendGameMessage(output, gameID);
        }

    }
}
