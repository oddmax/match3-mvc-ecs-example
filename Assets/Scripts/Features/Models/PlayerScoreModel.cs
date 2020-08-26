using Features.Signals;
using JetBrains.Annotations;
using Zenject;

namespace Features.Models
{
    /// <summary>
    /// Model for the player score and amount of remaining turns
    /// </summary>
    [UsedImplicitly]
    public class PlayerScoreModel : IInitializable
    {
        [Inject] 
        private SignalBus signalBus;
        
        public int Score
        {
            get => score;
            set
            {
                if (score != value)
                {
                    score = value;
                    signalBus.Fire<Match3Signals.PlayerScoreChangedSignal>();
                }
            }
        }
        
        public int Turns
        {
            get => turns;
            set
            {
                if (turns != value)
                {
                    turns = value;
                    signalBus.Fire<Match3Signals.TurnAmountChangedSignal>();
                }
            }
        }

        private int score;
        private int turns;

        public void Initialize()
        {
            Reset();
        }

        public void Reset()
        {
            Score = 0;
        }
    }
}