using Models;
using Signals;
using Zenject;

namespace DefaultNamespace.States.BoardStates
{
    public class PlayerTurnState : BoardBaseState
    {
        [Inject] 
        private PlayerScoreModel playerScoreModel;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            if (playerScoreModel.Turns <= 0)
                signalBus.Fire(new Match3Signals.StateChartSignal(BoardStateEvents.NoTurnsLeftEvent));
        }
    }
}