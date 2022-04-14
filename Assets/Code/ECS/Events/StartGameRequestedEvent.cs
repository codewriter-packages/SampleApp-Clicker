using System;
using Morpeh.Globals;
using UnityEngine;

namespace Code.ECS.Events
{
    [CreateAssetMenu(menuName = "SampleApp-Clicker/Events/StartGameRequested Event")]
    public class StartGameRequestedEvent : BaseGlobalEvent<StartGameRequestedEvent.Args>
    {
        [Serializable]
        public struct Args { }
    }
}