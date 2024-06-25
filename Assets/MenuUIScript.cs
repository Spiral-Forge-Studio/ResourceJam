using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIScript : MonoBehaviour
{
    public UIStats UIStats;

    public GameObject pauseMenu;
    public GameObject settingsMenu;

    // Start is called before the first frame update
    private void Awake()
    {
        UIStats.resumegame = false;
        UIStats.pause = false;
        UIStats.settings = false;
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
}
