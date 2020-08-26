using Features.Signals;

namespace Features.BoardStateChart.BoardStates
{
    public class GameOverState : BoardBaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            signalBus.Fire<Match3Signals.OutOfTurnsSignal>();
        }
    }
}