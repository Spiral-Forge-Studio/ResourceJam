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
        gameState.HqDead = false;
        gameState.EndLevel = false;
        gameState.BuildPhase = true;
        gameState.SetPaused(false);

        pauseMenu.gameObject.SetActive(false);
        endLevelMenu.gameObject.SetActive(false);
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
            
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void CheckIfEndLevel()
    {
        if (gameState.EndLevel == true)
        {
            Time.timeScale = 0;
            endLevelMenu.gameObject.SetActive(true);
        }
    }
}
