using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameState", menuName = "GameState")]
public class GameState : ScriptableObject
{
    [SerializeField] private bool _isPaused;

    [SerializeField] private bool _hqDead { get; set; }

    [SerializeField] private bool _endLevel { get; set; }

    [SerializeField] private bool _buildPhase { get; set; }

    public bool HqDead
    {
        get { return _hqDead; }
        set { _hqDead = value; }
    }

    public bool EndLevel
    {
        get { return _endLevel; }
        set { _endLevel = value; }
    }

    public bool BuildPhase
    {
        get { return _buildPhase; }
        set { _buildPhase = value; }
    }

    public bool IsPaused() => _isPaused;
    public void SetPaused(bool isPaused) => _isPaused = isPaused;

}
