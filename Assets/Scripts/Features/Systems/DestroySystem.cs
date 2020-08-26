using Features.Data.Components;
using Features.Models;
using Features.Signals;
using Unity.Entities;
using Zenject;

namespace Features.Systems
{
    /// <summary>
    /// System responsible for destroying managed gems
    /// </summary>
    [UpdateInGroup(typeof(Match3SimulationSystemGroup))]
    [UpdateBefore(typeof(SwapGemsSystem))]
    public class DestroySystem : SystemBase
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
            Entities.WithAll<DestroyedComponent>().WithStructuralChanges().ForEach((Entity entity, int entityInQueryIndex) =>
            {
                signalBus.Fire(new Match3Signals.GemDestroyedSignal(entity));
                EntityManager.DestroyEntity(entity);
            }).Run() ;
        }
    }
}