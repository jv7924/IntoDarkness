using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject pausePanel;
    public void ResumeGame()
    {
        Debug.Log("AS");
        GameStateManager.TogglePause();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    void Start()
    {
        pausePanel = gameObject.transform.GetChild(0).gameObject;
        DontDestroyOnLoad(gameObject);
    }

    void Update(){
        if (GameStateManager.state == GameStateManager.GAMESTATE.PAUSE){
            pausePanel.SetActive(true);
        }else{
            pausePanel.SetActive(false);
        }
    }
}
