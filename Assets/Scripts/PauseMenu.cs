using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    public AudioSource audioSource;
    public Canvas pauseCanvas;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioSource.Play();
            if (pauseCanvas.isActiveAndEnabled)
            {
                Time.timeScale = 1;
                pauseCanvas.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                pauseCanvas.gameObject.SetActive(true);
            }

        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseCanvas.gameObject.SetActive(false);
    }
}
