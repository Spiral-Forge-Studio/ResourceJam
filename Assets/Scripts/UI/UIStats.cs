using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIStats", menuName ="UIStats")]
public class UIStats : ScriptableObject
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public bool pause;
    [SerializeField] public bool settings;
    [SerializeField] public bool resumegame;

    public void ActivatePauseMenu()
    {
        pause = true;
    }
    public void DeactivatePauseMenu()
    {
        pause = false;
    }

    public void ActivateSettingsMenu()
    {
        pause = false;
        settings = true;
    }
    public void DeactivateSettingsMenu()
    {
        pause = true;
        settings = false;
    }

}
