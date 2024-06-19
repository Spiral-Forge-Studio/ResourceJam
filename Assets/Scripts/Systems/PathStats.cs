using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathStats", menuName = "PathStats")]
public class PathStats : ScriptableObject
{
    [SerializeField] private Path[] paths;

    public void StorePaths(Path[] _paths)
    {
        paths = _paths;
    }

    public Path[] GetPaths()
    {
        return paths;
    }

    public int GetNumberOfPaths() => paths.Length;

    public Path GetPath(int pathNumber)
    {
        if (pathNumber < 0 || pathNumber > paths.Length)
        {
            Debug.Log("Invalid path number. Should be between 0 and " + paths.Length);
            return null;
        }
        return paths[pathNumber];
    }

}
