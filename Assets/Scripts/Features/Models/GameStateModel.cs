using Data;
using Features.Config;
using JetBrains.Annotations;
using Signals;
using UnityEngine;
using Zenject;

namespace Models
{
    /// <summary>
    /// Main game model, holds current board game state and current level 
    /// </summary>
    [UsedImplicitly]
    public class GameStateModel
    {
        [Inject]
        private GameConfig gameConfig;
		
        [Inject]
        private SignalBus signalBus;
        
        private int currentLevelIndex;
        private Match3State state;

        public Match3State State
        {
            get => state;
            set
            {
                state = value;
                Debug.Log("Set state: " + state );
            }
        }

        public int CurrentLevelIndex
        {
            get => currentLevelIndex;
            set
            {
                if(value < 0 || value >= gameConfig.levels.Length)
                    return;
				
                currentLevelIndex = value;
                signalBus.Fire(new LevelChangedSignal(value));
            }
        }

        public int LevelsAmount => gameConfig.levels.Length;

        public LevelConfig GetCurrentLevelConfig()
        {
            return gameConfig.levels[CurrentLevelIndex];
        }
    }
}