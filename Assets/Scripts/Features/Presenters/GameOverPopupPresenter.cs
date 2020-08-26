using Features.Models;
using Features.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Presenters
{
    /// <summary>
    /// Updates game over popup and shows score
    /// </summary>
    public class GameOverPopupPresenter : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public Button backButton;
        public GameObject container;
        public GameObject containerBackground;

        [Inject] 
        private PlayerScoreModel playerScoreModel;

        [Inject] 
        private SignalBus signalBus;

        private void Start()
        {
            signalBus.Subscribe<Match3Signals.OutOfTurnsSignal>(OnOutOfTurnsSignal);
            container.SetActive(false);
            containerBackground.SetActive(false);
            backButton?.onClick.AddListener(OnRestartClick);
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<Match3Signals.OutOfTurnsSignal>(OnOutOfTurnsSignal);
        }

        private void OnEnable()
        {
            if(playerScoreModel != null)
                scoreText.text = playerScoreModel.Score.ToString();
        }
        
        private void OnOutOfTurnsSignal(Match3Signals.OutOfTurnsSignal signal)
        {
            container.SetActive(true);
            containerBackground.SetActive(true);
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            scoreText.text = playerScoreModel.Score.ToString();
        }
        
        public void OnRestartClick()
        {
            signalBus.Fire<ExitToMapSignal>(new ExitToMapSignal());
        }
    }
}