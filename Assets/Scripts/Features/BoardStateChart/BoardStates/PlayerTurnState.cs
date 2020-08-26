using Features.Models;
using Features.Signals;
using Zenject;

namespace Features.BoardStateChart.BoardStates
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