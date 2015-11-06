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

namespace Tanks_OlofSjöholm
{
    class ViewGamesGameScreen : MenuScreen
    {

        private bool initialized = false;
        
        public ViewGamesGameScreen(Rectangle window, Game1.State State)
            : base(window, State) {
                //onInitialize();
        }

        private void onInitialize() {
            joinGame();
        }

        private void findGames() {
            Nugetta.NugettaHandler nugettaHandler = Nugetta.MyNugettaHandler.getInstance();
            nugettaHandler.FindGames();
        }

        private void joinGame() {
            Nugetta.NugettaHandler nugettaHandler = Nugetta.MyNugettaHandler.getInstance();
            //nugettaHandler.JoinGame("MyGameServer3799ac24-8ee3-4b4f-a86d-bc8273b7258f");
        }

        public override void Update() {
            base.Update();
            if (!initialized) {
                onInitialize();
                initialized = true;
            }
        }

    }
}
