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
		private int[][] thisMino;
		private int originX;
		private int originY;
		private Player player;
		// Use this for initialization
		void Start()
		{
			player = GetComponent<Player>();
			originY = height + 1;
			originX = width / 2;
			InitField(field, height + 5, width + 2);
			tetrisMino = new TetrisMino(0, height, width);
			thisMino = tetrisMino.GenerateMino(field);
			DebugArray(field);
			//int[][] rotatedMino = tetrisMino.RotateMino(thisMino, 1);
			//int[] nextOrigin = tetrisMino.UpdateField(field, thisMino, rotatedMino, originY, originX,false);
			//DebugArray(field);
			
		}

		// Update is called once per frame
		void Update()
		{
			
			//入力があったら
			if(player.inputNum > 0)
			{
				int[][] movedMino = tetrisMino.MoveMino(thisMino, player.inputNum);

				int[] nextOrigin = UpdateField(field, thisMino, movedMino, originY, originX, false);
				if (nextOrigin[0] == -1 && nextOrigin[1] == -1)
					Debug.Log("そこには動かせません");
				else
				{
					nextOrigin = UpdateField(field, thisMino, movedMino, originY, originX, true);
					originY = nextOrigin[0];
					originX = nextOrigin[1];
				}
				DebugArray(field);
			}
		}


		//回転や移動のときミノの更新.新たな原点を返す。(y,x).実際にそこにおけるかも判定
		//flagがtrueで実際に置くfalseで実際には置かない.置けなければ、負の配列を返す。おければ新しい原点を返す。
		public int[] UpdateField(int[,] field, int[][] beforeMino, int[][] afterMino, int originY, int originX, bool flag)
		{
			if (flag)
			{

				//削除
				for (int i = 1; i < beforeMino.Length; i++)
					field[originY + beforeMino[i][1], originX + beforeMino[i][0]] = 0;
				//更新
				//原点の更新
				originY += afterMino[afterMino.Length - 1][1];
				originX += afterMino[afterMino.Length - 1][0];
				field[originY, originX] = 1;
				for (int i = 1; i < afterMino.Length - 1; i++)
					field[originY + afterMino[i][1], originX + afterMino[i][0]] = 1;

				int[] nextOrigin = new int[] { originY, originX };
				afterMino[afterMino.Length - 1] = new int[] { 0, 0 };
				return nextOrigin;
			}
			else
			{
				//盤面をコピー
				int[,] copy = new int[field.GetLength(0), field.GetLength(1)];
				for (int i = 0; i < copy.GetLength(0); i++)
				{
					for (int j = 0; j < copy.GetLength(1); j++)
					{
						copy[i, j] = field[i, j];
					}
				}
				//削除
				for (int i = 1; i < beforeMino.Length; i++)
				{
					Debug.Log((originY + beforeMino[i][1]) + "," + (originX + beforeMino[i][0]));
					copy[originY + beforeMino[i][1], originX + beforeMino[i][0]] = 0;
					
				}
				//更新
				//原点の更新
				originY += afterMino[afterMino.Length - 1][1];
				originX += afterMino[afterMino.Length - 1][0];
				if (copy[originY, originX] == 1)
					return new int[] { -1, -1 };
				copy[originY, originX] = 1;
				for (int i = 1; i < afterMino.Length - 1; i++)
				{
					if (copy[originY + afterMino[i][1], originX + afterMino[i][0]] == 1)
						return new int[] { -1, -1 };
					copy[originY + afterMino[i][1], originX + afterMino[i][0]] = 1;
				}
				int[] nextOrigin = new int[] { originY, originX };
				return nextOrigin;

			}

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
