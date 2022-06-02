using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Start()
    {
        //pauseMenuUI = gameObject.transform.GetChild(0).gameObject;
        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (GameStateManager.state == GameStateManager.GAMESTATE.PAUSE)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
    }

    public void RestartGame()
    {
        ResumeGame();
        Time.timeScale = 1;
        GameStateManager.NewGame();
    }

    public void ExitToMain()
    {
        ResumeGame();
        Time.timeScale = 1;
        GameStateManager.MainMenu();
    }
}
