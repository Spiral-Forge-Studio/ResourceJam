using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerNodeStats", menuName = "PowerNodeStats")]
public class PowerNodeStats : ScriptableObject
{
    [SerializeField] public PowerNodeScript[] powerNodes;

    [SerializeField] public float upkeepEnergy;
    [SerializeField] public float maxEnergy;

    [SerializeField] public float totalHealth;
    [SerializeField] public float totalMaxHealth;
    
    // use when placing towers
    public bool SpendUpkeep(float amount)
    {
        if (upkeepEnergy + amount <= maxEnergy)
        {
            upkeepEnergy += amount;
            return true;
        }
        else
        {
            Debug.Log("Max energy reached");
            return false;
        }
        
    }

    // use when selling or deactivating towers
    public bool GainUpkeep(float amount)
    {
        if (upkeepEnergy - amount >= 0)
        {
            upkeepEnergy -= amount;
            return true;
        }
        else
        {
            Debug.Log("Negative upkeep");
            return false;
        }
    }

    public float getPercentEnergy() => (upkeepEnergy / maxEnergy);
}
