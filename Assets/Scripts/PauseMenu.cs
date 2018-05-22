﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public AudioSource audioSource;
    public Canvas pauseCanvas;

    // Pause Buttons
    public Button resume;
    public Image resumeImg;
    public Button restart;
    public Image restartImg;
    public Button mainMenu;
    public Image mainmenuImg;
    public Button exitGame;
    public Image exitImg;

    private List<Button> buttonList;
    private List<Image> imageList;

    private int indice = 0;

    private bool onPause = false;
	// Use this for initialization
	void Start ()
    {
        buttonList = new List<Button>();
        imageList = new List<Image>();

        buttonList.Add(resume);
        buttonList.Add(restart);
        buttonList.Add(mainMenu);
        buttonList.Add(exitGame);

        imageList.Add(resumeImg);
        imageList.Add(restartImg);
        imageList.Add(mainmenuImg);
        imageList.Add(exitImg);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
        {
            audioSource.Play();
            if (pauseCanvas.isActiveAndEnabled)
            {
                onPause = false;
                Time.timeScale = 1;
                pauseCanvas.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                pauseCanvas.gameObject.SetActive(true);
                imageList[0].gameObject.SetActive(true);
                onPause = true;
            }

        }
        // Menu navigation
        if(onPause)
        {
            // Up
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                if(indice == 0)
                {
                    imageList[indice].gameObject.SetActive(false);
                    indice = (imageList.Count-1);
                    imageList[indice].gameObject.SetActive(true);
                }
                else
                {
                    imageList[indice].gameObject.SetActive(false);
                    indice--;
                    imageList[indice].gameObject.SetActive(true);
                }
            }
            // Down
            else if (Input.GetAxis("Vertical") < 0.0f)
            {
                if (indice == (imageList.Count-1))
                {
                    imageList[indice].gameObject.SetActive(false);
                    indice = 0;
                    imageList[indice].gameObject.SetActive(true);
                }
                else
                {
                    imageList[indice].gameObject.SetActive(false);
                    indice++;
                    imageList[indice].gameObject.SetActive(true);
                }
            }
            // Make action
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
            {
                if(buttonList[indice] == resume)
                {
                    ResumeGame();
                }
                else if(buttonList[indice] == restart)
                {
                    SceneManager.LoadScene("SpanishScene");
                }
                else if(buttonList[indice] == mainMenu)
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene("MainMenu");
                }
                else if(buttonList[indice] == exitGame)
                {
                    Application.Quit();
                }
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseCanvas.gameObject.SetActive(false);
    }
}
