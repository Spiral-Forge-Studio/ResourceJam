using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Path
{
    [SerializeField] public float pathNumber;
    [SerializeField] public List<Transform> pointList;

    public int GetPathLength() => pointList.Count;

}


public class LevelManager : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] public Path[] _grounPaths;
    [SerializeField] public Path[] _flyingPaths;

    [Header("[REFERNCES]")]
    [SerializeField] public PathStats pathStats;

    void Awake()
    {
        pathStats.StoreGroundPaths(_grounPaths);
        pathStats.StoreFlyingPaths(_flyingPaths);
    }
}
