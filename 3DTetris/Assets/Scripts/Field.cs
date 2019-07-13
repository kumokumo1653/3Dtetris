using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tetris
{
	public class Field : MonoBehaviour
	{
		//盤面は10×5だたし、11,12段目からミノは出現
		const int width = 10;
		const int height = 15;
		//ミノのプレハブの名前(string)を配列に.Blockクラスの配列と同じ順番にする。
		public string[] minoPrefabs;
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
		private GameObject thisMinoObject;//その時操作できるオブジェクトを格納
		private GameObject[] lineObjects;//行単位で操作するときの親オブジェクトの配列
		private bool coroutineFlag;//遊び時間実装時のフラグ
		private bool keyInputFlag;//キー入力があったかどうか
		// Use this for initialization
		void Start()
		{
			coroutineFlag = true;
			keyInputFlag = false;
			player = GetComponent<Player>();
			minoNums = new List<int> { 0, 1, 2, 3, 4, };
			int random = UnityEngine.Random.Range(0, minoNums.Count);
			InitField(field, height + 5, width + 2);
			nextOrigin = Setup(field, minoNums[random]);
			minoNums.RemoveAt(random);
			originY = nextOrigin[0];
			originX = nextOrigin[1];
			lineObjects = new GameObject[height+4];
			//行オブジェクトの生成
			for (int i = 1; i < lineObjects.Length; i++)
			{
				GameObject lineObject = new GameObject();
				lineObject.transform.position = new Vector3(0f, i, 0f);
				lineObject.name = "line" + i.ToString();
				lineObjects[i] = lineObject;
			}
			
		}

		// Update is called once per frame
		void Update()
		{

			int[][] changedMino = KeyEventReception(field, thisMino, originY, originX);
			if (JudgeFallen(field, changedMino, originY, originX) && coroutineFlag)
			{
				coroutineFlag = false;
				IEnumerator movingCoroutine = MinoFixCoroutine(0.5f);
				StartCoroutine(movingCoroutine);
				
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
			thisMinoObject = (GameObject)Resources.Load(minoPrefabs[type]);
			thisMinoObject = Instantiate(thisMinoObject, new Vector3(originX, originY, 0.0f), Quaternion.identity);
			return new int[] { originY, originX };
		}

		public int[][] KeyEventReception(int[,] field,int[][] changeMino,int originY, int originX)
		{
			//入力があったら
			if (player.inputNum > 0)
			{
				
				if (player.inputNum < 5)
				{
					int[][] movedMino = tetrisMino.MoveMino(changeMino, player.inputNum, thisMinoObject);

					int[] nextOrigin = UpdateField(field, changeMino, movedMino, originY, originX, false);
					if (nextOrigin[0] == -1 && nextOrigin[1] == -1)
					{
						Debug.Log("そこには動かせません");
						movedMino[movedMino.Length - 1][0] = 0;
						movedMino[movedMino.Length - 1][1] = 0;

					}
					else
					{
						keyInputFlag = true;
						nextOrigin = UpdateField(field, changeMino, movedMino, originY, originX, true);
						this.originY = nextOrigin[0];
						this.originX = nextOrigin[1];
						//実際のミノの移動
						switch (player.inputNum)
						{
							case 1:
								thisMinoObject.transform.position += Vector3.right;
								break;
							case 2:
								thisMinoObject.transform.position += Vector3.left;
								break;
							case 3:
								thisMinoObject.transform.position += Vector3.down;
								break;
						}
					}
					DebugArray(field);

					return movedMino;

				}
				else
				{
					int[][] rotatedMino;
					if (player.inputNum == 5)
					{
						rotatedTimes++;
						rotatedMino = tetrisMino.RotateMino(rotatedTimes, thisMinoObject,1,field,changeMino,originY,originX);
					}
					else
					//if(player.inputNum == 6)だったら
					{
						rotatedTimes--;
						rotatedMino = tetrisMino.RotateMino(rotatedTimes, thisMinoObject, -1, field, changeMino, originY, originX);

					}

					int[] nextOrigin = UpdateField(field, changeMino, rotatedMino, originY, originX, false);
					if (nextOrigin[0] == -1 && nextOrigin[1] == -1)
					{ 
						Debug.Log("そこには動かせません");
						if (player.inputNum == 5)
							rotatedMino = tetrisMino.RotateMino(--rotatedTimes, thisMinoObject);
						else if (player.inputNum == 6)
							rotatedMino = tetrisMino.RotateMino(++rotatedTimes, thisMinoObject);
					}
					else
					{
						keyInputFlag = true;
						nextOrigin = UpdateField(field, changeMino, rotatedMino, originY, originX, true);
						this.originY = nextOrigin[0];
						this.originX = nextOrigin[1];
						//changeMinoの更新
						for (int i = 1; i < changeMino.Length; i++)
						{
							changeMino[i][0] = rotatedMino[i][0];
							changeMino[i][1] = rotatedMino[i][1];
						}

					}
					DebugArray(field);
					return rotatedMino;
				}
			}
			
			return changeMino;
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
					lineNums[index] = i;
					index++;
					for (int j = 1; j < field.GetLength(1) - 1; j++)
						field[i, j] = 0;
					//その行の親オブジェクトの子オブジェクトを全部消す。
					foreach (Transform childObject in lineObjects[i].transform)
						Destroy(childObject.gameObject);
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
			for (int i = 0; i < killNums.Length; i++)
				Debug.Log(killNums[i]);
			for (int i = 0; i < killNums.Length; i++)
			{
				for(int j = killNums[i] - i; j < field.GetLength(0) - 2; j++)
				{
					for (int k = 1; k < field.GetLength(1) - 1; k++)
					{
						field[j, k] = field[j + 1, k];
					}
					//ミノのオブジェクトを下にずらす
					//配列入れ替え
					Debug.Log(j + "," + (j + 1));
					GameObject temp = lineObjects[j];
					lineObjects[j] = lineObjects[j + 1];
					lineObjects[j + 1] = temp;
					//リネーム
					lineObjects[j + 1].name = "line" + (j + 1).ToString();
					lineObjects[j].name = "line" + j.ToString();
					//座標更新
					lineObjects[j + 1].transform.position += Vector3.up;
					lineObjects[j].transform.position += Vector3.down;
				}
				DebugArray(field);
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

		//ミノが固定されるまで遊び時間の実装
		IEnumerator MinoFixCoroutine(float waitTimes)
		{
			while (keyInputFlag)
			{
				keyInputFlag = false;
				yield return new WaitForSeconds(waitTimes);
			}
			if (JudgeFallen(field, thisMino, originY, originX))
				MinoFixProcess();
			else
				coroutineFlag = true;
		}

		//ミノを固定する関数
		public void MinoFixProcess()
		{
			coroutineFlag = true;
			//ミノの親オブジェクトを削除
			//子オブジェクトを配列に格納
			//行ごとの親オブジェクトにいれる。
			GameObject[] childObjects = new GameObject[thisMinoObject.transform.childCount];
			for (int i = 0; i < thisMinoObject.transform.childCount; i++)
				childObjects[i] = thisMinoObject.transform.GetChild(i).gameObject;
			thisMinoObject.transform.DetachChildren();
			Destroy(thisMinoObject);
			foreach (GameObject childObject in childObjects)
				childObject.transform.parent = lineObjects[(int)Math.Round(childObject.transform.position.y)].transform;
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
				minoNums = new List<int> { 0, 1, 2, 3, 4 };
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
