using CodeWriter.ViewBinding;
using UniMob.UI;
using UnityEngine;

namespace Code.UI.Views
{
    public class MainMenuView : View<IMainMenuState>
    {
        [SerializeField] private ViewContext viewContext = default;

        [Header("Variables")]
        [SerializeField] private ViewVariableString primaryStyle = default;

        [Header("Events")]
        [SerializeField] private ViewEventVoid onPlayClick = default;

        protected override void Awake()
        {
            base.Awake();

            primaryStyle.Bind(this, () => State.PrimaryStyle);
            onPlayClick.Bind(this, () => State.Play());
        }

        protected override void Render()
        {
            viewContext.Render();
        }
    }

    public interface IMainMenuState : IViewState
    {
        string PrimaryStyle { get; }

        void Play();
    }
}