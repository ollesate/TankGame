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
	class PowerUp: GameObject{

		private Rectangle sourceRectangle;
		public enum Type { REGULAR, GRENADE, DAMAGE, SPEED, FIRERATE, HEALTH,};
		private Type type;

		public PowerUp(Vector2 Position, Type Type)
			: base(Position) {

			this.type = Type;
			Size = new Vector2(Settings.PowerUpSize, Settings.PowerUpSize);
			sourceRectangle = new Rectangle((int)(32 * 3), (int)(32 * 4), (int)32, (int)32);

		}


		public override void Draw(SpriteBatch sb) {
			
			switch (type) {
				case Type.REGULAR:
					break;
				case Type.GRENADE:
					sb.Draw(Settings.TextureAnimationSheet, Position + Center, sourceRectangle, Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Center, Settings.Scale, SpriteEffects.None, 0);
					break;
				case Type.DAMAGE:
					sb.Draw(Settings.TexturePowerUp, Position + Center, new Rectangle((int)(32 * ((int)type-2)), 0, 32, 32), Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Center, Settings.Scale, SpriteEffects.None, 0);
					break;
				case Type.SPEED:
					sb.Draw(Settings.TexturePowerUp, Position + Center, new Rectangle((int)(32 * ((int)type - 2)), 0, 32, 32), Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Center, Settings.Scale, SpriteEffects.None, 0);
					break;
				case Type.FIRERATE:
					sb.Draw(Settings.TexturePowerUp, Position + Center, new Rectangle((int)(32 * ((int)type - 2)), 0, 32, 32), Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Center, Settings.Scale, SpriteEffects.None, 0);
					break;
				case Type.HEALTH:
					sb.Draw(Settings.TexturePowerUp, Position + Center, new Rectangle((int)(32 * ((int)type - 2)), 0, 32, 32), Color.White, (float)Math.Atan2(Direction.Y, Direction.X), Center, Settings.Scale, SpriteEffects.None, 0);
					break;
				default:
					break;
			}

			

		}

		public void Hit(Player p) {

			//Settings.SoundPowerUp.Play();


			switch (type) {
				case Type.REGULAR:
					break;
				case Type.GRENADE:
					p.BulletType = Projectile.Type.GRENADE;
					break;
				case Type.DAMAGE:
					//p.Size *= 1.3f;
					break;
				case Type.SPEED:
					p.Speed *= 1.3f;
					break;
				case Type.FIRERATE:
					p.ReloadTime =  (int)(p.ReloadTime *.7f);
					break;
				case Type.HEALTH:
					p.SetHealth(p.CurrentHealth + 50);
					break;
				default:
					break;
			}

		}

		public static Type RandomizePowerUp() {



				int i = Game1.Random.Next(Enum.GetValues(typeof(Type)).Length);
				return (Type)Enum.GetValues(typeof(Type)).GetValue(i);


		}
	}
}
