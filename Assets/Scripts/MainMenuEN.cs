using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuEN : MonoBehaviour
{
    public AudioSource clickSound;

    //Info to main scene
    public GameObject usingControllerIsActive;

    // Buttons
    public Button startGame;
    public Image startImg;
    public Button language;
    public Image languageImg;
    public Button exitGame;
    public Image exitImg;

    private List<Button> buttonList;
    private List<Image> imageList;

    public GameObject languageGo;
    private int indice = 0;
    private bool onHold = false;

    private bool onLanguageMenu = false;
    // Use this for initialization
    void Start()
    {
        buttonList = new List<Button>();
        imageList = new List<Image>();

        buttonList.Add(startGame);
        buttonList.Add(language);
        buttonList.Add(exitGame);

        imageList.Add(startImg);
        imageList.Add(languageImg);
        imageList.Add(exitImg);

        indice = 0;
        imageList[indice].gameObject.SetActive(true);

        //Tutorial control information
        usingControllerIsActive.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Menu navigation
        // Up
        if(onLanguageMenu == false)
        {
            if (Input.GetAxis("Vertical") > 0.0f && onHold == true)
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
            else if (Input.GetAxis("Vertical") < 0.0f && onHold == true)
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
            // Make action
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {

                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    usingControllerIsActive.SetActive(true);
                }
                else
                    usingControllerIsActive.SetActive(false);
                clickSound.Play();
                if (buttonList[indice] == startGame)
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene("SpanishSceneEN");
                }
                else if (buttonList[indice] == language)
                {
                    onLanguageMenu = true;
                    languageGo.SetActive(true);
                }
                else if (buttonList[indice] == exitGame)
                {
                    Application.Quit();
                }
            }
            if (Input.GetAxis("Vertical") == 0)
            {
                onHold = true;
            }
        }
    }

    public void OnMouseOverContinue()
    {
        if (indice != 0)
        {            
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 0;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverLanguage()
    {
        if (indice != 1)
        {
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 1;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverExitGame()
    {
        if (indice != 2)
        {
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 2;
            imageList[indice].gameObject.SetActive(true);
        }
    }
}
