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

	class GameObject {


		
		public float Speed;
		protected Texture2D Texture;
        public Vector2 Direction;
		public Vector2 Size;
        public Vector2 TextureSize {
            get { return (Texture == null) ? Vector2.Zero : new Vector2(Texture.Width, Texture.Height); }
        }
		public Vector2 Position;
		public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); }}
		public Vector2 Center { get { return new Vector2(Size.X / 2, Size.Y / 2); } }
		public Vector2 CenterPosition { get { return new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2); } }
        public Boolean Active = true;

		public GameObject(Texture2D Texture, Vector2 Position) {
			this.Position = Position;
			this.Texture = Texture;
			Size = new Vector2(Texture.Width, Texture.Height);
		}

		public GameObject(Texture2D Texture, Vector2 Position, float Speed) {
			this.Texture = Texture;
			Size = new Vector2(Texture.Width, Texture.Height);
			this.Position = Position;
			this.Speed = Speed;
		}

		public GameObject(Vector2 Position, float Speed) {
			this.Position = Position;
			this.Speed = Speed;
		}

		public GameObject(Vector2 Position) {
			this.Position = Position;
		}

        public GameObject(Vector2 Position, float Speed, Vector2 Direction) {
            this.Direction = Direction;
            this.Position = Position;
            this.Speed = Speed;
        }

        public GameObject() {

        }

		public virtual void Move() {
            Position += Direction * Speed;
		}

		public virtual void StandardMove() {
			
		}

		public virtual void Draw(SpriteBatch sb) {
			
			sb.Draw(Texture, Rectangle, Color.White);

		}

		public virtual void RotateAndDraw(SpriteBatch sb, float rotation) {

		}

		public virtual bool Intersects(GameObject ob) {
			return Rectangle.Intersects(ob.Rectangle);
		}

		public virtual bool Intersects(Rectangle rect) {
			return Rectangle.Intersects(rect);
		}

		public virtual bool Intersects(Circle circl) {
			return true;
		}



	}
