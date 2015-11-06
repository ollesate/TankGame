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
    class MenuScreen
    {
		private List<MenuItem> ItemList = new List<MenuItem>();
        private int target;
        private KeyboardState previousKeyState;
        private SpriteFont font;
        private int lineHeight;
        private Rectangle window;
        public String Title;
        public Game1.State State;
        public int XAlignment;
        public int YAlignment;
        public Color FontColor;
        public Color TargetColor;
        


        public MenuScreen(Rectangle window, Game1.State State) {
            this.State = State;
            this.window = window;
            lineHeight = 56;
            target = 0;//markerad är den första
            font = Settings.FontStandard;
            setAlignment();
            Title = null;
            TargetColor = Color.White;
        }

        private void setAlignment() {

            XAlignment = window.Center.X;
            YAlignment = window.Center.Y/2;

            XAlignment += -80;
            YAlignment += -80;
        }

        public void AddTitel(String Title) {

            this.Title = Title;

        }

        public void AddStateMenuItem(String name, Game1.State action) {

			ItemList.Add(new ActionItem(name, action));

        }

		public void AddSettingMenuItem(String name, ChangeableFloat value) {

			ItemList.Add(new SettingItem(name, value));

		}

        public void SetPreviousKey(KeyboardState state) {

            previousKeyState = state;

        }

        public virtual void Update() {

            //flytta markerad objet
            if (KeyMouseReader.KeyClick(Keys.Up))
                target--;
            else if (KeyMouseReader.KeyClick(Keys.Down))
                target++;

            
            if(ItemList.Count>0)
            target = Calc.Mod(target, ItemList.Count);

        }

        public Game1.State GetState(Game1.State GameState) {

            if (KeyMouseReader.KeyClick(Keys.Enter)) {
				if(ItemList.ElementAt(target) is ActionItem)
				return ((ActionItem)(ItemList.ElementAt(target))).Action;
			}
                

            return 
                State;

        }

        public void Draw(SpriteBatch spriteBatch) {


            if(Title != null)
                spriteBatch.DrawString(Settings.FontStandard, Title, new Vector2(XAlignment, YAlignment), FontColor, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

            foreach (MenuItem m in ItemList) {

                Color tempColor;

                if (ItemList.IndexOf(m) == target)
                    tempColor = TargetColor;
                else
                    tempColor = FontColor;

                spriteBatch.DrawString(Settings.FontStandard, m.Name, new Vector2(XAlignment, YAlignment + (ItemList.IndexOf(m) + 2) * lineHeight), tempColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            }

        }



    }
}
