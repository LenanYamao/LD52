using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUi;
    public GameObject gm;
    public Button btnPause;
    public Button btnUpgrade;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
	}
    public void Quit()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }

}
