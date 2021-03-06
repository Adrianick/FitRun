﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	private SoundManager soundManager;
	private Scene scene;

	void Start()
	{
		Debug.Log("Application ending after " + Time.time + " seconds");
		scene = SceneManager.GetActiveScene();
		soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
	}

	// Update is called once per frame
	void Update()
    {
		if(menuButtonController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
			if (Input.GetAxis("Submit") == 1)
			{
				if (thisIndex == 0)
				{
					Play();
				}
				else if (thisIndex == 1)
				{
					if (scene.name == "RestartMenu")
					{
						BackToMenu();
					} 
					else {
						SoundOn();

					}
				}
				else if (thisIndex == 2)
				{
					if (scene.name == "RestartMenu")
					{
						GoToHighScoreTable();
					}
					else
					{
						SoundOff();

					}

				}
				else if (thisIndex == 3)
                {
					Quit();
                }
				animator.SetBool("pressed", true);
			}
			else if (animator.GetBool ("pressed")){
				animator.SetBool ("pressed", false);
				//animatorFunctions.disableOnce = true;
			}
		}else{
			animator.SetBool ("selected", false);
		}
    }

	void GoToHighScoreTable()
    {
		SceneManager.LoadSceneAsync("HighScore");

	}

	void Play()
	{
		SceneManager.LoadSceneAsync("WS10");
	}

	void SoundOff()
	{
		soundManager.OffSound();

	}

	void SoundOn()
	{
		soundManager.OnSound();
	}

	void Quit()
	{
		Debug.Log("You pressed the quit button");
		Application.Quit();
	}

	void BackToMenu()
    {
		SceneManager.LoadSceneAsync("MainMenu");
	}
}
