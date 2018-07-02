using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public Text language;
    public AudioSource audioSource;
    public AudioSource clickSound;
    public GameObject pauseCanvas;

    public GameObject dialogueCanvas;
    public GameObject controlsCanvasPC;
    public GameObject controlsCanvasController;
    private bool dialogueWasActive = false;
    // Pause Buttons
    public Button resume;
    public Image resumeImg;
    public Button controls;
    public Image controlsImg;
    public Button restart;
    public Image restartImg;
    public Button mainMenu;
    public Image mainmenuImg;
    public Button exitGame;
    public Image exitImg;
    public Image controlsActiveImage;

    public GameObject xboxUI;
    public GameObject pcUI;
    public GameObject camera;

    private List<Button> buttonList;
    private List<Image> imageList;

    private int indice = 0;
    private bool onHold = false;

    private bool onPause = false;
    // Bug fixing with Esc key
    private bool escActive = false;
    public bool xboxControllerActive = false;

	void Start ()
    {
        buttonList = new List<Button>();
        imageList = new List<Image>();

        buttonList.Add(resume);
        buttonList.Add(controls);
        buttonList.Add(restart);
        buttonList.Add(mainMenu);
        buttonList.Add(exitGame);

        imageList.Add(resumeImg);
        imageList.Add(controlsImg);
        imageList.Add(restartImg);
        imageList.Add(mainmenuImg);
        imageList.Add(exitImg);
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        SwapUI();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            audioSource.Play();
            if (pauseCanvas.activeInHierarchy)
            {
                if(dialogueWasActive)
                {
                    dialogueCanvas.SetActive(true);
                }
                controlsCanvasPC.SetActive(false);
                controlsCanvasController.SetActive(false);
                camera.GetComponent<BlurShader>().enabled = false;
                onPause = false;
                Time.timeScale = 1;
                pauseCanvas.gameObject.SetActive(false);
            }
            else
            {
                //Check if dialogue was active
                if (dialogueCanvas.activeInHierarchy)
                {
                    dialogueWasActive = true;
                    dialogueCanvas.SetActive(false);
                }
                else
                    dialogueWasActive = false;

                controlsCanvasPC.SetActive(false);
                controlsCanvasController.SetActive(false);
                camera.GetComponent<BlurShader>().enabled = true;
                Time.timeScale = 0;
                pauseCanvas.gameObject.SetActive(true);
                imageList[indice].gameObject.SetActive(false);
                indice = 0;
                imageList[indice].gameObject.SetActive(true);
                onPause = true;
                escActive = true;
            }
        }
        // Menu navigation
        if(onPause)
        {
            if (controlsCanvasPC.activeInHierarchy == false && controlsCanvasController.activeInHierarchy == false)
            {
                // Up
                if ((Input.GetAxis("Vertical") > 0.0f || Input.GetKeyDown(KeyCode.UpArrow)) && onHold == true)
                {
                clickSound.Play();
                if (indice == 0)
                {
                    imageList[indice].gameObject.SetActive(false);
                    indice = (imageList.Count - 1);
                    imageList[indice].gameObject.SetActive(true);
                    onHold = false;
                }
                else
                {
                    imageList[indice].gameObject.SetActive(false);
                    indice--;
                    imageList[indice].gameObject.SetActive(true);
                    onHold = false;
                }
                }
                // Down
            else if ((Input.GetAxis("Vertical") < 0.0f || Input.GetKeyDown(KeyCode.DownArrow)) && onHold == true)
            {
                clickSound.Play();
                if (indice == (imageList.Count - 1))
                {
                    imageList[indice].gameObject.SetActive(false);
                    indice = 0;
                    imageList[indice].gameObject.SetActive(true);
                    onHold = false;
                }
            else
                {
                    imageList[indice].gameObject.SetActive(false);
                    indice++;
                imageList[indice].gameObject.SetActive(true);
                       onHold = false;
                }
            }
            }
            // Make action
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                if (pauseCanvas.activeInHierarchy == false)
                {
                    SwapToControlsCanvas();
                }
                else
                {
                    clickSound.Play();
                    if (buttonList[indice] == resume)
                    {
                        onPause = false;
                        ResumeGame();
                    }
                    else if (buttonList[indice] == restart)
                    {
                        onPause = false;
                        if(language.text == "English")
                        {
                            SceneManager.LoadScene("SpanishSceneEN");
                        }
                        else
                            SceneManager.LoadScene("SpanishScene");
                    }
                    else if (buttonList[indice] == mainMenu)
                    {
                        onPause = false;
                        Time.timeScale = 1;
                        if (language.text == "English")
                        {
                            SceneManager.LoadScene("MainMenuEN");
                        }
                        else
                            SceneManager.LoadScene("MainMenu");

                    }
                    else if (buttonList[indice] == exitGame)
                    {
                        onPause = false;
                        Application.OpenURL("https://botttos.itch.io/thatgoodparty");
                        Application.Quit();
                    }
                    else if (buttonList[indice] == controls)
                    {
                        SwapToControlsCanvas();
                    }
                }
            }
            if (Input.GetAxis("Vertical") == 0)
            {
                onHold = true;
            }

            //Bug fixing with Esc key
            if (escActive)
            {
                escActive = false;
            }
        }
    }
    public void ResumeGame()
    {
        camera.GetComponent<BlurShader>().enabled = false;
        onPause = false;
        Time.timeScale = 1;
        pauseCanvas.gameObject.SetActive(false);
    }
    public void OnMouseOverResume()
    {
        if (indice != 0)
        {
            SetPCUI();
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 0;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverControls()
    {
        if (indice != 1)
        {
            SetPCUI();
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 1;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverRestart()
    {
        if (indice != 2)
        {
            SetPCUI();
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 2;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverMainMenu()
    {
        if (indice != 3)
        {
            SetPCUI();
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 3;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverExitGame()
    {
        if (indice != 4)
        {
            SetPCUI();
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 4;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverControlsAccept()
    {
        clickSound.Play();
        controlsActiveImage.gameObject.SetActive(true);
    }
    public void SwapUI()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            SetPCControlsController();
        }
        else
        {
            SetXboxUI();
        }
    }
    private bool SetPCUI()
    {
        xboxUI.SetActive(false);
        pcUI.SetActive(true);
        return true;
    }
    private void SetPCControlsController()
    {
        xboxControllerActive = false;
        SwapControlControllerOrPC();
    }
    private bool SetXboxUI()
    {
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
           Input.GetKey(KeyCode.Joystick1Button1) ||
           Input.GetKey(KeyCode.Joystick1Button2) ||
           Input.GetKey(KeyCode.Joystick1Button3) ||
           Input.GetKey(KeyCode.Joystick1Button4) ||
           Input.GetKey(KeyCode.Joystick1Button5) ||
           Input.GetKey(KeyCode.Joystick1Button6) ||
           Input.GetKey(KeyCode.Joystick1Button7) ||
           Input.GetKey(KeyCode.Joystick1Button8) ||
           Input.GetKey(KeyCode.Joystick1Button9) ||
           Input.GetKey(KeyCode.Joystick1Button10) ||
           Input.GetKey(KeyCode.Joystick1Button11) ||
           Input.GetKey(KeyCode.Joystick1Button12) ||
           Input.GetKey(KeyCode.Joystick1Button13) ||
           Input.GetKey(KeyCode.Joystick1Button14) ||
           Input.GetKey(KeyCode.Joystick1Button15) ||
           Input.GetKey(KeyCode.Joystick1Button16) ||
           Input.GetKey(KeyCode.Joystick1Button17) ||
           Input.GetKey(KeyCode.Joystick1Button18) ||
           Input.GetKey(KeyCode.Joystick1Button19) ||
           Input.GetAxis("Vertical") != 0.0f ||
           Input.GetAxis("Horizontal") != 0.0f)
        {
            xboxControllerActive = true;
            pcUI.SetActive(false);
            xboxUI.SetActive(true);
            SwapControlControllerOrPC();
            return true;
        }
        return false;
    }

    public void SwapToControlsCanvas()
    {
        if (controlsCanvasPC.activeInHierarchy || controlsCanvasController.activeInHierarchy)
        {
            clickSound.Play();
            controlsCanvasController.SetActive(false);
            controlsCanvasPC.SetActive(false);

            pauseCanvas.gameObject.SetActive(true);
        }
        else
        {
            if (xboxControllerActive)
            {
                controlsCanvasController.SetActive(true);
                controlsCanvasPC.SetActive(false);
            }
            else
            {
                controlsCanvasPC.SetActive(true);
                controlsCanvasController.SetActive(false);
            }
            SwapControlControllerOrPC();
            pauseCanvas.gameObject.SetActive(false);
        }
            
    }
    private void SwapControlControllerOrPC()
    {
        if (controlsCanvasController.activeInHierarchy == true && controlsCanvasPC.activeInHierarchy == false && xboxControllerActive == false)
        {
            controlsCanvasController.SetActive(false);
            controlsCanvasPC.SetActive(true);
            
        }
        else if (controlsCanvasPC.activeInHierarchy == true && controlsCanvasController.activeInHierarchy == false && xboxControllerActive == true)
        {
            controlsCanvasPC.SetActive(false);
            controlsCanvasController.SetActive(true);
        }
        
    }
}
