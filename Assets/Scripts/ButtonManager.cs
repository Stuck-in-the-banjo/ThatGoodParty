using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void NewGameBtn(string newGameLevel)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
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
}
