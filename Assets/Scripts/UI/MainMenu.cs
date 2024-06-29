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
    }

    public void Back()
    {
        AudioManager.instance.PlayButtonSFX(1);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlayButtonSFX(3);
        Application.Quit();
    }

    public void StartLevel(int level)
    {
        AudioManager.instance.PlayButtonSFX(4);

        if (level == 1)
        {
            SceneController.instance.LoadScene("Level 1");
        }
        else if (level == 2)
        {
            SceneController.instance.LoadScene("Level 2");
        }
        else if (level == 3)
        {
            SceneController.instance.LoadScene("Level 3");
        }

    }

}
