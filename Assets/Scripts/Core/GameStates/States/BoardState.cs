using System.Collections;
using Core.Config;
using Core.StateMachine;
using Core.Utils;
using Features.Signals;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.GameStates.States
{
    /// <summary>
    /// Main gameplay state with a game board and moving pieces
    /// </summary>
    [UsedImplicitly]
    public class BoardState : IState
    {
        [Inject]
        private CoroutineProvider coroutineProvider;
        
        [Inject]
        private GameSceneCatalogue gameSceneCatalogue;

        [Inject] 
        private SignalBus signalBus;
        
        public static AsyncOperationHandle<SceneInstance> StateSceneAsyncOperationHandle { get; private set; }
        
        public void OnEnter()
        {
            coroutineProvider.StartCoroutine(OnEnterCoroutine());
        }

        private IEnumerator OnEnterCoroutine()
        {
            yield return InitState();
        }
        
        private IEnumerator InitState()
        {
            StateSceneAsyncOperationHandle = Addressables.LoadSceneAsync(gameSceneCatalogue.BoardScene, LoadSceneMode.Additive);
            yield return StateSceneAsyncOperationHandle;
            
            signalBus.Fire<Match3Signals.CreateBoardSignal>();

        }

        public void OnExit()
        {
            if (StateSceneAsyncOperationHandle.IsDone)
                Addressables.UnloadSceneAsync(StateSceneAsyncOperationHandle);
        }
    }
}