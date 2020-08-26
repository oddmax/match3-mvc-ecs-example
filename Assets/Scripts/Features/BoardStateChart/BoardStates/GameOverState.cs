using Signals;
using StateChart;

namespace DefaultNamespace.States.BoardStates
{
    public class GameOverState : BoardBaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            signalBus.Fire<Match3Signals.OutOfTurnsSignal>();
        }

        public void OnExit()
        {
        }
    }
}