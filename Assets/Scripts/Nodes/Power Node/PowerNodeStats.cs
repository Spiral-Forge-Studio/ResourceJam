using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "PowerNodeStats", menuName = "PowerNodeStats")]
public class PowerNodeStats : ScriptableObject
{
    [SerializeField] public List<GameObject> powerNodes = new List<GameObject>();

    [SerializeField] private float _upkeepEnergy;
    [SerializeField] private float _maxEnergy;

    [SerializeField] private float _totalHealth;
    [SerializeField] private float _totalMaxHealth;

    [SerializeField] private bool _overCapped;
    [SerializeField] private bool _overHardCap;
    [SerializeField] public float _hardCapPenaltyMultiplier;


    // use when placing towers
    public void SpendUpkeep(float amount)
    {
        _upkeepEnergy += amount;
        Debug.Log("Spending upkeep of: " + amount);
        Debug.Log("total upkeep: " + _upkeepEnergy);
    }

    // use when selling or deactivating towers
    public void GainUpkeep(float amount)
    {
        _upkeepEnergy -= amount;

        if (_upkeepEnergy <= _maxEnergy)
        {
            Debug.Log("amount: " + amount);
            Debug.Log("gaining upkeep: " + _upkeepEnergy);
            SetOverCapped(false);
        }
        if (_upkeepEnergy < 0)
        {
            _upkeepEnergy = 0;
        }
    }

    public void CheckOverCapped()
    {
        if (_upkeepEnergy <= _maxEnergy)
        {
            SetOverHardCap(false);
            SetOverCapped(false);
        }
        else if (_upkeepEnergy > _maxEnergy * _hardCapPenaltyMultiplier)
        {
            SetOverHardCap(true);
            SetOverCapped(true);
        }
        else
        {
            SetOverHardCap(false);
            SetOverCapped(true);
        }
    }

    public bool IsOverCapped()
    {
        return _overCapped;
    }

    public void SetOverCapped(bool overCapped)
    {
        _overCapped = overCapped;
    }

    public bool IsOverHardCap()
    {
        return _overHardCap;
    }

    public void SetOverHardCap(bool overHardCap)
    {
        _overHardCap = overHardCap;
    }

    public float GetUpkeepOvercapPercent()
    {
        Assert.IsTrue(_overCapped, "Not overcapped, can't get overcapped upkeep!");
        return 100*((_upkeepEnergy - _maxEnergy)/_maxEnergy);
    }

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
