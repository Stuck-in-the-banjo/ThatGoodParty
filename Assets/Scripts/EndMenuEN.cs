using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndMenuEN : MonoBehaviour
{
    public AudioSource clickSound;

    // Buttons
    public Button startGame;
    public Image startImg;
    public Button exitGame;
    public Image exitImg;

    public Button franTwitter;
    public Image franImg;
    public Button ericTwitter;
    public Image ericImg;

    private List<Button> buttonList;
    private List<Image> imageList;

    private int indice = 0;
    private bool onHold = false;
    private bool onHold2 = false;

    // Use this for initialization
    void Start()
    {
        buttonList = new List<Button>();
        imageList = new List<Image>();

        buttonList.Add(startGame);
        buttonList.Add(exitGame);
        buttonList.Add(franTwitter);
        buttonList.Add(ericTwitter);

        imageList.Add(startImg);
        imageList.Add(exitImg);
        imageList.Add(franImg);
        imageList.Add(ericImg);

        indice = 0;
        imageList[indice].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Menu navigation
        // Up
        if (Input.GetAxis("Vertical") > 0.0f && onHold == true)
        {
            clickSound.Play();
            if (indice == 0 || indice == 2)
            {
                imageList[indice].gameObject.SetActive(false);
                if (indice == 0)
                {
                    indice = 1;
                }
                else
                    indice = 3;
                imageList[indice].gameObject.SetActive(true);
                onHold = false;
            }
            else
            {
                imageList[indice].gameObject.SetActive(false);
                if (indice == 1)
                {
                    indice = 0;
                }
                else
                    indice = 2;
                imageList[indice].gameObject.SetActive(true);
                onHold = false;
            }
        }
        // Down
        else if (Input.GetAxis("Vertical") < 0.0f && onHold == true)
        {
            clickSound.Play();
            if (indice == 1 || indice == 3)
            {
                imageList[indice].gameObject.SetActive(false);
                if (indice == 1)
                {
                    indice = 0;
                }
                else
                    indice = 2;
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
        //Sides
        else if ((Input.GetAxis("Horizontal") > 0.0f || Input.GetAxis("Horizontal") < 0.0f) && onHold2 == true)
        {
            clickSound.Play();
            if (indice == 0 || indice == 1)
            {
                imageList[indice].gameObject.SetActive(false);
                indice = 2;
                imageList[indice].gameObject.SetActive(true);
                onHold2 = false;
            }
            else
            {
                imageList[indice].gameObject.SetActive(false);
                indice = 0;
                imageList[indice].gameObject.SetActive(true);
                onHold2 = false;
            }
        }

        // Make action
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            clickSound.Play();
            if (buttonList[indice] == startGame)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("SpanishSceneEN");
            }
            else if (buttonList[indice] == exitGame)
            {
                Application.OpenURL("https://botttos.itch.io/thatgoodparty");
                Application.Quit();
            }
            else if (buttonList[indice] == franTwitter)
            {
                Application.OpenURL("https://twitter.com/botttos_");
            }
            else if (buttonList[indice] == ericTwitter)
            {
                Application.OpenURL("https://twitter.com/djcerdany");
            }
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            onHold = true;
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            onHold2 = true;
        }
    }

    public void OnMouseOverNewGame()
    {
        if (indice != 0)
        {
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 0;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverExitGame()
    {
        if (indice != 1)
        {
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 1;
            imageList[indice].gameObject.SetActive(true);
        }
    }

    public void OnMouseOverFranTwitter()
    {
        if (indice != 2)
        {
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 2;
            imageList[indice].gameObject.SetActive(true);
        }
    }

    public void OnMouseOverEricTwitter()
    {
        if (indice != 3)
        {
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 3;
            imageList[indice].gameObject.SetActive(true);
        }
    }
}