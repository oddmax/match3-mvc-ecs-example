using Features.BoardStateChart;
using Features.Data.Components;
using Features.Models;
using Features.Signals;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Features.Commands
{
    /// <summary>
    /// Triggers swap on the board
    /// </summary>
    public class StartSwapCommand
    {
        [Inject] 
        private EntityManager entityManager;
        
        [Inject] 
        private BoardModel boardModel;
        
        [Inject] 
        private GameStateModel gameStateModel; 
        
        [Inject] 
        private SignalBus signalBus;

        public void Execute(Match3Signals.StartSwapSignal startSwapSignal)
        {
            Debug.Log("Start swap -> " + gameStateModel.State);
            
            var firstSwapEntity = boardModel.GetEntityAt(startSwapSignal.FirstSwapPosition);
            var secondSwapEntity = boardModel.GetEntityAt(startSwapSignal.SecondSwapPosition);
            
            var firstEntityBoardPosition = entityManager.GetComponentData<BoardPositionComponent>(firstSwapEntity);
            var secondEntityBoardPosition = entityManager.GetComponentData<BoardPositionComponent>(secondSwapEntity);
            
            entityManager.AddComponent<SwappingComponent>(firstSwapEntity);
            entityManager.AddComponent<SwappingComponent>(secondSwapEntity);
                
            var firstEntitySwapComponent = new SwappingComponent {OriginBoardPosition = firstEntityBoardPosition.Position, TargetBoardPosition = secondEntityBoardPosition.Position};
            var secondEntitySwapComponent = new SwappingComponent {OriginBoardPosition = secondEntityBoardPosition.Position, TargetBoardPosition = firstEntityBoardPosition.Position};
            
            entityManager.SetComponentData(firstSwapEntity, firstEntitySwapComponent);
            entityManager.SetComponentData(secondSwapEntity, secondEntitySwapComponent);

            if (startSwapSignal.isRevertSwap == false)
            {
                boardModel.SaveSwapInfo(startSwapSignal.FirstSwapPosition, startSwapSignal.SecondSwapPosition);
                signalBus.Fire(new Match3Signals.StateChartSignal(BoardStateEvents.StartSwapEvent));
            }
            else
            {
                signalBus.Fire(new Match3Signals.StateChartSignal(BoardStateEvents.RevertSwapEvent));
            }
        }
    }
}