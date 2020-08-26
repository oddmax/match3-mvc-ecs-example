using Data;
using Models;
using StateChart;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.States.BoardStates
{
    public class BoardBaseState : IState
    {
        [Inject] 
        protected GameStateModel gameStateModel;
        
        [Inject] 
        protected SignalBus signalBus;
        
        private Match3State state;

        public void SetState(Match3State stateEnum)
        {
            state = stateEnum;
        }
        
        public virtual void OnEnter()
        {
            gameStateModel.State = state;
            Debug.Log("Enter state " + state);
        }

        public virtual void OnExit()
        {
            Debug.Log("Exit state " + state);
        }
    }
}