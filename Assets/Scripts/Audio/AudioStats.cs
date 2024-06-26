using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioStats", menuName = "AudioStats")]
public class AudioStats : ScriptableObject
{
    [Header("UI Sounds")]
    [SerializeField] public int menuButton;
    [SerializeField] public int pauseGameButton;
    [SerializeField] public int resumeFromPauseButton;
    [SerializeField] public int settingsButton;
    [SerializeField] public int returnToMainMenuButton;
    [SerializeField] public int quitButton;
    [SerializeField] public int startWaveButton;
    

    [Header("Placement Interaction Sounds")]
    [SerializeField] public int dragTowerSFX;
    [SerializeField] public int placeTowerSFX;
    [SerializeField] public int selectPlacedTowerSFX;
}
