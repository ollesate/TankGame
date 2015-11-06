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
	class Tile {

		public Rectangle Rectangle;
		
        
		public Vector2 Position;
		public Vector2 Size;
        public Vector2 CenterPosition { get { return new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2); } }

		public Tile(Vector2 Position) {

			this.Position = Position;
			Size = new Vector2(Settings.TileSize, Settings.TileSize);
			Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);



		}

	}
}
