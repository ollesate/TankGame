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
    class Wall : Tile
    {

        public enum Type { DESTRUCTABLE, INDESTRUCTABLE };
        private Type type;
        private Rectangle sourceRectangel;
        private int lives;
        

        public Wall(Vector2 Position, Type type)
            : base(Position) {

            lives = 3;
            this.type = type;
            sourceRectangel = new Rectangle((int)(32 * 1) - 1, (int)(32 * 1), (int)32, (int)32);

        }

        public virtual bool Intersects(GameObject ob) {

                return Rectangle.Intersects(ob.Rectangle);

        }

        public virtual void Draw(SpriteBatch sb) {

            if (type == Type.DESTRUCTABLE) {
                sb.Draw(Settings.TextureWoodWall, Rectangle, Color.White);
                sb.Draw(Settings.TextureCracksAnimationSheet, Rectangle, new Rectangle((int)(32 * (3-lives)), (int)(32 * 0), (int)32, (int)32), Color.White);
            }
            else if (type == Type.INDESTRUCTABLE)
                sb.Draw(Settings.TextureAnimationSheet, Rectangle, sourceRectangel, Color.White);

        }

        public void Update() {



        }

        public bool isDestroyed(){

            return (lives <= 0);

        }

        public void Hit() {

            if (type == Type.DESTRUCTABLE) {
                if(lives > 0)
                lives--;
            }

        }
    }
}
