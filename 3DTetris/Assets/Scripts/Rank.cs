using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;



namespace Tetris
{
	public class Rank : MonoBehaviour
	{
		private string filePath;
		private StreamReader txtReader;
		private StreamWriter txtWriter;
		private const int rankInNum = 5;
		private int[] ranking = new int[rankInNum];
		public string fileName;
		private Field Board;
		private Text rankText;
		// Use this for initialization
		void Start()
		{
			if (SceneData.Instance.referer == "MainStage")
			{
				for (int i = 0; i < ranking.Length; i++)
					ranking[i] = 0;
				rankText = this.GetComponent<Text>();
				rankText.text = "";
				filePath = Application.streamingAssetsPath + "/" + fileName;
				ReadTxt();
				Board = GameObject.Find("GameManager").GetComponent<Field>();
				UpdateRank(Board.score);
				DisplayRank();
				WriteTxt();
			}
			else
			{
				for (int i = 0; i < ranking.Length; i++)
					ranking[i] = 0;
				rankText = this.GetComponent<Text>();
				rankText.text = "";
				filePath = Application.streamingAssetsPath + "/" + fileName;
				ReadTxt();
				DisplayRank();
			}
		}

		// Update is called once per frame
		void Update()
		{

		}

		public  void ReadTxt()
		{
			if(filePath != null)
			{
				txtReader = new StreamReader(filePath, Encoding.UTF8);
				int count = 0;
				while (txtReader.Peek() != -1)
				{
					ranking[count] = int.Parse(txtReader.ReadLine());
					count++;
				}
				txtReader.Close();
			}
		}
		public void UpdateRank(int rankInVal)
		{
			for(int i = 0; i < ranking.Length; i++)
			{
				if(ranking[i] < rankInVal)
				{
					for(int j = ranking.Length - 1;j > i; j--)
					{
						ranking[j] = ranking[j - 1];
					}
					ranking[i] = rankInVal;
					break;
				}
			}
		}
		void DisplayRank()
		{
			int order = 1;
			for(int i = 0; i < ranking.Length; i++)
			{
				if (i != 0 && ranking[i] == ranking[i - 1])
					order--;
				rankText.text += order.ToString() + ".\t" + (ranking[i] * 100).ToString() + "\n";
				order++;
			}
		}
		void WriteTxt()
		{
			txtWriter = new StreamWriter(filePath, false);
			for (int i = 0; i < ranking.Length; i++)
				txtWriter.WriteLine(ranking[i]);
			txtWriter.Close();
		}
	}
}
