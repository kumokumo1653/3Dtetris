using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tetris
{
	public class TetrisMino : Block
	{
		private int height;
		private int width;
		private int type;
		private GameObject gameManager;
		private Field Board;
		public TetrisMino(int type,int height, int width)
		{
			gameManager = GameObject.Find("GameManager");
			Board = gameManager.GetComponent<Field>();
			this.height = height;
			this.width = width;
			this.type = type;
			
		}

		//ミノ生成関数
		public int[][] GenerateMino(int[,] field)
		{
			int[][] mino = new int[][]
			{
				new int[]{Blocks[type][0][0]},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0}
			};
			for (int i = 1; i < Blocks[type].Length; i++)
			{
				mino[i][0] = Blocks[type][i][0];
				mino[i][1] = Blocks[type][i][1];
			}

		
			//出現位置にミノを出現
			int originY = height + 1;
			int originX = width / 2;
			int putX = originX;
			int putY = originY;
			
			for (int i = 1; i < mino.Length; i++)
			{
				putY += mino[i][1];
				putX += mino[i][0];
				field[putY, putX] = 1;
				putX = originX;
				putY = originY;
			}
			return mino;
		}



		//roteteNum回、回転したブロックの配列を返す。
		//rotatederection　-1で右回転　1で左回転
		public int[][] RotateMino(int rotateNum, GameObject thisMinoObject, int ratatedirection,int[,] field,int[][] thisMino)
		{
			int[][] rotatedMino = new int[][]
			{
				new int[]{Blocks[type][0][0]},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0}
			};
			
			int[][] offset = new int[5][];
			
			//回転回数の正規化
			int rotationTimes = (Blocks[type][0][0] + (rotateNum % Blocks[type][0][0])) % Blocks[type][0][0];
			//回転後の状態(右回転基準)
			int afterRotataionStates = (Blocks[type][0][0] - rotationTimes) % Blocks[type][0][0];
			//回転前の状態を定義
			int beforeRotationState = (Blocks[type][0][0] + afterRotataionStates + ratatedirection) % Blocks[type][0][0];
			Debug.Log(afterRotataionStates + "," + beforeRotationState);
			//Oミノであったら
			if (type == 1)
			{
				for (int i = 1; i < Blocks[type].Length; i++)
				{
					rotatedMino[i][0] = Blocks[type][i][0];
					rotatedMino[i][1] = Blocks[type][i][1];
				}
				thisMinoObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
				return rotatedMino;
			}
			for (int i = 0; i < offset.GetLength(0); i++)
			{
				offset[i] = new int[2];
				offset[i][0] = MinoOffset[type][beforeRotationState][i][0] - MinoOffset[type][afterRotataionStates][i][0];
				offset[i][1] = MinoOffset[type][beforeRotationState][i][1] - MinoOffset[type][afterRotataionStates][i][1];
				Debug.Log(offset[i][0] + "," + offset[i][1]);
			}
			//元の形に戻ったら(O型は常にこれ)
			if (rotationTimes == 0)
			{
				for (int i = 1; i < Blocks[type].Length; i++)
				{
					rotatedMino[i][0] = Blocks[type][i][0];
					rotatedMino[i][1] = Blocks[type][i][1];
				}
				thisMinoObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
				
			}

				int rotationAngle = rotationTimes * 90;
			for (int i = 1; i < Blocks[type].Length; i++)
			{
				//y方向
				rotatedMino[i][1] = Blocks[type][i][0] * Sin(rotationAngle) + Blocks[type][i][1] * Cos(rotationAngle);
				//ｘ方向
				rotatedMino[i][0] = Blocks[type][i][0] * Cos(rotationAngle) - Blocks[type][i][1] * Sin(rotationAngle);
				
			}
			thisMinoObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));
			//SRSの判定
			int offsetNum = 0;
			while (true)
			{
				Debug.Log("SRS");
				//回転ミノの原点更新
				rotatedMino[rotatedMino.Length - 1][1] = offset[offsetNum][1];
				rotatedMino[rotatedMino.Length - 1][0] = offset[offsetNum][0];
				int[] origin = Board.UpdateField(field, thisMino, rotatedMino, false);
				if (origin[0] == -1 && origin[1] == -1)
					offsetNum++;
				else
				{
					//移動
					thisMinoObject.transform.position += new Vector3(offset[offsetNum][0], offset[offsetNum][1], 0);
					break;
				}
				if(offsetNum == offset.Length)
				{
					Debug.Log("おけない");
					//原点の修正
					rotatedMino[rotatedMino.Length - 1][1] = 0;
					rotatedMino[rotatedMino.Length - 1][0] = 0;
					break;
				}
					
			}
			return rotatedMino;
		}

		//オーバーロード
		//回転をもとに戻すよう
		public int[][] RotateMino(int rotateNum, GameObject thisMinoObject)
		{
			int[][] rotatedMino = new int[][]
			{
				new int[]{Blocks[type][0][0]},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0}
			};
			//回転回数の正規化
			int rotationTimes = (Blocks[type][0][0] + (rotateNum % Blocks[type][0][0])) % Blocks[type][0][0];
			//元の形に戻ったら(O型は常にこれ)
			if (rotationTimes == 0)
			{
				for (int i = 1; i < Blocks[type].Length; i++)
				{
					rotatedMino[i][0] = Blocks[type][i][0];
					rotatedMino[i][1] = Blocks[type][i][1];
				}
				thisMinoObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
				return rotatedMino;
			}

			int rotationAngle = rotationTimes * 90;
			for (int i = 1; i < Blocks[type].Length; i++)
			{
				//y方向
				rotatedMino[i][1] = Blocks[type][i][0] * Sin(rotationAngle) + Blocks[type][i][1] * Cos(rotationAngle);
				//ｘ方向
				rotatedMino[i][0] = Blocks[type][i][0] * Cos(rotationAngle) - Blocks[type][i][1] * Sin(rotationAngle);

			}
			thisMinoObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));
			return rotatedMino;
		}

		//ミノの移動
		//1…右
		//2…左
		//3…下
		public int[][] MoveMino(int[][] mino, int moveDirection)
		{
			int[][] movedMino = new int[][]
			{
				new int[]{mino[0][0]},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0}
			};
			for (int i = 1; i < mino.Length; i++)
				movedMino[i] = new int[] { mino[i][0], mino[i][1] };
			switch (moveDirection) {
				case 1:
					movedMino[mino.Length - 1][0] += 1;
 					break;
				case 2:
					movedMino[mino.Length - 1][0] -= 1;
 					break;
				case 3:
					movedMino[mino.Length - 1][1] -= 1;
 					break;
			}
			return movedMino;
		}
		


		//1か0を返す。
		public int Sin(int angle)
		{
			angle = angle / 90 % 4;
			switch (angle)
			{
				case 1:
					return 1;
				case 0:
				case 2:
					return 0;
				case 3:
					return -1;
				default:
					return 0;
			}
		}
		public int Cos(int angle)
		{
			angle = angle / 90 % 4;
			switch (angle)
			{
				case 1:
				case 3:
					return 0;
				case 0:
					return 1;
				case 2:
					return -1;
				default:
					return 0;
			}
		}
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
