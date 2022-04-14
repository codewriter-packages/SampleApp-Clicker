using Code.ECS.Events;
using Morpeh;
using Morpeh.Globals;
using UnityEngine;

namespace Code.ECS.Systems
{
    [CreateAssetMenu(menuName = "SampleApp-Clicker/Systems/Game System")]
    public class GameSystem : UpdateSystem
    {
        [Header("Variables")]
        [SerializeField] private GlobalVariableBool gameRunning = default;
        [SerializeField] private GlobalVariableFloat remainingTime = default;

        [Header("Events")]
        [SerializeField] private StartGameRequestedEvent startGameRequestedEvent = default;

        [Header("Constants")]
        [SerializeField] private float gameDuration = 5;

        public override void OnAwake()
        {
            remainingTime.Value = 0f;
            gameRunning.Value = false;
        }

        public override void OnUpdate(float deltaTime)
        {
            ProcessGameStartRequest();
            ProcessGameTime(deltaTime);
        }

        private void ProcessGameStartRequest()
        {
            if (!startGameRequestedEvent.IsPublished)
            {
                return;
            }

            gameRunning.Value = true;
            remainingTime.Value = gameDuration;
        }

        private void ProcessGameTime(float deltaTime)
        {
            if (!gameRunning.Value)
            {
                return;
            }

            remainingTime.Value -= deltaTime;

            if (remainingTime.Value > 0f)
            {
                return;
            }

            remainingTime.Value = 0f;
            gameRunning.Value = false;
        }
    }
}