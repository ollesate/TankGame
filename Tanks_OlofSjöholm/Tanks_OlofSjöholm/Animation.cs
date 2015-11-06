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

namespace Tanks_OlofSjöholm {
	class Animation{

		private int currentFrame;
		private int maxFrames;
		private int timeSinceLastFrame;

		public int Delay;
        public Rectangle SourceRectangle;

		public Animation(Rectangle SourceRectangle, int MaxFrames){

			timeSinceLastFrame = 0;
			currentFrame = 0;
			Delay = 1000;
			this.maxFrames = MaxFrames;
			this.SourceRectangle = SourceRectangle;

		}


		public void Update(GameTime gametime) {

				timeSinceLastFrame += gametime.ElapsedGameTime.Milliseconds;

				if (timeSinceLastFrame >= Delay) {
					timeSinceLastFrame -= Delay;
					currentFrame++;

					currentFrame = Calc.Mod(currentFrame, maxFrames);

					SourceRectangle = new Rectangle(currentFrame * SourceRectangle.Width, SourceRectangle.Y, SourceRectangle.Width, SourceRectangle.Height); 
				}

		}

        public void Update(GameTime gametime, int speed) {

            timeSinceLastFrame += gametime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame >= Delay) {
                timeSinceLastFrame -= Delay;
                currentFrame += speed;

                currentFrame = Calc.Mod(currentFrame, maxFrames);

                SourceRectangle = new Rectangle(currentFrame * SourceRectangle.Width, SourceRectangle.Y, SourceRectangle.Width, SourceRectangle.Height);
            }

        }

		public void NextFrame() {

			//currentFrame++;

			//if (currentFrame >= maxFrames)
			//    currentFrame = 0;


			////SourceRectangle = new Rectangle((int)(currentFrame * (Size.X)), 0, (int)(Size.X), (int)Size.Y);

		}

		public void SetFrame(int frame) {
			//currentFrame = frame;
			//SourceRectangle = new Rectangle((int)(currentFrame * (Size.X)), 0, (int)(Size.X), (int)Size.Y);
		}

	}
}
