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
	class ParticleEffect {


        List<Particle> particles = new List<Particle>();
		public Color Color;
		public int NumberOfParticles;
		public float Size;
        public Vector2 SizeGenerator;
        public Vector2 AngleGenerator;
        public Vector2 SpeedGenerator;
        public Vector3 ColorGenerator;
		public Texture2D Texture;
		public float Speed;
		//Modes: Alpha toning, duration och eller distance;
        //Fade and Disappear, Last untill, 

		public ParticleEffect() {

			Color = Color.White;
			Speed = 0f;
			NumberOfParticles = 1;
			Size = 50;
			//Enum Duration or no Duration
		
		}

		public void newParticleEffect(int numberOfParticles, Vector2 pos, int size, float speed, Random rand, Texture2D texture, Rectangle SourceRectangle){

            int numbParticles = numberOfParticles;
            

            for (int i = 0; i < numberOfParticles; i++) {
                float angle = (float)(rand.NextDouble() * (2 * Math.PI));
                float length = (float)(rand.NextDouble() * 10f);
                float sp = (float)(rand.NextDouble() * 3f);
                particles.Add(new Particle(pos, size, speed, angle, length, Color, texture));

            }
			

		}

        public void newParticleEffect(int numberOfParticles, Vector2 pos, int size, float speed, Random rand, Texture2D texture) {

            int numbParticles = numberOfParticles;
            float angle, length, sp;
			pos.X -= size/2;
			pos.Y -= size / 2;

            for (int i = 0; i < numberOfParticles; i++) {
                //angle
                if (AngleGenerator != Vector2.Zero)
                    angle = (float)rand.NextDouble() * AngleGenerator.Y + AngleGenerator.X;
                else
                    angle = (float)(rand.NextDouble() * 2 * Math.PI);


                length = (float)(rand.NextDouble() * 50);
                particles.Add(new Particle(pos, size, speed, angle, length, Color, texture));
            }


        }



		public void newParticleEffect(Vector2 pos) {

			pos.X -= Size / 2;
			pos.Y -= Size / 2;

            float angle, length, speed;
            for (int i = 0; i < NumberOfParticles; i++) {
                //angle
                if (AngleGenerator != Vector2.Zero)
                    angle = (float)Game1.Random.NextDouble() * (AngleGenerator.Y-AngleGenerator.X) + AngleGenerator.X;
                else
                    angle = (float)(Game1.Random.NextDouble() * 2 * Math.PI);

                //speed
                if (SpeedGenerator != Vector2.Zero)
                    speed = (float)Game1.Random.NextDouble() * (SpeedGenerator.Y - SpeedGenerator.X) + SpeedGenerator.X;
                else 
                    speed = Speed;



                length = (float)(Game1.Random.NextDouble() * 5000000);
                particles.Add(new Particle(pos, Size, speed, angle, length, Color, Texture));
            }

		}

        public void Update(GameTime gameTime) {

			
			foreach (Particle p in particles) {

				p.Update(gameTime);

			}

			for (int i = particles.Count - 1; i >= 0; i--) {
				//Sophantering
				if (!particles.ElementAt(i).Active) {
					particles.RemoveAt(i);

				}
			}
        }


		public void Draw(SpriteBatch b) {

            foreach (Particle p in particles)
                p.Draw(b);
        }

        private Color getColor(int index) {
			switch (index) {
				case 0:
					return Color.Yellow;
				case 1:
					return Color.Green;
				case 2:
					return Color.Red;
				case 3:
					return Color.Blue;
				case 4:
					return Color.Cyan;
				default:
					return Color.Black;
			}
		
		}
	}
}
