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

namespace Tanks_OlofSjöholm
{
    class StateManager
    {


        List<Game1.State> InWhatState = new List<Game1.State>();
        List<Keys> WhenPressed = new List<Keys>();
        List<Game1.State> DoThis = new List<Game1.State>();
        //In state Title -> Escape = exit

        public Game1.State GetState(Game1.State GameState) {

            int numberOfActions = InWhatState.Count;

            for (int i = 0; i < numberOfActions; i++) {
                Keys currentKey = WhenPressed.ElementAt(i);
                if (KeyMouseReader.KeyClick(currentKey) && InWhatState.ElementAt(i) == GameState) {
                    return DoThis.ElementAt(i);
                    }
            }

            return GameState;

        }

        public void InitializeAction(Game1.State state, Keys key, Game1.State action) {
            InWhatState.Add(state);
            WhenPressed.Add(key);
            DoThis.Add(action);
        }

    }
}
