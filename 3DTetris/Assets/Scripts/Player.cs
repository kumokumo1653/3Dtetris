using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
	public class Player : MonoBehaviour
	{
		public int inputNum { get; set; }
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			inputNum = KeyEvent();
		}

		public int KeyEvent()
		{
			if (Input.anyKeyDown)
			{
				if (Input.GetKeyDown(KeyCode.A))
					return 2;
				if (Input.GetKeyDown(KeyCode.D))
					return 1;
				if (Input.GetKeyDown(KeyCode.S))
					return 3;
				if (Input.GetKeyDown(KeyCode.J))
					return 5;
				if (Input.GetKeyDown(KeyCode.L))
					return 6;
			}
			return -1;
		}
	}
}
