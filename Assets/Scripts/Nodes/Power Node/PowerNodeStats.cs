using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerNodeStats", menuName = "PowerNodeStats")]
public class PowerNodeStats : ScriptableObject
{
    [SerializeField] public List<GameObject> powerNodes = new List<GameObject>();

    [SerializeField] private float _upkeepEnergy;
    [SerializeField] private float _maxEnergy;

    [SerializeField] private float _totalHealth;
    [SerializeField] private float _totalMaxHealth;

    // use when placing towers
    public bool SpendUpkeep(float amount)
    {
        if (_upkeepEnergy + amount <= _maxEnergy)
        {
            _upkeepEnergy += amount;
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
        if (_upkeepEnergy - amount >= 0)
        {
            _upkeepEnergy -= amount;
            return true;
        }
        else
        {
            Debug.Log("Negative upkeep");
            return false;
        }
    }

    public float getPercentEnergy() => (_upkeepEnergy / _maxEnergy);


    #region GETTERS AND SETTERS
    public float GetUpkeep() => _upkeepEnergy;
    public float GetMaxUpkeep() => _maxEnergy;
    public float GetTotalHealth() => _totalHealth;
    public float GetTotalMaxHealth() => _totalMaxHealth;

    public float SetUpkeep(float upkeep) => _upkeepEnergy = upkeep;
    public float SetMaxUpkeep(float maxUpkeep) => _maxEnergy = maxUpkeep;
    public float SetTotalHealth(float totalHealth) => _totalHealth = totalHealth;
    public float SetTotalMaxHealth(float totalMaxHealth) => _totalMaxHealth = totalMaxHealth;

    #endregion
}
