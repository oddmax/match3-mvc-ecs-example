using Core.StateMachine;
using Core.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Core.GameStates.States
{
    /// <summary>
    /// Boot state where all initialization and loading should happen
    /// </summary>
    [UsedImplicitly]
    public class BootState : IState
    {
        [Inject]
        private CoroutineProvider coroutineProvider;
        
        public void OnEnter()
        {
            Debug.Log("Booting the game!");
        }

        public void OnExit()
        {
            Debug.Log("Booting is complete!");
        }
    }
}