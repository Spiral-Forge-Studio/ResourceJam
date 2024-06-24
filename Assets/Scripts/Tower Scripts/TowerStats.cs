using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TowerStats", menuName ="TowerStats")]
public class TowerStats : ScriptableObject
{
    [Header("References")]
    public PowerNodeStats powerNodeStats;
    public ResourceStats resourceStats;

    [Header("HQ transform")]
    [SerializeField] public Transform hqTransform;

    [Header("Auto Cannon")]
    [SerializeField] private float _autoCannonPrice;
    [SerializeField] private float _autoCannonUpgrade;
    [SerializeField] private float _autoCannonUpkeep;
    [SerializeField] private float _autoCannonRange;
    [SerializeField] private float _autoCannonBaseFireRate;

    [Header("Tesla Coil")]
    [SerializeField] private float _teslaCoilPrice;
    [SerializeField] private float _teslaCoilUpgrade;
    [SerializeField] private float _teslaCoilUpkeep;
    [SerializeField] private float _teslaCoilRange;
    [SerializeField] private float _teslaCoilBaseFireRate;

    [Header("SAM")]
    [SerializeField] private float _SAMPrice;
    [SerializeField] private float _SAMUpgrade;
    [SerializeField] private float _SAMUpkeep;
    [SerializeField] private float _SAMRange;
    [SerializeField] private float _SAMBaseFireRate;

    [Header("Earthquake Tower")]
    [SerializeField] private float _EarthquakeTowerPrice;
    [SerializeField] private float _EarthquakeTowerUpgrade;
    [SerializeField] private float _EarthquakeTowerUpkeep;
    [SerializeField] private float _EarthquakeTowerRange;
    [SerializeField] private float _EarthquakeTowerBaseFireRate;
    [SerializeField] private float _EarthquakeTowerDamage;

    [Header("Common Attributes")]
    [SerializeField] private float _fireRateMultiplier;

    [Header("Upkeep Mechanics")]
    [SerializeField] private float _fireratePenaltyPercentCap;
    [SerializeField] private int _fireratePenaltyIntervals;

    [Header("[DEBUG] Other upkeep mechanic variables")]
    [SerializeField] private bool _fireratePenaltyIsOvercapped;
    [SerializeField] private float _fireratePenaltyPercentPerInterval;
    [SerializeField] private float _fireratePercentPenalty;

    [Header("[DEBUG] Tower List")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();


    public void AddTower(GameObject tower)
    {
        TowerParent _towerToAdd = tower.GetComponent<TowerParent>();

        if (resourceStats.SpendResources(_towerToAdd._price))
        {
            powerNodeStats.SpendUpkeep(_towerToAdd._upkeep);
            _towers.Add(tower);
        }
        else
        {
            Destroy(tower);
        }
    }

    public void SellTower(GameObject tower)
    {
        TowerParent _towerToRemove = tower.GetComponent<TowerParent>();

        resourceStats.SpendResources(_towerToRemove._price);
        powerNodeStats.GainUpkeep(_towerToRemove._upkeep);

        Destroy(tower);
    }


    public void SetAutoCannon(Ballista ballista)
    {
        ballista._price = _autoCannonPrice;
        ballista._upgrade = _autoCannonUpgrade;
        ballista._upkeep = _autoCannonUpkeep;
        ballista._range = _autoCannonRange;
        ballista._baseFireRate = _autoCannonBaseFireRate;
        ballista._fireRateMultiplier = _fireRateMultiplier;
    }

    public void SetTeslaCoil(Tesla_Coil teslaCoil)
    {
        teslaCoil._price = _teslaCoilPrice;
        teslaCoil._upgrade = _teslaCoilUpgrade;
        teslaCoil._upkeep = _teslaCoilUpkeep;
        teslaCoil._range = _teslaCoilRange;
        teslaCoil._baseFireRate = _teslaCoilBaseFireRate;
        teslaCoil._fireRateMultiplier = _fireRateMultiplier;
    }

    public void SetSAM(SAM sam)
    {
        sam._price = _SAMPrice;
        sam._upgrade = _SAMUpgrade;
        sam._upkeep = _SAMUpkeep;
        sam._range = _SAMRange;
        sam._baseFireRate = _SAMBaseFireRate;
        sam._fireRateMultiplier = _fireRateMultiplier;
    }

    public void SetEarthquakeTower(Earthquake_Tower earthquakeTower)
    {
        earthquakeTower._price = _EarthquakeTowerPrice;
        earthquakeTower._upgrade = _EarthquakeTowerUpgrade;
        earthquakeTower._upkeep = _EarthquakeTowerUpkeep;
        earthquakeTower._range = _EarthquakeTowerRange;
        earthquakeTower._baseFireRate = _EarthquakeTowerBaseFireRate;
        earthquakeTower.damagePoint = _EarthquakeTowerDamage;
        earthquakeTower._fireRateMultiplier = _fireRateMultiplier;
    }

    public Transform GetHQTransform()
    {
        return hqTransform; 
    }

    public List<GameObject> GetTowersList()
    {
        return _towers;
    }    
    public void SetTowersList(List<GameObject> _towerList)
    {
        _towers = _towerList;
    }

    public float GetPenaltyMultiplier()
    {
        _fireratePenaltyPercentPerInterval = (_fireratePenaltyPercentCap / _fireratePenaltyIntervals)/100;

        float scalingFactor = 100 / _fireratePenaltyPercentCap;

        float overcappedUpkeepPercent = powerNodeStats.GetUpkeepOvercapPercent()/100;

        for (int i = 1; i < _fireratePenaltyIntervals + 1; i++)
        {
            float resultingPenaltyPercent = 0;

            resultingPenaltyPercent = _fireratePenaltyPercentPerInterval * i * scalingFactor;

            if (overcappedUpkeepPercent <= resultingPenaltyPercent)
            {
                //Debug.Log("resulting penalty: " + resultingPenaltyPercent);
                return resultingPenaltyPercent;
            }
        }

        //Debug.Log("resulting penalty: 1");
        return 1f;
    }
}
