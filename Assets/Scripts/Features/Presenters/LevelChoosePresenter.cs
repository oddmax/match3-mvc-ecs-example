using Features.Views;
using Models;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Presenters
{
    /// <summary>
    /// Main presenter of level map state
    /// </summary>
    public class LevelChoosePresenter : MonoBehaviour
    {
        [SerializeField]
        public GameObject buttonLevelSelectPrefab;

        [SerializeField]
        public GridLayoutGroup gridLayout;

        private SignalBus signalBus;
        private GameStateModel gameStateModel;

        [Inject]
        public void Inject(SignalBus signalBus, GameStateModel gameStateModel)
        {
            this.gameStateModel = gameStateModel;
            this.signalBus = signalBus;

            InitButtons();
        }

        private void InitButtons()
        {
            for (int i = 0; i < gameStateModel.LevelsAmount; i++)
            {
                var levelIndex = i;
                var gameObject = Instantiate(buttonLevelSelectPrefab, gridLayout.transform);
                var button = gameObject.GetComponent<LevelChooseButton>();
                button.SetLevelIndex(levelIndex);
                button.OnClick += StartLevel;
            }
        }

        private void StartLevel(int level)
        {
            Debug.Log("Start level " + level);
            gameStateModel.CurrentLevelIndex = level;
            signalBus.Fire(new ChangeLevelSignal(level));
        }
    }
}