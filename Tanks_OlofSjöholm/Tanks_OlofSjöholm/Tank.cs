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
using Nugetta;

namespace Tanks_OlofSjöholm {
	class Tank : GameObject, OnlineObject {

		private Animation ani;
        public Vector2 TurretDirection;
        private Rectangle TurretSourceRectangle;
        private GameTime gameTime;
		private int ammo;
        
        private int bulletTimer;
        public int ReloadTime;
        public Vector2 Velocity;
		public Color Color;
		public Circle Circle{ get{ return new Circle(CenterPosition.X, CenterPosition.Y, Settings.TankSize.X / 2); }}
		private ParticleEffect SmokePuffPuff;
        private ParticleEffect BurnBabyBurn;
		public Projectile.Type BulletType;

		public Tank(Vector2 position) :
			base(position)   {

			Size = Settings.TankSize;
			ani = new Animation(new Rectangle(0, (int)(Size.Y * 2), (int)Size.X, (int)Size.Y), 10);
			ani.Delay = (int)(17 / Settings.TankSpeed) * 3;
			Direction = new Vector2(1, 0);
			TurretDirection = new Vector2(1, 0);
			TurretSourceRectangle = new Rectangle(0, (int)(Size.Y * 3), (int)Size.X, (int)Size.Y);
            
            BulletType = Projectile.Type.REGULAR;
            bulletTimer = 0;
            ReloadTime = Settings.BulletReloadTime;
			Color = Color.White;
			Speed = Settings.TankSpeed;
			ammo = 500;

            //Effects
			SmokePuffPuff = new ParticleEffect();
			SmokePuffPuff.Texture = Settings.TextureParticle;
			SmokePuffPuff.Color = Color.Black;
            SmokePuffPuff.Size = 20;
            SmokePuffPuff.NumberOfParticles = 15;
            SmokePuffPuff.SpeedGenerator = new Vector2(.1f,.5f);
            BurnBabyBurn = new ParticleEffect();
            BurnBabyBurn.Texture = Settings.TextureParticle;
            BurnBabyBurn.Color = Color.OrangeRed;
            BurnBabyBurn.Size = SmokePuffPuff.Size*.5f;
            BurnBabyBurn.NumberOfParticles = 5;
            BurnBabyBurn.SpeedGenerator = SmokePuffPuff.SpeedGenerator*.5f;


		}

		public virtual void Update(GameTime gameTime) {

			SmokePuffPuff.Update(gameTime);
            BurnBabyBurn.Update(gameTime);
            this.gameTime = gameTime;
            bulletTimer += gameTime.ElapsedGameTime.Milliseconds;

		}

		public override void Draw(SpriteBatch spriteBatch){

            //Tank kroppen
			spriteBatch.Draw(Settings.TextureAnimationSheet, Position + Center, ani.SourceRectangle, Color, (float)Math.Atan2(Direction.Y, Direction.X), Center, Settings.Scale, SpriteEffects.None, 0);

            //Tornet
			spriteBatch.Draw(Settings.TextureAnimationSheet, Position + Center, TurretSourceRectangle, Color, (float)Math.Atan2(TurretDirection.Y, TurretDirection.X), Center, Settings.Scale, SpriteEffects.None, 0);

			//Rök
			SmokePuffPuff.Draw(spriteBatch);
            //Eld
            BurnBabyBurn.Draw(spriteBatch);
		
        }

        public override void Move() {
            Position += Velocity;
        }

        public void TurretTurnRight() {

            TurretDirection = Calc.RotateVector(TurretDirection, Settings.TankTurretTurnSpeed);
            TurretDirection.Normalize();

        }

        public void TurretTurnLeft() {

            TurretDirection = Calc.RotateVector(TurretDirection, -Settings.TankTurretTurnSpeed);
            TurretDirection.Normalize();

        }

        public void TurretTurn(double angle) {

            TurretDirection = Calc.RotateVector(TurretDirection, angle);
            TurretDirection.Normalize();

        }

        public void Stop() {

            Velocity = Vector2.Zero;

        }

        public void MoveForward() {

            ani.Update(gameTime, 1);
			Velocity = Direction * Speed;

		}

        public void MoveBackward() {

            ani.Update(gameTime, -1);
			Velocity = -Direction * Speed;

        }

		public void Turn(double angle) {

			Direction = Calc.RotateVector(Direction, angle);
			Direction.Normalize();

			//vänd också för tornet
			TurretDirection = Calc.RotateVector(TurretDirection, angle);
			TurretDirection.Normalize();
		}

		public void TurnLeft(){

            Direction = Calc.RotateVector(Direction, -Settings.TankTurnSpeed);
			Direction.Normalize();

            //vänd också för tornet
            TurretDirection = Calc.RotateVector(TurretDirection, -Settings.TankTurretTurnSpeed); 
            TurretDirection.Normalize();
		}

		public void TurnRight() {

            Direction = Calc.RotateVector(Direction, Settings.TankTurnSpeed); 
            Direction.Normalize();

            //vänd också för tornet
            TurretDirection = Calc.RotateVector(TurretDirection, Settings.TankTurretTurnSpeed);
            TurretDirection.Normalize();
		}

        public void Fire() {

			if (bulletTimer >= ReloadTime && ammo > 0) {
                bulletTimer = 0;
				ammo--;

                Projectile projectile = null;

				if (BulletType == Projectile.Type.REGULAR) {
					projectile = new RegularMissile(Position, TurretDirection);
				} else if (BulletType == Projectile.Type.GRENADE) {
					projectile = new GrenadeMissile(Position, TurretDirection);
				}

                MyController.getInstance().SpawnObject(projectile);

                SmokePuffPuff.AngleGenerator = new Vector2((float)(Calc.AngleOf(TurretDirection) - Math.PI / 6), (float)(Calc.AngleOf(TurretDirection) + Math.PI / 6));
				SmokePuffPuff.newParticleEffect(CenterPosition  + TurretDirection * Size / 2);
                BurnBabyBurn.AngleGenerator = new Vector2((float)(Calc.AngleOf(TurretDirection) - Math.PI / 6), (float)(Calc.AngleOf(TurretDirection) + Math.PI / 6));
                BurnBabyBurn.newParticleEffect(CenterPosition + TurretDirection * Size / 2);

            }
        }

        public void Weapon(Projectile.Type bulletType) {

            TurretSourceRectangle = new Rectangle((int)(Size.X * (int)bulletType), (int)(Size.Y * 3), (int)Size.X, (int)Size.Y);
            this.BulletType = bulletType;

        }

        public Newtonsoft.Json.Linq.JObject getJSONObject() {
            Newtonsoft.Json.Linq.JObject obj = new Newtonsoft.Json.Linq.JObject();
            obj[Fields.Position.ToString()] = Newtonsoft.Json.Linq.JToken.FromObject(Position);
            return obj;
        }

        public object CreateFromGameMessage(Newtonsoft.Json.Linq.JObject obj, string ownerID) {

            Vector2 pos = obj[Fields.Position.ToString()].ToObject<Vector2>();

            Tank tank = new Tank(pos);

            return (Object)(tank);
        }

        public string getOwner() {
            throw new NotImplementedException();
        }

        public void setOwner(string owner) {
            
        }

        enum Fields
        {
            Position
        }
    }
}
