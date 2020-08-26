using Core.StateChartMachine;
using Core.Utils;
using UnityEngine;
using Zenject;

namespace Core.GameStates.States
{
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