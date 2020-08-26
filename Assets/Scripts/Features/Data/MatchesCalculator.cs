using System.Collections.Generic;
using Data.Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Found match information
    /// </summary>
    public class MatchInfo
    {
        public List<Entity> Gems;
        public GemColor GemColor;
    }
    
    public class MatchesCalculator
    {
        private readonly Entity[][] board;
        private readonly int boardWidth;
        private readonly int boardHeight;
        private EntityManager entityManager;

        public MatchesCalculator(Entity[][] board, EntityManager entityManager)
        {
            this.board = board;
            boardWidth = board.Length;
            boardHeight = board[0].Length;
            this.entityManager = entityManager;
        }

        public List<MatchInfo> FindMatches()
        {
            var matchIndex = 1;
            var matchesMap = new int[boardWidth][];
            for (int x = 0; x < boardWidth; x++)
            {
                matchesMap[x] = new int[boardHeight];
                for (int y = 0; y < boardHeight; y++)
                {
                    matchesMap[x][y] = 0;
                }
            }

            Dictionary<int, List<int2>> matchesDict = new Dictionary<int, List<int2>>(); 
            
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    var entity = board[x][y];
                    var gemColor = entityManager.GetComponentData<GemComponent>(entity).Color;
                    //already part of the match, skip
                    if (matchesMap[x][y] == 0)
                    {
                        var horizontalMatchCount = CountSameColorMatchHorizontal(x, y, gemColor);
                        //Found a horizontal match!
                        if (horizontalMatchCount >= 3)
                        {
                            Debug.Log("Horizontal match found -> " + x + ", " + y + " - count " + horizontalMatchCount);
                            List<int> existingMatches = GetExistingMatches(x, y, horizontalMatchCount, matchesMap);
                            var newMatchIndex = MergeMatches(existingMatches, matchesMap, matchesDict);
                            if (newMatchIndex == 0)
                            {
                                newMatchIndex = matchIndex;
                                matchesDict.Add(matchIndex, new List<int2>());
                                matchIndex++;
                            }

                            AddHorizontalGemsToMatchList(x, y, horizontalMatchCount, newMatchIndex, matchesDict,
                                matchesMap);
                        }
                    }

                    var verticalMatchCount = CountSameColorMatchVertical(x, y, gemColor);
                    if (verticalMatchCount >= 3)
                    {
                        Debug.Log("Vertical match found -> " + x + ", " + y + " - count " + verticalMatchCount);

                        var newMatchIndex = matchesMap[x][y];
                        if (newMatchIndex == 0)
                        {
                            newMatchIndex = matchIndex;
                            matchesDict.Add(matchIndex, new List<int2>());
                            matchIndex++;
                        }
                        AddVerticalGemsToMatchList(x, y, verticalMatchCount, newMatchIndex, matchesDict, matchesMap);
                    }
                }
            }

            var result = ConvertToMatchInfos(matchesDict);

            return result;
        }

        private List<MatchInfo> ConvertToMatchInfos(Dictionary<int, List<int2>> matchesDict)
        {
            var result = new List<MatchInfo>();
            foreach (KeyValuePair<int, List<int2>> keyValuePair in matchesDict)
            {
                var list = keyValuePair.Value;
                var firstPos = list[0];
                var entity = board[firstPos.x][firstPos.y];
                var gemColor = entityManager.GetComponentData<GemComponent>(entity).Color;
                var gemsEntitiesList = new List<Entity>();
                foreach (int2 pos in list)
                {
                    gemsEntitiesList.Add(board[pos.x][pos.y]);
                }

                var matchInfo = new MatchInfo {GemColor = gemColor, Gems = gemsEntitiesList};
                result.Add(matchInfo);
            }

            return result;
        }

        private void AddVerticalGemsToMatchList(int startX, int startY, int count, int matchIndex, Dictionary<int,List<int2>> matchesDict, int[][] matchesMap)
        {
            var matchList = matchesDict[matchIndex];
            for (int y = startY; y < startY + count; y++)
            {
                matchList.Add(new int2(startX, y));
                matchesMap[startX][y] = matchIndex;
            }
        } 
        
        private void AddHorizontalGemsToMatchList(int startX, int startY, int count, int matchIndex, Dictionary<int,List<int2>> matchesDict, int[][] matchesMap)
        {
            var matchList = matchesDict[matchIndex];
            for (int x = startX; x < startX + count; x++)
            {
                matchList.Add(new int2(x, startY));
                matchesMap[x][startY] = matchIndex;
            }
        }

        private static List<int> GetExistingMatches(int startX , int startY, int count, int[][] matchesMap)
        {
            List<int> existingMatches = null;
            for (int x = startX; x < startX + count; x++)
            {
                //has existing vertical matches with these gems involved?
                var existingMatchIndex = matchesMap[x][startY];
                if (existingMatchIndex != 0)
                {
                    if (existingMatches == null)
                        existingMatches = new List<int>();

                    existingMatches.Add(existingMatchIndex);
                }
            }

            return existingMatches;
        }

        private int CountSameColorMatchHorizontal(int startX, int startY, GemColor gemColor)
        {
            int count = 0;
            for (int x = startX; x < boardWidth; x++)
            {
                var nextEntity = board[x][startY];
                var nextGemColor = entityManager.GetComponentData<GemComponent>(nextEntity).Color;
                if (gemColor == nextGemColor)
                    count++;
                else
                    break;
            }

            return count;
        }
        
        private int CountSameColorMatchVertical(int startX, int startY, GemColor gemColor)
        {
            int count = 0;
            for (int y = startY; y < boardHeight; y++)
            {
                var nextEntity = board[startX][y];
                var nextGemColor = entityManager.GetComponentData<GemComponent>(nextEntity).Color;
                if (gemColor == nextGemColor)
                    count++;
                else
                    break;
            }

            return count;
        }

        private int MergeMatches(List<int> existingMatches, int[][] matchesMap, Dictionary<int,List<int2>> matchesDict)
        {
            if (existingMatches != null && existingMatches.Count > 0)
            {
                var firstMatchIndex = existingMatches[0];
                var firstMatchGemsList = matchesDict[firstMatchIndex];
                if (existingMatches.Count > 1)
                {
                    for (int i = 1; i < existingMatches.Count; i++)
                    {
                        var matchIndex = existingMatches[i];
                        for (int j = 0; j < matchesDict[matchIndex].Count; j++)
                        {
                            int2 gemPos = matchesDict[matchIndex][j];
                            firstMatchGemsList.Add(matchesDict[matchIndex][j]);
                            matchesMap[gemPos.x][gemPos.y] = firstMatchIndex;
                        }

                        matchesDict.Remove(matchIndex);
                    }
                }

                return firstMatchIndex;
            }

            return 0;
        }
    }
}