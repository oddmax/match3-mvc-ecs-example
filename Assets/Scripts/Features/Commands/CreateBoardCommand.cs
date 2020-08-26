using Core.Config;
using Features.Config;
using Features.Data;
using Features.Models;
using Features.Signals;
using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Features.Commands
{
    /// <summary>
    /// Initial command for Board State responsible for creating board and resetting models
    /// </summary>
    [UsedImplicitly]
    public class CreateBoardCommand
    {
        [Inject] 
        private SignalBus signalBus;
	
        [Inject] 
        private GameStateModel gameStateModel;
        
        [Inject] 
        private BoardModel boardModel;
	
        [Inject] 
        private PlayerScoreModel playerScoreModel;

        [Inject] 
        private EntityManager entityManager;
        
        [Inject] 
        private AssetsCatalogue assetsCatalogue;
	
        public void Execute(Match3Signals.CreateBoardSignal createBoardSignal)
        {
            Debug.Log("Create board -> " + gameStateModel.State);
            var levelConfig = gameStateModel.GetCurrentLevelConfig();
            playerScoreModel.Reset();
            playerScoreModel.Turns = levelConfig.Turns;
            boardModel.ResetBoard(levelConfig);
            BoardCalculator.InitBoardSize(boardModel.BoardWidth, boardModel.BoardHeight);
            CreateGems(levelConfig);
            signalBus.Fire<Match3Signals.OnBoardCreatedSignal>();
        }

        private void CreateGems(LevelConfig levelConfig)
        {
            for (int x = 0; x < levelConfig.Width; x++)
            {
                for (int y = 0; y < levelConfig.Height; y++)
                {
                    var gemColor = EntitiesHelper.GetRandomColor(levelConfig.availableColors);
                    var boardPosition = new int2(x, y);
                    var position = BoardCalculator.ConvertBoardPositionToTransformPosition(boardPosition);
                    var gemEntity = EntitiesHelper.CreateGem(gemColor,
                        position,
                        boardPosition);
                    signalBus.Fire(new Match3Signals.GemCreatedSignal(gemEntity, position));
                    boardModel.SetEntityAt(x, y, gemEntity);
                }
            }
        }
    }
}