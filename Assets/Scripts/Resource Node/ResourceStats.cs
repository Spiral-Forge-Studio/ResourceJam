using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceStats", menuName = "ResourceStats")]
public class ResourceStats : ScriptableObject
{
    public List<GameObject> nodes;
    public float totalRPM;
    public float totalResources;

    //call this when buying towers
    public void spendResources(float amount)
    {
        if (totalResources - amount >= 0)
        {
            totalResources -= amount;
        }
        else
        {
            Debug.Log("Not enough money to spend");
        }
    }

    //call this when selling towers
    public void returnResources(float amount)
    {
        totalResources += amount;
    }
}
