using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris
{
	public class PlayerText : MonoBehaviour
	{

		public Text lavelText;
		public Text scoreText;
		private int lavelVal;
		private int scoreVal;
		private Field Board;
		// Use this for initialization
		void Start()
		{
			Board = GetComponent<Field>();
			lavelVal =  int.Parse(lavelText.text);
			scoreVal = int.Parse(scoreText.text);
			
		}

		// Update is called once per frame
		void Update()
		{
			if (lavelVal != Board.lavel)
			{
				lavelVal = Board.lavel;
				lavelText.text = lavelVal.ToString();
			}
			if (scoreVal != Board.score)
			{
				scoreVal = Board.score;
				scoreText.text = (scoreVal*100).ToString();
			}
		}
	}
}
