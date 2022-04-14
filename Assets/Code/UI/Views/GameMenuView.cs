using CodeWriter.ViewBinding;
using UniMob.UI;
using UnityEngine;

namespace Code.UI.Views
{
    public class GameMenuView : View<IGameMenuState>
    {
        [SerializeField] private ViewContext viewContext = default;

        [Header("Variables")]
        [SerializeField]
        private ViewVariableFloat timeRemaining = default;

        [SerializeField]
        private ViewVariableString primaryStyle = default;

        protected override void Awake()
        {
            base.Awake();

            primaryStyle.Bind(this, () => State.PrimaryStyle);
            timeRemaining.Bind(this, () => State.GameTimeRemaining);
        }

        protected override void Render()
        {
            viewContext.Render();
        }
    }

    public interface IGameMenuState : IViewState
    {
        string PrimaryStyle { get; }

        float GameTimeRemaining { get; }
    }
}