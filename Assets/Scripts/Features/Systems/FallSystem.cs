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
    /// System responsible for moving falling gems and updating their BoardPositionComponent to new ones
    /// </summary>
    [UpdateInGroup(typeof(Match3SimulationSystemGroup))]
    public  partial  class FallSystem : SystemBase
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
        
        protected override void OnCreate()
        {
            // Run while there is *either* a falling piece or one that is still animating
            RequireAnyForUpdate(
                GetEntityQuery(ComponentType.ReadOnly<IsFallingComponent>()),
                GetEntityQuery(ComponentType.ReadOnly<MoveToBoardPositionComponent>())
            );
        }
        
        protected override void OnStopRunning()
        {
            signalBus.Fire(new Match3Signals.StateChartSignal(BoardStateEvents.FallingCompleteEvent));
        }
        
        protected override void OnUpdate()
        {
            if(gameStateModel == null || gameStateModel.State != Match3State.MatchesFall) 
                return;

            var time = World.Time.DeltaTime;
            Entities.WithAll<IsFallingComponent>().WithNone<DestroyedComponent>().WithStructuralChanges().ForEach(
                (Entity entity, int entityInQueryIndex, ref BoardPositionComponent boardPositionComponent) =>
                {
                    var positionUnder = new int2(boardPositionComponent.Position.x, boardPositionComponent.Position.y - 1);
                    if (boardModel.IsPositionInBorders(positionUnder) && !boardModel.HasEntityAt(positionUnder))
                    {
                        boardModel.ClearAt(boardPositionComponent.Position);
                        
                        if(EntityManager.HasComponent<MoveToBoardPositionComponent>(entity) == false) 
                            EntityManager.AddComponentData(entity, new MoveToBoardPositionComponent {TargetPosition = positionUnder});
                        else
                            EntityManager.SetComponentData(entity, new MoveToBoardPositionComponent {TargetPosition = positionUnder});
                        
                        boardPositionComponent = new BoardPositionComponent { Position = positionUnder};
                        boardModel.SetEntityAt(positionUnder.x, positionUnder.y, entity);
                    }
                    else
                    {
                        if (boardModel.IsPositionInBorders(positionUnder) == false)
                            EntityManager.RemoveComponent<IsFallingComponent>(entity);
                        else
                        {
                            var entityUnder = boardModel.GetEntityAt(positionUnder.x, positionUnder.y);
                           if (EntityManager.HasComponent<IsFallingComponent>(entityUnder) == false)
                                EntityManager.RemoveComponent<IsFallingComponent>(entity);
                        }
                    }
                    
                }).Run();

            
            Entities.WithNone<DestroyedComponent>().WithStructuralChanges().ForEach(
                (Entity entity,
                    int entityInQueryIndex,
                    ref LocalTransform transform,
                    in BoardPositionComponent boardPositionComponent,
                    in MoveToBoardPositionComponent moveComponent) =>
                {
                    var targetPosition = BoardCalculator.ConvertBoardPositionToTransformPosition(boardPositionComponent.Position);
                    
                    var direction = targetPosition - transform.Position;
                    if (math.distancesq(transform.Position, targetPosition) < 0.01f || transform.Position.y < targetPosition.y)
                    {
                        transform.Position = targetPosition;     
                        EntityManager.RemoveComponent<MoveToBoardPositionComponent>(entity);
                    }
                    else
                    {
                        var velocity = math.normalize(direction) * 10 * time;
                        transform.Position += velocity; 
                    }
                }).Run();
        }
    }
}