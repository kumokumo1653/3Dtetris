using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tetris
{
	public class Field : MonoBehaviour
	{
		//盤面は10×5だたし、11,12段目からミノは出現
		const int width = 7;
		const int height = 10;
		[HideInInspector]
		public int[,] field = new int[height + 5, width + 2];
		private TetrisMino tetrisMino;
		private int[][] thisMino;
		private int originX;
		private int originY;
		private Player player;
		private int rotatedTimes;
		private int[] nextOrigin;
		private List<int> minoNums;
		// Use this for initialization
		void Start()
		{
			player = GetComponent<Player>();
			minoNums = new List<int> { 0, 1, 2, 3, 4, };
			int random = UnityEngine.Random.Range(0, minoNums.Count);
			InitField(field, height + 5, width + 2);
			nextOrigin = Setup(field, minoNums[random]);
			minoNums.RemoveAt(random);
			originY = nextOrigin[0];
			originX = nextOrigin[1];
			/*int[][] rotateMino = tetrisMino.RotateMino(1);
			nextOrigin  = UpdateField(field, thisMino, rotateMino, originY, originX, true);
			originY = nextOrigin[0];
			originX = nextOrigin[1];
			DebugArray(field);
			for (int i = 1; i < thisMino.Length; i++)
			{
				thisMino[i][0] = rotateMino[i][0];
				thisMino[i][1] = rotateMino[i][1];
			}
			rotateMino = tetrisMino.RotateMino(2);
			for (int i = 1; i < rotateMino.Length; i++)
				Debug.Log(rotateMino[i][1] + "," + rotateMino[i][0]);
			nextOrigin = UpdateField(field, thisMino, rotateMino, originY, originX, true);
			originY = nextOrigin[0];
			originX = nextOrigin[1];
			DebugArray(field);
			*/
			
		}

		// Update is called once per frame
		void Update()
		{

			nextOrigin = KeyEventReception(field, thisMino, originY, originX);
			originY = nextOrigin[0];
			originX = nextOrigin[1];

			if (originX == -1 && originY == -1)
			{
				//ラインが消えたなら
				int[] killNums = JudgeKillLine(field);
				if (killNums.Length >= 1)
				{
					UpdateField(field, killNums);
				}
				int random = UnityEngine.Random.Range(0, minoNums.Count);
				Setup(field, minoNums[random]);
				minoNums.RemoveAt(random);
				if (minoNums.Count == 0)
					minoNums = new List<int>{0,1,2,3,4 };
			}
			
		}

		public int[] Setup(int [,] field ,int type)
		{
			tetrisMino = new TetrisMino(type, height, width);
			thisMino = tetrisMino.GenerateMino(field);
			DebugArray(field);
			originY = height + 1;
			originX = width / 2;
			rotatedTimes = 0;
			return new int[] { originY, originX };
		}

		public int[] KeyEventReception(int[,] field,int[][] changeMino,int originY, int originX)
		{
			//入力があったら
			if (player.inputNum > 0)
			{
				if (player.inputNum < 5)
				{
					int[][] movedMino = tetrisMino.MoveMino(changeMino, player.inputNum);

					int[] nextOrigin = UpdateField(field, changeMino, movedMino, originY, originX, false);
					if (nextOrigin[0] == -1 && nextOrigin[1] == -1)
					{
						Debug.Log("そこには動かせません");
						movedMino[movedMino.Length - 1][0] = 0;
						movedMino[movedMino.Length - 1][1] = 0;

					}
					else
					{
						nextOrigin = UpdateField(field, changeMino, movedMino, originY, originX, true);
						originY = nextOrigin[0];
						originX = nextOrigin[1];
					}
					DebugArray(field);
					if (JudgeFallen(field, movedMino, originY, originX))
						return new int[] { -1, -1 };
					else
						return new int[] { originY, originX };

				}
				else
				{
					rotatedTimes++;
					int[][] rotatedMino = tetrisMino.RotateMino(rotatedTimes);
					int[] nextOrigin = UpdateField(field, changeMino, rotatedMino, originY, originX, false);
					if (nextOrigin[0] == -1 && nextOrigin[1] == -1)
					{
						Debug.Log("そこには動かせません");
						rotatedMino = tetrisMino.RotateMino(rotatedTimes - 1);
					}
					else
					{
						nextOrigin = UpdateField(field, changeMino, rotatedMino, originY, originX, true);
						originY = nextOrigin[0];
						originX = nextOrigin[1];
						for(int i = 1; i < changeMino.Length; i++)
						{
							changeMino[i][0] = rotatedMino[i][0];
							changeMino[i][1] = rotatedMino[i][1];
						}
					}
					DebugArray(field);
					if(JudgeFallen(field, rotatedMino, originY, originX))
						return new int[] { -1, -1 };
					else
						return new int[] { originY, originX };
				}
			}
			return new int[] { originY, originX };
		}


		//ラインが消せれば消す。

		public int[] JudgeKillLine(int[,] field) {
			bool flag = false;
			int index = 0;
			int[] lineNums = new int[field.GetLength(0)];
			for(int i = 1;i < field.GetLength(0) - 1; i++)
			{
				flag = true;
				for(int j  = 0; j < field.GetLength(1); j++)
				{
					if (field[i, j] == 0)
						flag = false;
				}
				if (flag)
				{
					lineNums[index] = 1;
					index++;
					for (int j = 1; j < field.GetLength(1) - 1; j++)
						field[i, j] = 0;
				}
			}
			Array.Resize(ref lineNums, index);
			return lineNums;
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
					copy[originY + beforeMino[i][1], originX + beforeMino[i][0]] = 0;
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
		//オーバーロード
		//消えたラインを埋める
		public void UpdateField(int[,] field, int[] killNums)
		{
			for(int i = 0; i < killNums.Length; i++)
			{
				for(int j = killNums[i] - i; j < field.GetLength(0) - 2; j++)
				{
					for (int k = 1; k < field.GetLength(1) - 1; k++)
					{
						field[j, k] = field[j + 1, k];
					}
				}
			}
		}
		//ミノが落ちきったか判定　true 落ちきった　false　落ちてない
		public bool JudgeFallen(int[,]field ,int[][] mino,int originY,int originX)
		{
			bool flag = false;
			for(int i = 1; i < mino.Length; i++)
			{
				//ブロックの下が1だったら
				if(field[originY + mino[i][1] - 1,originX + mino[i][0]] == 1)
				{
					flag = true;
					for(int j = 1; j < mino.Length; j++)
					{
						//それがミノ自身のブロックだったら
						if (originY + mino[i][1] - 1 == originY + mino[j][1] &&
							originX + mino[i][0] == originX + mino[j][0])
							flag = false;
						
					}
					if (flag)
						return true;
				}
			}
			return flag;
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
