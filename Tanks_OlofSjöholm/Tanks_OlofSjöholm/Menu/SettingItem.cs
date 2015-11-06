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
	class SettingItem : MenuItem{

		public bool Marked;
        ChangeableFloat Value;
        String nameTemp;

        public SettingItem(String name, ChangeableFloat Value)
			: base(name) {

			this.Value = Value;
            nameTemp = name;
			this.Name = name + ": " + Value;

		}

		public void Update() {

			if (Marked) {
				if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    Value.Value++;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    Value.Value--;

				if (Keyboard.GetState().IsKeyDown(Keys.Enter))
					Marked = false;
			}

            this.Name = nameTemp + ": " + Value;
		}

	}

	//Lägg in värde ha en referens
	//Vänta på enter klick Varav andra sidan kan redigeras




	abstract class AbsChangeableValue
	{
		//public static List<AbsChangeableValue> allvalues = new List<AbsChangeableValue>();



		abstract public void TextInput(string text);
	}

	class ChangeableFloat : AbsChangeableValue
	{
		public float Value;
		public ChangeableFloat(float startValue) {
			Value = startValue;
		}

		public override void  TextInput(string text)
		{
 			Value = Convert.ToSingle(text);
		}

        public override String ToString() {

            return Value.ToString();
        }
	}

	class ChangeableColor : AbsChangeableValue 
	{
		Color color;

		public override void TextInput(string text) {
			switch (text.ToLower()) 
			{ 
				case "red":
					color = Color.Red;
					break;
			}
		}
	}

	class TestTank
	{
	}
}
