using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelMenu : MonoBehaviour
{
    public GameState gameState;
    public void NextLevel()
    {
        SceneController.instance.NextScene();
    }

    public void ReturnToMainMenu()
    {
        SceneController.instance.LoadScene("Main Menu");
    }

    public void RestartLevel()
    {
        SceneController.instance.ReloadScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
