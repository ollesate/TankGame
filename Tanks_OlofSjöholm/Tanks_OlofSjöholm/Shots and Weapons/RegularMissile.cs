using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.nuggeta.game.core.ngdl.nobjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Tanks_OlofSjöholm {



	class RegularMissile : Projectile {

		public RegularMissile(Vector2 Position, Vector2 Direction)
			: base(Position, Direction) {

            SourceRectangle = new Rectangle((int)(Size.Y * (int)0), (int)(Size.Y * 4), (int)Size.X, (int)Size.Y);

		}

		public override void Update(GameTime gameTime) {
			Move();
		}

		public override void Draw(SpriteBatch sb) {
			sb.Draw(Settings.TextureAnimationSheet, Position + Center, SourceRectangle, Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Center, Settings.Scale, SpriteEffects.None, 0);
		}

        public override Object CreateFromGameMessage(JObject obj, String ownerID) {
            Vector2 dir = obj[MSG.DIRECTION.ToString()].ToObject<Vector2>();
            Vector2 pos = obj[MSG.POSITION.ToString()].ToObject<Vector2>();
            RegularMissile missile = new RegularMissile(pos, dir);
            missile.ownerID = ownerID;
            return (Object)(missile);
        }

	}
}
