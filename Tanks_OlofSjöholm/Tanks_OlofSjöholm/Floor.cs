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
    class Floor : Tile
    {

        private Rectangle sourceRectangel;

        public Floor(Vector2 Position)
            : base(Position) {

                sourceRectangel = new Rectangle((int)(32 * 1) - 1, (int)(32 * 0), (int)32, (int)32);

        }

        public virtual void Draw(SpriteBatch sb) {

                sb.Draw(Settings.TextureAnimationSheet, Rectangle, sourceRectangel, Color.White);

        }
    }
}
