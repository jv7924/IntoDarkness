using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance;

    [SerializeField]
    private List<string> levels = new List<string>();
    [SerializeField]
    private string titleSceneName;

    enum GAMESTATE
    {
        MENU,
        PLAYING,
        PAUSE,
        GAMEOVER
    }

    private static GAMESTATE state;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public static void NewGame()
    {
        state = GAMESTATE.PLAYING;
        if (instance.levels.Count > 0)
        {
            SceneManager.LoadScene(instance.levels[0]);
        }
    }

    public static void GameOver()
    {
        state = GAMESTATE.GAMEOVER;
        SceneManager.LoadScene(instance.titleSceneName);
    }

    public static void TogglePause()
    {
        if (state == GAMESTATE.PLAYING)
        {
            state = GAMESTATE.PAUSE;
            Time.timeScale = 0;
        }
        else if (state == GAMESTATE.PAUSE)
        {
            state = GAMESTATE.PLAYING;
            Time.timeScale = 1;
        }
    }
}
