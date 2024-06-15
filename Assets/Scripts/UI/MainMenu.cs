using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneController.instance.NextScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
