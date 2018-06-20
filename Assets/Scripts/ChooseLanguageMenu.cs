using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChooseLanguageMenu : MonoBehaviour
{
    public AudioSource clickSound;

    // Buttons
    public Button english;
    public Image englishImg;
    public Button spanish;
    public Image spanishImg;

    private List<Button> buttonList;
    private List<Image> imageList;

    private int indice = 0;
    private bool onHold = false;

    // Use this for initialization
    void Start()
    {
        buttonList = new List<Button>();
        imageList = new List<Image>();

        buttonList.Add(english);
        buttonList.Add(spanish);

        imageList.Add(englishImg);
        imageList.Add(spanishImg);

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
            clickSound.Play();
            if (buttonList[indice] == english)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenuEN");
            }
            else if (buttonList[indice] == spanish)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
            }
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            onHold = true;
        }
    }

    public void OnMouseOverEnglish()
    {
        if (indice != 0)
        {
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 0;
            imageList[indice].gameObject.SetActive(true);
        }
    }
    public void OnMouseOverSpanish()
    {
        if (indice != 1)
        {
            clickSound.Play();
            imageList[indice].gameObject.SetActive(false);
            indice = 1;
            imageList[indice].gameObject.SetActive(true);
        }
    }
}
