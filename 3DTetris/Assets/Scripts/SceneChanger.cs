using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	// Use this for initialization
	private string activeSceneName;
	void Start () {
		activeSceneName = SceneManager.GetActiveScene().name;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey)
		{
			switch (activeSceneName)
			{
				case "MainStage":
					if (Input.anyKey)
						SceneManager.LoadScene("Ranking");
					break;
				case "Title":
					if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Start"))
					{
						if(GameObject.Find("GameManager") != null)
							SceneManager.MoveGameObjectToScene(GameObject.Find("GameManager"), SceneManager.GetActiveScene());
						SceneManager.LoadScene("MainStage");
					}
					if (Input.GetKeyDown(KeyCode.U) || Input.GetButtonDown("Select"))
						SceneManager.LoadScene("Ranking");
					break;
				case "Ranking":
					if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Start"))
					{
						if (GameObject.Find("GameManager") != null)
							SceneManager.MoveGameObjectToScene(GameObject.Find("GameManager"), SceneManager.GetActiveScene());
						SceneManager.LoadScene("MainStage");
					}
					if (Input.GetKeyDown(KeyCode.U) || Input.GetButtonDown("Select"))
						SceneManager.LoadScene("Title");
					break;
				default:
					break;
			}
		}
	}
}
