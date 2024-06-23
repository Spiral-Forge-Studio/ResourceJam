using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [Header("[REFERENCES]")]
    public PauseMenu pauseMenu;
    public EndLevelMenu endLevelMenu;
    public GameState gameState;

    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 1;
        gameState.hqDead = false;
        gameState.endLevel = false;
        pauseMenu.gameObject.SetActive(false);
        endLevelMenu.gameObject.SetActive(false);
        gameState.SetPaused(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPauseStatus();
        CheckIfEndLevel();
    }

    private void CheckPauseStatus()
    {
        if (gameState.IsPaused() == true)
        {
            Time.timeScale = 0;
            pauseMenu.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.gameObject.SetActive(false);
        }
    }

    private void CheckIfEndLevel()
    {
        if (gameState.endLevel == true)
        {
            Time.timeScale = 0;
            endLevelMenu.gameObject.SetActive(true);
        }
    }
}
