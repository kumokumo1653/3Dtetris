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
		public TetrisMino(int type,int height, int width)
		{
			int[][] Mino = Blocks[type];
			this.height = height;
			this.width = width;
			this.type = type;
			
		}

		//ミノ生成関数
		public int[][] GenerateMino(int[,] field)
		{
			int[][] mino =Blocks[type];

		
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
		public int[][] RotateMino(int[][] mino, int rotateNum)
		{
			int[][] rotatedMino = new int[][]
			{
				new int[]{0},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0},
				new int[]{0,0}
			};
			//回転回数の正規化
			int rotationTimes = rotateNum % mino[0][0];
			//元の形に戻ったら(O型は常にこれ)
			if (rotationTimes == 0)
				return Blocks[type];
			int rotationAngle = rotationTimes * 90;
			for (int i = 1; i < mino.Length; i++)
			{
				//y方向
				rotatedMino[i][1] = mino[i][0] * Sin(rotationAngle) + mino[i][1] * Cos(rotationAngle);
				//ｘ方向
				rotatedMino[i][0] = mino[i][0] * Cos(rotationAngle) - mino[i][1] * Sin(rotationAngle);
				
			}
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
