using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameState gameState;
    public void ResumeGame()
    {
        gameState.SetPaused(false);
    }

    public void ReturnToMainMenu()
    {
        SceneController.instance.LoadScene("Main Menu");
    }

    public void PauseMenuSettings()
    {
       
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
