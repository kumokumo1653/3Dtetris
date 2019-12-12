using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tetris
{

	public class Block : MonoBehaviour
	{
		const int type = 7;
		[HideInInspector]
		//ミノの定義
		//0,0を中心に相対的にブロックがあるところを指定.0,0は末尾要素にこれは絶対座標
		//先頭の要素一個のとこは回転パターン
		public int[][][] Blocks { get; }
		//SRS仕様オフセット順番はBlocksと同じ
		public int[][][][] MinoOffset { get; }
		public Block()
		{
			Blocks = new int[][][]
			{
				//I型
				new int[][]{
					new int[]{4},
					new int[]{-1,0},
					new int[]{1,0},
					new int[]{2,0},
					new int[]{0,0}
				},
				//O型左下を原点に
				new int[][]{
					new int[]{1},
					new int[]{1,0},
					new int[]{0,1},
					new int[]{1,1},
					new int[]{0,0}
				},
				//S型
				new int[][]{
					new int[]{4},
					new int[]{-1,0},
					new int[]{0,1},
					new int[]{1,1},
					new int[]{0,0}
				},
				//L型
				new int[][]{
					new int[]{4},
					new int[]{1,0},
					new int[]{-1,0},
					new int[]{1,1},
					new int[]{0,0}
				},
				//T型
				new int[][]{
					new int[]{4},
					new int[]{1,0},
					new int[]{-1,0},
					new int[]{0,1},
					new int[]{0,0}
				},
				//J型　
				new int[][]
				{
					new int[]{4},
					new int[]{1,0},
					new int[]{-1,0},
					new int[]{-1,1},
					new int[]{0,0}
				},
				//Z型
				new int[][]{
					new int[]{4},
					new int[]{1,0},
					new int[]{0,1},
					new int[]{-1,1},
					new int[]{0,0}
				}
				
			};
		MinoOffset = new int[][][][]
			{
				//Iミノ
				new int[][][]
				{
					//0
					new int[][]
					{
						new int[]{0,0},
						new int[]{-1,0},
						new int[]{2,0},
						new int[]{-1,0},
						new int[]{2,0},
					},
					//R
					new int[][]
					{
						new int[]{-1,0},
						new int[]{0,0},
						new int[]{0,0},
						new int[]{0,1},
						new int[]{0,-2},
					},
					//2
					new int[][]
					{
						new int[]{-1,1},
						new int[]{1,1},
						new int[]{2,1},
						new int[]{1,0},
						new int[]{-2,0},
					},
					//L
					new int[][]
					{
						new int[]{0,1},
						new int[]{0,1},
						new int[]{1,0},
						new int[]{0,-1},
						new int[]{0,2},
					}
				},
				//Oミノ
				new int[][][]
				{
					//0
					new int[][]
					{
						new int[]{0,0}
					},
					//R
					new int[][]
					{
						new int[]{0,-1}
					},
					//2
					new int[][]
					{
						new int[]{-1,-1}
					},
					//L
					new int[][]
					{
						new int[]{-1,0}
					},
				},
				//Sミノ
				new int[][][]
				{
					//0
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//R
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 1,0},
						new int[]{ 1,-1},
						new int[]{ 0,2},
						new int[]{ 1,2}
					},
					//2
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//L
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ -1,0},
						new int[]{ -1,-1},
						new int[]{ 0,2},
						new int[]{ -1,2}
					},
				},
				//Lミノ
				new int[][][]
				{
					//0
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//R
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 1,0},
						new int[]{ 1,-1},
						new int[]{ 0,2},
						new int[]{ 1,2}
					},
					//2
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//L
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ -1,0},
						new int[]{ -1,-1},
						new int[]{ 0,2},
						new int[]{ -1,2}
					},
				},
				//Tミノ
				new int[][][]
				{
					//0
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//R
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 1,0},
						new int[]{ 1,-1},
						new int[]{ 0,2},
						new int[]{ 1,2}
					},
					//2
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//L
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ -1,0},
						new int[]{ -1,-1},
						new int[]{ 0,2},
						new int[]{ -1,2}
					},
				},
				//Jミノ
				new int[][][]
				{
					//0
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//R
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 1,0},
						new int[]{ 1,-1},
						new int[]{ 0,2},
						new int[]{ 1,2}
					},
					//2
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//L
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ -1,0},
						new int[]{ -1,-1},
						new int[]{ 0,2},
						new int[]{ -1,2}
					},
				},
				//Zミノ
				new int[][][]
				{
					//0
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//R
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 1,0},
						new int[]{ 1,-1},
						new int[]{ 0,2},
						new int[]{ 1,2}
					},
					//2
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0},
						new int[]{ 0,0}
					},
					//L
					new int[][]
					{
						new int[]{ 0,0},
						new int[]{ -1,0},
						new int[]{ -1,-1},
						new int[]{ 0,2},
						new int[]{ -1,2}
					},
				}
			};
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
