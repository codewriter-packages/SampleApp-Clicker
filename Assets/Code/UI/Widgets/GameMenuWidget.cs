using System;
using Code.ECS.Events;
using Code.UI.Views;
using Morpeh.Globals;
using UniMob;
using UniMob.UI;

namespace Code.UI.Widgets
{
    public class GameMenuWidget : StatefulWidget
    {
        public Action OnGameEnded { get; set; }

        public override State CreateState() => StateProvider.Of(this);
    }

    public class GameMenuState : ViewState<GameMenuWidget>, IGameMenuState
    {
        private readonly GlobalVariableString _primaryStyle;
        private readonly GlobalVariableBool _gameRunning;
        private readonly GlobalVariableFloat _timeRemaining;
        private readonly StartGameRequestedEvent _startGameRequestedEvent;

        public GameMenuState(WidgetViewReference view,
            GlobalVariableString primaryStyle,
            GlobalVariableBool gameRunning,
            GlobalVariableFloat timeRemaining,
            StartGameRequestedEvent startGameRequestedEvent)
        {
            _primaryStyle = primaryStyle;
            _gameRunning = gameRunning;
            _timeRemaining = timeRemaining;
            _startGameRequestedEvent = startGameRequestedEvent;
            View = view;
        }

        public override WidgetViewReference View { get; }

        public override async void InitState()
        {
            base.InitState();

            _startGameRequestedEvent.NextFrame(new StartGameRequestedEvent.Args());

            await Atom.When(StateLifetime, () => _gameRunning.Value);
            await Atom.When(StateLifetime, () => _gameRunning.Value == false);

            Widget.OnGameEnded?.Invoke();
        }

        [Atom] public string PrimaryStyle => _primaryStyle.Value;

        [Atom] public float GameTimeRemaining => _timeRemaining.Value;
    }
}