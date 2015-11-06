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
    class Player : Tank
    {

		public enum Controller { XBOX_XA, XBOX_ANALOG, PC_Full, PC_Player1, PC_Player2 };
		private Controller controller;

        public String PlayerID;

		private float maxHealth;
        private Rectangle healthRect;
        private Vector2 healthOffSet;
		public float CurrentHealth;
        public bool Updated;

        public Player(Vector2 position, Controller controller)
            : base(position) {

            this.controller = controller;
            maxHealth = 100;
            CurrentHealth = maxHealth;
            healthRect = new Rectangle(0,0, 32, 5);
            healthOffSet = new Vector2(0, -5);

        }

        public override void Update(GameTime gameTime) {


            keyListener();
            base.Update(gameTime);

        }

        public void keyListener() {

            switch (controller) {
                case Controller.XBOX_XA:
                    Xbox_XA();
                    break;
                case Controller.XBOX_ANALOG:
                    Xbox_Analog();
                    break;
                case Controller.PC_Full:
                    PC_Full();
                    break;
                case Controller.PC_Player1:
                    PC_Player1();
                    break;
                case Controller.PC_Player2:
                    PC_Player2();
                    break;
                default:
                    break;
            }



        }

        private void PC_Player1() {

            if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                MoveForward();
                Updated = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.S)) {
                MoveBackward();
                Updated = true;
            }

            else
                Stop();

            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                TurnLeft();
                Updated = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                TurnRight();
                Updated = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Fire();

        }

        private void PC_Player2() {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                MoveForward();

            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                MoveBackward();

            else
                Stop();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                TurnLeft();

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                TurnRight();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                Fire();

        }

		public void Xbox_Analog() {

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp))
				MoveForward();

			else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))
				MoveBackward();

			else
				Stop();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft) && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X) > .1)
				TurnLeft();


			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickRight) && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X) > .1)
				TurnRight();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightTrigger))
				Fire();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightThumbstickLeft))
				TurretTurnLeft();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightThumbstickRight))
				TurretTurnRight();


		}

		public void Xbox_XA() {

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
				MoveForward();

			else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X))
				MoveBackward();

			else
				Stop();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickLeft))
				TurnLeft();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickRight))
				TurnRight();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightTrigger))
				Fire();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftShoulder))
				TurretTurnLeft();

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightShoulder))
				TurretTurnRight();


		}

		public void PC_Full() {

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
                MoveForward();
                Updated = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Down)) {
                MoveBackward();
                Updated = true;
            }

            else
                Stop();

            if (Keyboard.GetState().IsKeyDown(Keys.Left)) {
                TurnLeft();
                Updated = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
                TurnRight();
                Updated = true;
            }

			if (Keyboard.GetState().IsKeyDown(Keys.Space))
				Fire();

			if (Keyboard.GetState().IsKeyDown(Keys.A))
				TurretTurnLeft();

			if (Keyboard.GetState().IsKeyDown(Keys.D))
				TurretTurnRight();

			if (Keyboard.GetState().IsKeyDown(Keys.D1))
				Weapon(Projectile.Type.REGULAR);

			if (Keyboard.GetState().IsKeyDown(Keys.D2))
				Weapon(Projectile.Type.GRENADE);

		}

        public override void Draw(SpriteBatch spriteBatch) {

            //Draw Health bar: Röd
            spriteBatch.Draw(Settings.TextureRed, Position + healthOffSet, healthRect, Color.Red);
            //Draw Health bar: Grön
            spriteBatch.Draw(Settings.TextureGreen, Position + healthOffSet, new Rectangle(0, 0, (int)(CurrentHealth / maxHealth * healthRect.Width), healthRect.Height), Color.White);

            base.Draw(spriteBatch);

        }

		public void SetHealth(float health) {

			CurrentHealth = health;
			if (CurrentHealth > maxHealth)
				CurrentHealth = maxHealth;

		}

		public void Hit(float Damage) {

			CurrentHealth -= Damage;

			if (CurrentHealth < 0)
				CurrentHealth = 0;

		}

        public void parseData(Newtonsoft.Json.Linq.JObject obj) {
            Newtonsoft.Json.JsonConvert.PopulateObject(obj.ToString(), this);


            String[] array = new String[(int)Fields.COUNT];
            int i = 0;
            foreach (Fields val in Enum.GetValues(typeof(Fields))) {
                array[i] = val.ToString();
                i++;
            }
        }

        public void populateData(Newtonsoft.Json.Linq.JObject obj) {
            Newtonsoft.Json.JsonConvert.PopulateObject(obj.ToString(), this);
        }

        public Newtonsoft.Json.Linq.JObject getData() {
            Newtonsoft.Json.Linq.JObject obj = new Newtonsoft.Json.Linq.JObject();
            obj[Fields.Position] = Newtonsoft.Json.Linq.JToken.FromObject(Position);
            obj[Fields.Direction] = Newtonsoft.Json.Linq.JToken.FromObject(Direction);
            return obj;
        }

        enum Fields
        {
            Position,
            Direction,
            COUNT
        }

	}
}
