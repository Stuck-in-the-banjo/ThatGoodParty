using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void NewGameBtn(string newGameLevel)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SpanishScene");
    }
    public void NewGameBtnEN(string newGameLevel)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SpanishSceneEN");
    }

    public void ExitgameBtn()
    {
        Application.Quit();
    }

    public void TwitterFran()
    {
        Application.OpenURL("https://twitter.com/botttos_");
    }
    public void TwitterEric()
    {
        Application.OpenURL("https://twitter.com/djcerdany");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void MainMenuENG(string newGameLevel)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuEN");
    }
}
