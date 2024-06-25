using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameState gameState;
    public UIStats UIStats;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        gameState.SetPaused(false);
        gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneController.instance.LoadScene("Main Menu");
    }

    public void PauseMenuSettings()
    {
        gameState.SetPaused(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
