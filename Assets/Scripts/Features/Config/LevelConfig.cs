using System.Collections.Generic;
using Features.Data;
using UnityEngine;

namespace Features.Config
{
	/// <summary>
	/// Config for a specific level, we can setup board size, amount of turns and which gems will be spawned
	/// </summary>
	public class LevelConfig : ScriptableObject
	{
		[SerializeField, Range(5, 50)]
		public int Width;
		
		[SerializeField, Range(5, 50)]
		public int Height;

		[SerializeField, Range(1, 500)] 
		public int Turns;

		public List<GemColor> availableColors;
	}
}