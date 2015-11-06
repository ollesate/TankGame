using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tanks_OlofSjöholm {
	class GrenadeMissile : Projectile {

		public ParticleEffect smokeTrail;

		public GrenadeMissile(Vector2 Position, Vector2 Direction)
			: base(Position, Direction) {

			SourceRectangle = new Rectangle((int)(Size.Y * (int)1), (int)(Size.Y * 4), (int)Size.X, (int)Size.Y);
			Damage *= 1.5f;
			Speed *= 1.5f;

			smokeTrail = new ParticleEffect();
			smokeTrail.Texture = Settings.TextureParticle;
			smokeTrail.Size = 5;
			smokeTrail.NumberOfParticles = 3;
			smokeTrail.SpeedGenerator = new Vector2(0.1f, 0.2f);

		}

		public override void Update(GameTime gameTime) {


			smokeTrail.newParticleEffect(CenterPosition);
			smokeTrail.Update(gameTime);
			Move();

		}

		public override void Draw(SpriteBatch sb) {

			smokeTrail.Draw(sb);
			sb.Draw(Settings.TextureAnimationSheet, Position + Center, SourceRectangle, Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Center, Settings.Scale, SpriteEffects.None, 0);

		}

        public override Object CreateFromGameMessage(JObject obj, String ownerID) {
            this.ownerID = ownerID;
            Vector2 dir = obj[MSG.DIRECTION.ToString()].ToObject<Vector2>();
            Vector2 pos = obj[MSG.POSITION.ToString()].ToObject<Vector2>();
            return (Object)(new RegularMissile(pos, dir));
        }
	}
}
