using StateChart;
using UnityEngine;
using Utils;
using Zenject;

namespace DefaultNamespace.States
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