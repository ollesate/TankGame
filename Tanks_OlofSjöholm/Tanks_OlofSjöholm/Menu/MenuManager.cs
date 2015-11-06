using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tanks_OlofSjöholm
{
    class MenuManager
    {

        private List<MenuScreen> MenuScreen = new List<MenuScreen>();

        public void Update() {

            foreach (MenuScreen ms in MenuScreen)
                ms.Update();

        }

        public void AddMenuScreen(MenuScreen MenuScreen) {

            this.MenuScreen.Add(MenuScreen);

        }

        public Game1.State GetState(Game1.State GameState) {

            foreach (MenuScreen ms in MenuScreen)
                if (ms.State == GameState) {

                    ms.Update();

                    return ms.GetState(GameState);

                }

            return GameState;

        }
    }
}
