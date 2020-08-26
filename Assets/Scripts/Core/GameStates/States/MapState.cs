using System.Collections;
using Core.Config;
using StateChart;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Utils;
using Zenject;

namespace DefaultNamespace.States
{
    public class MapState : IState
    {
        [Inject]
        private CoroutineProvider coroutineProvider;
        
        [Inject]
        private GameSceneCatalogue gameSceneCatalogue;
        
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
            StateSceneAsyncOperationHandle = Addressables.LoadSceneAsync(gameSceneCatalogue.MapScene, LoadSceneMode.Additive);
            yield return StateSceneAsyncOperationHandle;
        }

        public void OnExit()
        {
            if (StateSceneAsyncOperationHandle.IsDone)
                Addressables.UnloadSceneAsync(StateSceneAsyncOperationHandle);
        }
    }
}