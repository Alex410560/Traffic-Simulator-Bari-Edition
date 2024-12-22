using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    bool gamePaused = false;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] CameraManager c;
    [SerializeField] TimeScaler scaler;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Time.timeScale = 1;
                c.enabled = true;
                scaler.enabled = true;
                Cursor.visible = false;
                gamePaused = false;
                pauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                c.enabled = false;
                scaler.enabled = false;
                Cursor.visible = true;
                gamePaused = true;
                pauseMenu.SetActive(true);
            }
        }
    }

    public void StartMenu()
    {
        Time.timeScale = 1;
        c.enabled = true;
        scaler.enabled = true;
        gamePaused = false;
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        c.enabled = true;
        scaler.enabled = true;
        Cursor.visible = false;
        gamePaused = false;
        pauseMenu.SetActive(false);
    }
}
