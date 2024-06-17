using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Path
{
    [SerializeField] public float pathNumber;
    [SerializeField] public List<Transform> pointList;
}


public class LevelManager : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] public Path[] _paths;

    [Header("[REFERNCES]")]
    [SerializeField] public PathStats pathStats;

    void Awake()
    {
        pathStats.StorePaths(_paths);
    }
}
