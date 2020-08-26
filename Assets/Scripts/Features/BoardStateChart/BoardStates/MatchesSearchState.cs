using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Components;
using Features.Config;
using Models;
using Signals;
using Unity.Entities;
using UnityEngine;
using Utils;
using Zenject;

namespace DefaultNamespace.States.BoardStates
{
    public class MatchesSearchState : BoardBaseState
    {  
        [Inject] 
        private BoardModel boardModel;
        
        [Inject] 
        private GameStateModel gameStateModel;
	
        [Inject] 
        private PlayerScoreModel playerScoreModel;
        
        [Inject] 
        private GameConfig gameConfig;

        [Inject] 
        private EntityManager entityManager;
        
        [Inject] 
        private CoroutineProvider coroutineProvider; 
        
        [Inject] 
        private SignalBus signalBus;
        
        public override void OnEnter()
        {
            var isPrevStateSwap = gameStateModel.State == Match3State.SwapInProgress;
            base.OnEnter();
            Debug.Log("Check matches -> " + gameStateModel.State);
            var matches = boardModel.FindMatches();
            
            //Found matches
            if (matches.Count > 0)
            {
                DestroyMatches(matches);
                if (isPrevStateSwap)
                    playerScoreModel.Turns--;

                coroutineProvider.StartCoroutine(WaitForDestruction());
                return;
            }
            
            //No matches found
            if (isPrevStateSwap)
            {
                //Start revert swap
                signalBus.Fire(new Match3Signals.StartSwapSignal(boardModel.FirstEntitySwapPosition,
                    boardModel.SecondEntitySwapPosition, true));
            }
            else
            {
                signalBus.Fire(new Match3Signals.StateChartSignal(BoardStateEvents.NoMatchesFoundEvent));
            }
        }

        private void DestroyMatches(List<MatchInfo> matches)
        {
            int totalScore = 0;
            foreach (var matchInfo in matches)
            {
                foreach (var gemEntity in matchInfo.Gems)
                {
                    var positionComponent = entityManager.GetComponentData<BoardPositionComponent>(gemEntity);
                    boardModel.ClearAt(positionComponent.Position);
                    entityManager.AddComponentData(gemEntity, new DestroyedComponent());
                }

                totalScore += gameConfig.GetMatchRewardScore(matchInfo.Gems.Count);
            }

            playerScoreModel.Score += totalScore;
        }

        private IEnumerator WaitForDestruction()
        {
            yield return new WaitForSeconds(0.2f);
            signalBus.Fire(new Match3Signals.StateChartSignal(BoardStateEvents.MatchesFoundEvent));
        }
    }
}