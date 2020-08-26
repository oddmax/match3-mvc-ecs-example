using DG.Tweening;
using Features.Models;
using Features.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Presenters
{
    /// <summary>
    /// Main ui in board states responsible for updating score and turns 
    /// </summary>
    public class BoardUiPresenter : MonoBehaviour
    {
        [Inject] 
        private SignalBus signalBus;
		
        [Inject] 
        private PlayerScoreModel playerScoreModel;
        
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI turnText;
        public Vector3 defaultScale;
        public Vector3 punchScale = new Vector3(1.2f,1.2f,1.2f);
        public Button backButton;
        
        private void Start()
        {
            signalBus.Subscribe<Match3Signals.PlayerScoreChangedSignal>(OnPlayerScoreChanged);
            signalBus.Subscribe<Match3Signals.TurnAmountChangedSignal>(OnPlayerTurnChanged);
        }

        private void OnPlayerTurnChanged()
        {
            turnText.text = playerScoreModel.Turns.ToString();
            turnText.transform.DOPunchScale(punchScale, 0.3f, 5, 0.3f);
        }

        private void OnPlayerScoreChanged()
        {
            scoreText.text = playerScoreModel.Score.ToString();
            scoreText.transform.DOPunchScale(punchScale, 0.3f, 5, 0.3f);
        }

        public void OnRestartClick()
        {
            signalBus.Fire<ExitToMapSignal>(new ExitToMapSignal());
        }
    }
}