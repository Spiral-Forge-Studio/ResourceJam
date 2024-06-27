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
        AudioManager.instance.PlayButtonSFX(1);
        gameState.SetPaused(false);
        gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        AudioManager.instance.PlayButtonSFX(1);
        SceneController.instance.LoadScene("Main Menu");
    }

    public void PauseMenuSettings()
    {
        AudioManager.instance.PlayButtonSFX(1);
        //gameState.SetPaused(false);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlayButtonSFX(1);
        Application.Quit();
    }

    public void RestartLevel()
    {
        AudioManager.instance.PlayButtonSFX(1);
        SceneController.instance.ReloadScene();
    }

}
