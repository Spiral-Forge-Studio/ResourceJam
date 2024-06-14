using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceStats", menuName = "ResourceStats")]
public class ResourceStats : ScriptableObject
{
    public List<GameObject> nodes;
    public float totalRPM;
    public float totalResources;
}
