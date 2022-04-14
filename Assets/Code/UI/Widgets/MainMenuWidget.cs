using Code.UI.Views;
using Morpeh.Globals;
using UniMob;
using UniMob.UI;
using UniMob.UI.Widgets;

namespace Code.UI.Widgets
{
    public class MainMenuWidget : StatefulWidget
    {
        public override State CreateState() => StateProvider.Of(this);
    }

    public class MainMenuState : ViewState<MainMenuWidget>, IMainMenuState
    {
        private readonly GlobalVariableString _primaryStyle;

        public MainMenuState(WidgetViewReference view,
            GlobalVariableString primaryStyle)
        {
            _primaryStyle = primaryStyle;

            View = view;
        }

        public override WidgetViewReference View { get; }

        [Atom] public string PrimaryStyle => _primaryStyle.Value;

        public void Play()
        {
            Navigator.Of(Context).Push(new PageRouteBuilder(
                settings: new RouteSettings("game", RouteModalType.Fullscreen),
                pageBuilder: (context, animation, secondaryAnimation) => new GameMenuWidget
                {
                    OnGameEnded = () => Navigator.Of(context).Pop(),
                }
            ));
        }
    }
}