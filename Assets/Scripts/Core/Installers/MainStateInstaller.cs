using Core.GameStates;
using Core.Utils;
using Features.Models;
using Features.Signals;
using Zenject;

namespace Core.Installers
{
	/// <summary>
	/// Main installer with global game dependencies
	/// </summary>
	public class MainStateInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);
		
			//Signals
			Container.DeclareSignal<LevelChangedSignal>();
			Container.DeclareSignal<ChangeLevelSignal>();
			Container.DeclareSignal<ExitToMapSignal>();
			Container.DeclareSignal<Match3Signals.PlayerScoreChangedSignal>();
			Container.DeclareSignal<Match3Signals.TurnAmountChangedSignal>();
			Container.DeclareSignal<Match3Signals.CreateBoardSignal>();
			Container.DeclareSignal<Match3Signals.OutOfTurnsSignal>();

			//Models
			Container.BindInterfacesAndSelfTo<PlayerScoreModel>().AsSingle();
			Container.BindInterfacesAndSelfTo<GameStateModel>().AsSingle();

			//Utilities and others
			Container.BindInterfacesAndSelfTo<GameStateChart>().AsSingle();
			Container.Bind<CoroutineProvider>().FromNewComponentOnNewGameObject().AsSingle();
		}
	}
}