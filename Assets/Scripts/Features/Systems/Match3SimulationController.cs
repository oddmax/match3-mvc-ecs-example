using System;
using System.Collections;
using System.Collections.Generic;
using Core.Utils;
using Features.Data;
using Features.Models;
using Unity.Entities;
using Zenject;

namespace Features.Systems
{
    /// <summary>
    /// Unfortunately currently DOTS doesn't support injection into systems so this is workaround to have injections inside of the systems
    /// </summary>
    public class Match3SimulationController : IInitializable, IDisposable, ILateTickable
    {
        private SignalBus signalBus;
        private EntityManager entityManager;
        private IEnumerable<ComponentSystemBase> systems;
        private BoardModel boardModel;
        private GameStateModel gameStateModel;
        private CoroutineProvider coroutineProvider;

        public Match3SimulationController(SignalBus signalBus, EntityManager entityManager, BoardModel boardModel, GameStateModel gameStateModel, CoroutineProvider coroutineProvider)
        {
            this.gameStateModel = gameStateModel;
            this.boardModel = boardModel;
            this.entityManager = entityManager;
            this.signalBus = signalBus;
            this.coroutineProvider = coroutineProvider;
            
            EntitiesHelper.Initialize(entityManager);
        }
        
        public void Initialize()
        {
            var swapSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SwapGemsSystem>();
            swapSystem.Init(signalBus, boardModel, gameStateModel);
            
            var fillSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<FillSystem>();
            fillSystem.Init(signalBus, boardModel, gameStateModel);
            
            var fallSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<FallSystem>();
            fallSystem.Init(signalBus, boardModel, gameStateModel);
            
            var destroySystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DestroySystem>();
            destroySystem.Init(signalBus,boardModel, gameStateModel);
            
            var simulationGroup = World.DefaultGameObjectInjectionWorld.GetExistingSystem<Match3SimulationSystemGroup>();
            this.systems = simulationGroup.Systems;
        }
        
        public void Dispose()
        {
            coroutineProvider.StartCoroutine(DisposeWorld());
        }

        private static IEnumerator DisposeWorld()
        {
            World.DefaultGameObjectInjectionWorld.QuitUpdate = true;
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(null);

            yield return null;

            World.DisposeAllWorlds();
        }

        public void LateTick()
        {
        }
    }
}