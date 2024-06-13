using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceStats", menuName = "ResourceStats")]
public class ResourceStats : ScriptableObject
{
    public ResourceNodeScript[] nodes;
    public float totalRPM;
    public int totalResources;

    public void updateNodeMultipliers(float multiplier)
    {
        foreach (ResourceNodeScript node in nodes)
        {
            node.increaseMultiplier(multiplier);
        }
    }    
    public void updateNodeAdditionals(float additional)
    {
        foreach (ResourceNodeScript node in nodes)
        {
            node.increaseMultiplier(additional);
        }
    }

}
