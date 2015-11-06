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
    class Calc
    {
        public static int Mod(int a, int b) {
            return (Math.Abs(a * b) + a) % b;
        }

        public static Vector2 RotateVector(Vector2 vector, double angle) {

            return new Vector2((float)Math.Cos(Math.Atan2(vector.Y, vector.X) + angle), (float)Math.Sin(Math.Atan2(vector.Y, vector.X) + angle)); ;
        }

        public static double AngleBetween(Vector2 vector1, Vector2 vector2) {

            double a1 = Math.Atan2(vector2.Y, vector2.X);
            double a2 = Math.Atan2(vector1.Y, vector1.X);
			
            return a1 - a2;
        }

        public static double AngleOf(Vector2 vector) {

            return Math.Atan2(vector.Y, vector.X);

        }
    }
}
