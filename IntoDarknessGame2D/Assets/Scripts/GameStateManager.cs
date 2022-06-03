using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance;
    private static AudioSource deathSound;

    [SerializeField]
    private List<string> levels = new List<string>();

    private int levelNum = 1;
    public int nextLevel = 0;

    [SerializeField]
    public string titleSceneName;

    public enum GAMESTATE
    {
        MENU,
        PLAYING,
        PAUSE,
        GAMEOVER
    }

    public static GAMESTATE state;
    private void Awake()
    {
        deathSound = transform.GetChild(0).GetComponent<AudioSource>();
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
            if (instance.nextLevel + 1 > instance.levels.Count - 1)
            {
                MainMenu();
            }
            else
            {
                SceneManager.LoadScene(instance.levels[instance.levelNum + instance.nextLevel]);
            }
        }
    }

    public static void NextLevel()
    {
        instance.nextLevel++;
    }

    public static void GameOver()
    {
        state = GAMESTATE.GAMEOVER;
        deathSound.Play();
        NewGame();
        //SceneManager.LoadScene(instance.levels[1]);
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

    public static void MainMenu()
    {
        state = GAMESTATE.MENU;
        instance.nextLevel = 0;
        SceneManager.LoadScene(instance.titleSceneName);
    }
}
