using Microsoft.Xna.Framework;

using Nugetta;

using System.Collections.Generic;

using com.nuggeta;
using com.nuggeta.api;
using com.nuggeta.game.core.ngdl.nobjects;
using com.nuggeta.network.plug;
using com.nuggeta.game.core.api.handlers;

namespace UI
{
    class StartMenu : Screen
    {
        
        private static string[] Options = { "JOIN GAME", "CREATE GAME", "OPTIONS", "QUIT" };
        private static string[] TextureNames = { "null", "null", "null", "null" };
        private WidgetMenuScroll Menu;


        public StartMenu() : base ("test") {

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

            for (int i = 0; i < Options.Length; ++i) {
                WidgetMenuNode node = new WidgetMenuNode(i);
                node.Parent(Menu);
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
                text.String = Options[i];
                text.Parent(node);
                text.ParentAttach = E_Align.MiddleCentre;
                text.ColorBase = Color.Orange;
                Add(text);

            }
        }

        // OnProcessMessage
        protected override void OnProcessMessage(ref ScreenMessage message) {
            E_UiMessageType type = (E_UiMessageType)message.Type;

            if (type == E_UiMessageType.PopupConfirm) {
                switch ((E_PopupType)message.Data) {
                    case E_PopupType.NewGame:  break;
                    case E_PopupType.Quit: _UI.Game.Exit(); break;
                }
            }
        }

        // OnProcessInput
        protected override void OnProcessInput(Input input) {
            if (input.ButtonJustPressed((int)E_UiButton.A)) {
                if (Menu.GetByValue() == 0) {
                    NugettaHandler connection = MyNugettaHandler.getInstance();
                    connection.FindGames((GetGamesResponse response) => {
                        List<NGame> list = response.getGames();
                        _UI.Screen.AddScreen(new JoinGame(list));
                    });
                    //_UI.Screen.AddScreen(new Screen_Popup(E_PopupType.NewGame));
                }
                else
                    if (Menu.GetByValue() == 1) {
                        _UI.Screen.SetNextScreen(null);
                    }
                    else
                        if (Menu.GetByValue() == 2) {
                            _UI.Screen.SetNextScreen(new Screen_Options());
                        }
                        else
                            if (Menu.GetByValue() == 3) {
                                _UI.Screen.AddScreen(new Screen_Popup(E_PopupType.Quit));
                            }
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
