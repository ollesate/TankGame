using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tanks_OlofSjöholm
{
    class MenuItem 
    {


        public String Name;
        public MenuItem(String name) {
            this.Name = name;
        }

        //Name, Color, Type: (Titel, ActionButton, SettingsButton), Size
        //AddActionItem: State
        //AddSettingItem: Referens
        //AddTitel: ~

        //Gemensamma: Name, Color, Size
    }
}
