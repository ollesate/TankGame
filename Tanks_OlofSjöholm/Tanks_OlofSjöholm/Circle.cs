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

	class Circle {


		public float Radius;
		public Vector2 CenterPosition;

        public Circle(float CenterX, float CenterY, float radius) {

            CenterPosition = new Vector2(CenterX, CenterY);
            this.Radius = radius;

		}

		public bool Intersects(Rectangle rect) {

			Vector2 closestRectPos = CenterPosition;

			if (CenterPosition.X < rect.X)
                closestRectPos.X = rect.X;
			else if (CenterPosition.X > rect.X + rect.Width)
                closestRectPos.X = rect.X + rect.Width;
			if (CenterPosition.Y < rect.Y)
                closestRectPos.Y = rect.Y;
			else if (CenterPosition.Y > rect.Y + rect.Height)
                closestRectPos.Y = rect.Y + rect.Height;
            
      

			return Vector2.Distance(CenterPosition, closestRectPos) < Radius;
		}

		public bool Intersects(Circle circl) {

			return Vector2.Distance(CenterPosition, circl.CenterPosition) < (Radius + circl.Radius);

		}
	}
