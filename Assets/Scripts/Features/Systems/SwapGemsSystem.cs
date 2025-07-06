using Features.BoardStateChart;
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
    /// System responsible for swapping to gems which have SwapComponents
    /// </summary>
    [UpdateInGroup(typeof(Match3SimulationSystemGroup))]
    public  partial  class SwapGemsSystem : SystemBase
    {
        private SignalBus signalBus;
        private BoardModel boardModel;
        private GameStateModel gameStateModel;
        private EntityQuery query;

        public void Init(SignalBus signalBus, BoardModel boardModel, GameStateModel gameStateModel)
        {
            this.gameStateModel = gameStateModel;
            this.signalBus = signalBus;
            this.boardModel = boardModel;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            query = GetEntityQuery(typeof(SwappingComponent));
            RequireForUpdate(query);
        }

        protected override void OnStopRunning()
        {
            signalBus.Fire(new Match3Signals.StateChartSignal(BoardStateEvents.SwapCompleteEvent));
        }

        protected override void OnUpdate()
        {
            if(gameStateModel == null) 
                return;
            
            var time = World.Time.DeltaTime;
            Entities.WithStructuralChanges().ForEach(
                (Entity entity, 
                int entityInQueryIndex,
                ref LocalTransform transform,
                ref BoardPositionComponent boardPositionComponent,
                in SwappingComponent swapComponent) =>
                {
                    var startPosition = BoardCalculator.ConvertBoardPositionToTransformPosition(swapComponent.OriginBoardPosition);
                    var targetPosition = BoardCalculator.ConvertBoardPositionToTransformPosition(swapComponent.TargetBoardPosition);
                    
                    var direction = targetPosition - transform.Position;
                  
                    if (math.distancesq(transform.Position, targetPosition) < 0.01f)
                    {
                        boardPositionComponent = new BoardPositionComponent { Position = swapComponent.TargetBoardPosition};
                        boardModel.SetEntityAt(swapComponent.TargetBoardPosition.x, swapComponent.TargetBoardPosition.y, entity);
                        transform.Position = targetPosition;  
                        EntityManager.RemoveComponent<SwappingComponent>(entity);
                    }
                    else
                    {
                        var velocity = math.normalize(direction) * 7 * time;
                        transform.Position += velocity;
                    }
                }).Run();
        }
        
    }
}