using Features.Data;
using Features.Data.Components;
using Features.Models;
using Features.Signals;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Zenject;

namespace Features.Systems
{
    /// <summary>
    /// System responsible for creating new gems instead of destroyed ones
    /// </summary>
    [UpdateInGroup(typeof(Match3SimulationSystemGroup))]
    [UpdateBefore(typeof(FallSystem))]
    public  partial  class FillSystem : SystemBase
    {
        private SignalBus signalBus;
        private BoardModel boardModel;
        private GameStateModel gameStateModel;

        public void Init(SignalBus signalBus, BoardModel boardModel, GameStateModel gameStateModel)
        {
            this.gameStateModel = gameStateModel;
            this.signalBus = signalBus;
            this.boardModel = boardModel;
        }
        
        protected override void OnUpdate()
        {
            if(boardModel == null || gameStateModel.State != Match3State.MatchesFall) 
                return;
            
            for (int x = 0; x < boardModel.BoardWidth; x++)
            {
                if(boardModel.HasEntityAt(new int2(x, boardModel.BoardHeight-1)) == false)
                {
                    var levelConfig = gameStateModel.GetCurrentLevelConfig();
                    var boardPosition = new int2(x, boardModel.BoardHeight);
                    var spawnPosition = BoardCalculator.ConvertBoardPositionToTransformPosition(boardPosition);
                    if(HasGemInProximityInColumn(x, spawnPosition))
                        continue;
                    
                    var entity = EntitiesHelper.CreateGem(EntitiesHelper.GetRandomColor(levelConfig.availableColors),
                        BoardCalculator.ConvertBoardPositionToTransformPosition(boardPosition), boardPosition);
                    
                    signalBus.Fire(new Match3Signals.GemCreatedSignal(entity, BoardCalculator.ConvertBoardPositionToTransformPosition(boardPosition)));
                    EntityManager.AddComponent<IsFallingComponent>(entity);
                }
            }
        }

        private bool HasGemInProximityInColumn(int x, float3 position)
        {
            for (int y = 0; y < boardModel.BoardHeight; y++)
            {
                if (boardModel.HasEntityAt(new int2(x, y)))
                {
                    var entity = boardModel.GetEntityAt(x, y);
                    var lt = EntityManager.GetComponentData<LocalTransform>(entity);
                    if (math.distancesq(position, lt.Position) < BoardCalculator.GEM_SIZE)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}