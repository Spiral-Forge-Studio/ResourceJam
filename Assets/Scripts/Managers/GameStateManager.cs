using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [Header("[REFERENCES]")]
    public PauseMenu pauseMenu;
    public GameState gameState;

    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 1;
        gameState.hqDead = false;
        pauseMenu.gameObject.SetActive(false);
        gameState.SetPaused(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPauseStatus();
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
}
