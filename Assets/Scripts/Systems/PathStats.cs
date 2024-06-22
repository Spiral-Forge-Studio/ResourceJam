using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PathStats", menuName = "PathStats")]
public class PathStats : ScriptableObject
{
    [SerializeField] private Path[] groundPaths;
    [SerializeField] private Path[] flyingPaths;

    public void StoreGroundPaths(Path[] _paths)
    {
        groundPaths = _paths;
    }

    public void StoreFlyingPaths(Path[] _paths)
    {
        flyingPaths = _paths;
    }

    public Path[] GetGroundPaths()
    {
        return groundPaths;
    }

    public Path[] GetFlyingPaths()
    {
        return flyingPaths;
    }

    public int GetNumberOfGroundPaths() => groundPaths.Length;
    public int GetNumberOfFlyingPaths() => flyingPaths.Length;

    public Path GetGroundPath(int pathNumber)
    {
        if (pathNumber < 0 || pathNumber > groundPaths.Length)
        {
            //Debug.Log("Invalid path number. Should be between 0 and " + groundPaths.Length);
            return null;
        }
        return groundPaths[pathNumber];
    }

    public Path GetFlyingPath(int pathNumber)
    {
        if (pathNumber < 0 || pathNumber > flyingPaths.Length)
        {
            //Debug.Log("Invalid path number. Should be between 0 and " + flyingPaths.Length);
            return null;
        }
        return flyingPaths[pathNumber];
    }

}
