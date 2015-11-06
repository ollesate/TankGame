using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tanks_OlofSjöholm
{
    abstract class Projectile : GameObject, OnlineObject
    {
        public enum Type { REGULAR, GRENADE };
		public float Damage;
		protected Rectangle SourceRectangle;
        protected String ownerID;
        private float radius;

        public Projectile(Vector2 Position, float Speed, Vector2 Direction)
            : base(Position, Speed, Direction) {

            Size.X = 32;
            Size.Y = 32;
			Damage = 15;
            radius = 7;

        }

		public Projectile(Vector2 Position, Vector2 Direction)
			: base(Position, Settings.BulletSpeed, Direction) {

			Size.X = 32;
			Size.Y = 32;
			Damage = 15;
			radius = 7;

		}

        public virtual void Update(GameTime gameTime) {

			base.Move();

        }

		public override bool Intersects(Rectangle rect) {

            bool intersect = new Circle(Position.X + Size.X / 2, Position.Y + Size.Y / 2, radius).Intersects(rect);
            return intersect;
		}

		public override bool Intersects(Circle c) {

            bool intersect = new Circle(Position.X + Size.X / 2, Position.Y + Size.Y / 2, radius).Intersects(c);
			return intersect;

		}

        //Online Update
        protected enum MSG
        {
            POSITION,
            DIRECTION
        }

        public JObject getJSONObject() {
            JObject obj = new JObject();
            obj[MSG.DIRECTION.ToString()] = JToken.FromObject(Direction);
            obj[MSG.POSITION.ToString()] = JToken.FromObject(Position);
            return obj;
        }

        public abstract Object CreateFromGameMessage(JObject obj, String ownerID);

        public String getOwner() {
            return ownerID;
        }

        public void setOwner(String owner) {
            this.ownerID = owner;
        }
    }
}
