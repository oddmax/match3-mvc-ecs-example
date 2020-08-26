using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Components;
using Features.Config;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// Main board model, which keeps a map which gem is on which position in the matrix and provides handy operation with it
    /// </summary>
    public class BoardModel : IDisposable
    {
        public int BoardWidth => boardWidth;
        public int BoardHeight => boardHeight;
        
        public int2 FirstEntitySwapPosition;
        public int2 SecondEntitySwapPosition;
        
        private Entity[][] board;
        private int boardWidth;
        private int boardHeight;
        private EntityManager entityManager;
        
        public BoardModel(EntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public Entity GetEntityAt(int x, int y)
        {
            return board[x][y];
        }
        
        public Entity GetEntityAt(int2 position)
        {
            return GetEntityAt(position.x, position.y);
        }
        
        public void SaveSwapInfo(int2 firstEntitySwapPosition, int2 secondEntitySwapPosition)
        {
            FirstEntitySwapPosition = firstEntitySwapPosition;
            SecondEntitySwapPosition = secondEntitySwapPosition;
        }

        public void ResetBoard(LevelConfig levelConfig)
        {
            boardWidth = levelConfig.Width;
            boardHeight = levelConfig.Height;
            
            board = new Entity[boardWidth][];
            
            for (int x = 0; x < boardWidth; x++)
                board[x] = new Entity[levelConfig.Height];
        }

        public void SetEntityAt(int x, int y, Entity gemEntity)
        {
            board[x][y] = gemEntity;
        }

        public void ClearAt(int2 position)
        {
            if(IsPositionInBorders(position) == false)
                return;
            
            board[position.x][position.y] = Entity.Null;
        }

        public bool HasEntityAt(int2 position)
        {
            if (IsPositionInBorders(position) == false)
                return false;
            
            return board[position.x][position.y] != Entity.Null;
        }

        public bool IsPositionInBorders(int2 position)
        {
            if (position.x < 0 || position.x >= boardWidth)
                return false;
            
            if (position.y < 0 || position.y >= boardHeight)
                return false;
            
            return true;
        }
        
        public List<MatchInfo> FindMatches()
        {
            var matchesCalculator = new MatchesCalculator(board, entityManager);
            return matchesCalculator.FindMatches();
        }
        
        public void Dispose()
        {
            
        }
    }
}