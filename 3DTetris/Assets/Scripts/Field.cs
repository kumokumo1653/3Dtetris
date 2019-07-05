using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
	public class Field : MonoBehaviour
	{
		//盤面は10×5だたし、11,12段目からミノは出現
		const int width = 5;
		const int height = 10;
		[HideInInspector]
		public int[,] field = new int[height + 5, width + 2];
		private TetrisMino tetrisMino;
		private Block block;
		// Use this for initialization
		void Start()
		{
			int originY = height + 1;
			int originX = width / 2;
			InitField(field, height + 5, width + 2);
			tetrisMino = new TetrisMino(0, height, width);
			int[][] thisMino = tetrisMino.GenerateMino(field);
			DebugArray(field);
			int[][] rotatedMino = tetrisMino.RotateMino(thisMino, 1);
			int[] nextOrigin = tetrisMino.UpdateField(field, thisMino, rotatedMino, originY, originX);
			DebugArray(field);
			int [][] movedMino = tetrisMino.MoveMino(rotatedMino, 3);
			nextOrigin = tetrisMino.UpdateField(field, rotatedMino, movedMino, nextOrigin[0], nextOrigin[1]);

			DebugArray(field);
		}

		// Update is called once per frame
		void Update()
		{

		}

		//フィールド初期化関数
		public void InitField(int[,] field, int height, int width)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					field[i, j] = 0;
					if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
						field[i, j] = 1;
				}
			}

		}
		//デバッグ用配列表示関数
		public void DebugArray(int[,] field)
		{
			string log = "";
			for (int i = field.GetLength(0) - 1; i >= 0; i--)
			{
				for (int j = 0; j < field.GetLength(1); j++)
				{
					log += field[i, j].ToString();
					log += " ";
				}
				Debug.Log(log);
				log = "";
			}
		}


	}
}
