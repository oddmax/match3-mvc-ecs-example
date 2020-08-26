using Features.Commands;
using Features.Models;
using Features.Signals;
using Features.Systems;
using Unity.Entities;
using Zenject;

namespace Features.Installers
{
    /// <summary>
    /// Dependencies specific for board/match3 state
    /// </summary>
    public class BoardStateInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            //Signals
            Container.DeclareSignal<Match3Signals.OnBoardCreatedSignal>();
            Container.DeclareSignal<Match3Signals.StartSwapSignal>();
            Container.DeclareSignal<Match3Signals.FindMatchesSignal>();
            Container.DeclareSignal<Match3Signals.StartFallSignal>();
            Container.DeclareSignal<Match3Signals.GemCreatedSignal>();
            Container.DeclareSignal<Match3Signals.GemDestroyedSignal>();
            Container.DeclareSignal<Match3Signals.StateChartSignal>();
            
            //High-level controllers
            Container.BindInterfacesAndSelfTo<CreateBoardCommand>().AsSingle();
            Container.BindSignal<Match3Signals.CreateBoardSignal>()
                .ToMethod<CreateBoardCommand>(x => x.Execute).FromResolve();
            
            Container.BindInterfacesAndSelfTo<StartSwapCommand>().AsSingle();
            Container.BindSignal<Match3Signals.StartSwapSignal>()
                .ToMethod<StartSwapCommand>(x => x.Execute).FromResolve();
            
            Container.BindInterfacesAndSelfTo<BoardModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoardStateChart.BoardStateChart>().AsSingle();
            
            DefaultWorldInitialization.Initialize("Match3World", false);
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Container.Bind<EntityManager>().FromInstance(entityManager).AsSingle();
            Container.BindInterfacesAndSelfTo<Match3SimulationController>().AsSingle().NonLazy();
        }
    }
}