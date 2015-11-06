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
	class Particle {


		private Vector2 pos;
		private float length;
		private Color color;
		private float duration;
		public bool Active;
		private float speed;
		private float angle;
        private Texture2D texture;
        private Rectangle sourceRectangle;
        private Rectangle rectangle;
		float Size;
		public float Duration;
		
		private int timer;
        private int alpha;
        public double AlphaStartPercentage;





        public Particle(Vector2 pos, float size, float speed, float angle, float length, Color color, Texture2D texture) {

            Active = true;
            this.pos = pos;
            this.Size = size;
            this.speed = speed;
            this.angle = angle;
            this.texture = texture;
            this.length = length;
            //this.sourceRectangle = SourceRectangle;
			rectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)Size, (int)Size);
            this.color = color;
			Duration = 1000;
			Active = true;
			timer = 0;
            alpha = 255;
            AlphaStartPercentage = 0.2;
		}

        //public Particle(int size, float length, Color color, Texture2D texture) {

        //    Active = true;
        //    this.Size = size;
        //    this.texture = texture;
        //    this.length = length;
        //    rectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)Size, (int)Size);
        //    this.color = color;
        //    Duration = 500;//frames
        //    Active = true;
        //    timer = 0;
           
        //}


		public void Draw(SpriteBatch b) {

            if (Active)
                b.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, (int)Size, (int)Size), new Color(color.R, color.G, color.B, alpha));

		}

		public void Update(GameTime gameTime) {

			timer += gameTime.ElapsedGameTime.Milliseconds;


			if (timer >= Duration) {
				Active = false;

			}else if(Active){
				Move();
                alpha = (int)((double)(Duration-timer) / Duration * (255 * AlphaStartPercentage));
				length -= speed;
				if (length < 0)
					Active = false;
			}




			

		}

		public void Move() {
			pos.X += speed * (float)Math.Cos(angle);
			pos.Y += speed * (float)Math.Sin(angle);
		}
	}
}
