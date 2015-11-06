using Microsoft.Xna.Framework;
using System.Collections.Generic;
using com.nuggeta.game.core.ngdl.nobjects;


namespace UI
{
    class JoinGame : Screen
    {

        private WidgetMenuScroll Menu;
        private List<NGame> games;

        public JoinGame(List<NGame> games)
            : base("Join Game") {

            this.games = games;

            //black background
            WidgetGraphic black = new WidgetGraphic();
            black.Size = new Vector3(_UI.SX, _UI.SY, 0.0f);
            black.AddTexture("null", 0.0f, 0.0f, 1.0f, 1.0f);
            black.ColorBase = Color.Black;
            Add(black);

            WidgetMenuScroll menu = new WidgetMenuScroll(E_MenuType.Vertical);
            menu.Position = new Vector3(_UI.SXM, _UI.SYM + 25.0f, 0.0f);
            menu.Padding = 75.0f;
            menu.Alpha = 1.0f;
            Add(menu);

            Menu = menu;


            for (int i = 0; i < games.Count; ++i) {
                WidgetMenuNode node = new WidgetMenuNode(i);
                node.Parent(menu);
                Add(node);

                Timeline nodeT = new Timeline("selected", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.Start);
                nodeT.AddEffect(new TimelineEffect_ScaleX(0.0f, 0.125f, E_LerpType.SmoothStep));
                nodeT.AddEffect(new TimelineEffect_ScaleY(0.0f, 0.125f, E_LerpType.SmoothStep));

                Timeline nodeT2 = new Timeline("selected", false, 0.0f, 0.5f, E_TimerType.Stop, E_RestType.Start);
                nodeT2.AddEffect(new TimelineEffect_Intensity(0.0f, .75f, E_LerpType.SmoothStep));

                node.AddTimeline(nodeT);
                node.AddTimeline(nodeT2);

                WidgetText text = new WidgetText();
                text.Size = new Vector3(0.0f, 50.0f, 0.0f);
                text.Align = E_Align.MiddleCentre;
                text.FontStyleName = "Default";
                text.String = games[i].getName();
                text.Parent(node);
                text.ParentAttach = E_Align.MiddleCentre;
                text.ColorBase = Color.Orange;
                Add(text);

            }

        }

        // OnProcessInput
        protected override void OnProcessInput(Input input) {
            if (input.ButtonJustPressed((int)E_UiButton.A)) {
                int index = Menu.GetByValue();

                Controller c = MyController.getInstance();
                c.JoinGame(games[index].getId());
                _UI.Screen.SetNextScreen(null);

            }
            else
                if (input.ButtonJustPressed((int)E_UiButton.B)) {
                    SetScreenTimers(0.0f, 0.5f);

                    _G.UI.SS_FromMainMenu = true;

                    _UI.Screen.SetNextScreen(new Screen_Start());
                }
        }

    }
}
