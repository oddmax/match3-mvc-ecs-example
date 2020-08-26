using System;

namespace Features.Config
{
    /// <summary>
    /// Amount of score to give for gems match of certain size
    /// </summary>
    [Serializable]
    public struct GemMatchRewardConfig
    {
        public int MatchSize;
        public int Score;
    }
}