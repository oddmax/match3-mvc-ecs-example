using Core.StateMachine;
using Features.Data;
using Features.Models;
using UnityEngine;
using Zenject;

namespace Features.BoardStateChart.BoardStates
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