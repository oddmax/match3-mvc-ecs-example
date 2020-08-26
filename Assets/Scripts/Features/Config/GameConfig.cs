using UnityEngine;

namespace Features.Config
{
	/// <summary>
	/// Main game configuration with levels, rewards, etc
	/// </summary>
	public class GameConfig : ScriptableObject
	{
		public LevelConfig[] levels;
		public PlayerConfig playerConfig;
		public GemMatchRewardConfig[] matchRewards;

		public int GetMatchRewardScore(int matchSize)
		{
			foreach (var matchRewardConfig in matchRewards)
			{
				if (matchRewardConfig.MatchSize == matchSize)
					return matchRewardConfig.Score;
			}

			return 0;
		}
	}
}