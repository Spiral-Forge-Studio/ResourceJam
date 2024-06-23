using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameState", menuName = "GameState")]
public class GameState : ScriptableObject
{
    private bool _isPaused;

    public bool hqDead;

    public bool endLevel;

    public bool IsPaused() => _isPaused;
    public void SetPaused(bool isPaused) => _isPaused = isPaused;

}
