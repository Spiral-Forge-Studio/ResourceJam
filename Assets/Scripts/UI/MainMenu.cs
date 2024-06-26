using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayMusic(0);
    }

    public void StartGame()
    {
        AudioManager.instance.PlayButtonSFX(4);
        SceneController.instance.NextScene();
    }

    public void QuitGame()
    {
        AudioManager.instance.PlayButtonSFX(3);
        Application.Quit();
    }

}
