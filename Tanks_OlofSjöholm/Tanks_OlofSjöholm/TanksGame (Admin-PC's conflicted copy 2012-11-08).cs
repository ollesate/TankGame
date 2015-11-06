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

namespace Tanks_OlofSjöholm {
	class TanksGame {

		Player player1;

		public TanksGame() {

			LoadContent();

		}

		public void LoadContent() {

			player1 = new Player(new Vector2(0, 0), 5f);

		}
		
		public void Update(GameTime gameTime) {

			player1.Update(gameTime);

            if (player1.Position.X + player1.Size.X > Settings.ScreenWidth)
                player1.Position.X = Settings.ScreenWidth - player1.Size.X;
            if (player1.Position.X < 0)
                player1.Position.X = 0;
            if (player1.Position.Y + player1.Size.Y > Settings.ScreenHeight)
                player1.Position.Y = Settings.ScreenHeight - player1.Size.Y;
            if (player1.Position.Y < 0)
                player1.Position.Y = 0;

		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

			player1.Draw(spriteBatch);

		}

	}
}
