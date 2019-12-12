using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
	public class SceneData : MonoBehaviour
	{
		public readonly static SceneData Instance = new SceneData();

		public string referer = string.Empty;

	}
}
