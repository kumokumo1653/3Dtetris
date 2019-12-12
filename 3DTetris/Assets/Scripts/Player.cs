using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetris
{
	public class Player : MonoBehaviour
	{
		public int inputNum { get; set; }
		[HideInInspector]
		public int keyWaitTime = 12;
		private int count = 0;
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			inputNum = KeyEvent();
			//Debug.LogError(count+":::"+inputNum);
		}

		public int WaitKey(int keyNum)
		{
			count++;
			if (count > keyWaitTime || count == 1)
				return keyNum;
			else return -1;

		}
		public int KeyEvent()
		{
			if (Input.anyKey)
			{

				if (Input.GetKey(KeyCode.A) || Input.GetButton("Left"))
				{
					return WaitKey(2);
				}
				else if (Input.GetKey(KeyCode.D) || Input.GetButton("Right"))
				{
					return WaitKey(1);
				}
				else if (Input.GetKey(KeyCode.S) || Input.GetButton("Down"))
				{
					
					return WaitKey(3);
				}
				else if( Input.GetKey(KeyCode.W) || Input.GetButton("Up"))
				{ 
						return WaitKey(4);
				}
				else if (Input.GetKey(KeyCode.J) || Input.GetButton("Cross"))
				{
					return WaitKey(5);
				}
				else if (Input.GetKey(KeyCode.L) || Input.GetButton("Circle"))
				{
					return WaitKey(6);
				}
				
			}
			count = 0;
			return -1;
		}
	}
}
