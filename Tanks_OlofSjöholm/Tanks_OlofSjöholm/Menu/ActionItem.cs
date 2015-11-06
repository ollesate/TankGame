using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tanks_OlofSjöholm {
	class ActionItem : MenuItem {


		public Game1.State Action;

		public ActionItem(String name, Game1.State action)
			: base(name) {

			this.Name = name;
			this.Action = action;
		}

	}
}
