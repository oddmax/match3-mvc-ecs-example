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
            var world = World.DefaultGameObjectInjectionWorld;

            // ── create / fetch the systems ────────────────────────────────
            var swapSystem    = world.GetOrCreateSystemManaged<SwapGemsSystem>();
            var fillSystem    = world.GetOrCreateSystemManaged<FillSystem>();
            var fallSystem    = world.GetOrCreateSystemManaged<FallSystem>();
            var destroySystem = world.GetOrCreateSystemManaged<DestroySystem>();

            // ── pass your external refs exactly as before ─────────────────
            swapSystem.Init   (signalBus, boardModel, gameStateModel);
            fillSystem.Init   (signalBus, boardModel, gameStateModel);
            fallSystem.Init   (signalBus, boardModel, gameStateModel);
            destroySystem.Init(signalBus, boardModel, gameStateModel);

            // ── grab the custom simulation group and its children ─────────
            var simulationGroup = world.GetExistingSystemManaged<Match3SimulationSystemGroup>();
            this.systems        = simulationGroup.ManagedSystems;   // was .Systems
        }
        
        public void Dispose()
        {
            coroutineProvider.StartCoroutine(DisposeWorld());
        }

        private static IEnumerator DisposeWorld()
        {
            World.DefaultGameObjectInjectionWorld.QuitUpdate = true;
            //ScriptBehaviourUpdateOrder.UpdatePlayerLoop(null);

            yield return null;

            World.DisposeAllWorlds();
        }

        public void LateTick()
        {
        }
    }
}