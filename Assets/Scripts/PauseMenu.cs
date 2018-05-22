using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    public AudioSource audioSource;
    public Canvas pauseCanvas;

    // Pause Buttons
    public Button resume;
    public Image resumeImg;
    public Button restart;
    public Image buttonImg;
    public Button mainMenu;
    public Image mainmenuImg;
    public Button exitGame;
    public Image exitImg;

    private List<Button> buttonList;
    private List<Image> imageList;

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
                onPause = true;
            }

        }
        // Menu navigation
        if(onPause)
        {
            // Make action
            if(Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
            {

            }
            // Up
            else if(Input.GetAxis("Vertical") > 0.0f)
            {
                
            }
            // Down
            else if (Input.GetAxis("Vertical") < 0.0f)
            {

            }
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseCanvas.gameObject.SetActive(false);
    }
}
