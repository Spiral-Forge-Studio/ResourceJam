using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TowerStats", menuName ="TowerStats")]
public class TowerStats : ScriptableObject
{
    [Header("References")]
    public PowerNodeStats powerNodeStats;
    public ResourceStats resourceStats;

    [Header("Auto Cannon")]
    [SerializeField] private float _autoCannonPrice;
    [SerializeField] private float _autoCannonUpgrade;
    [SerializeField] private float _autoCannonUpkeep;
    [SerializeField] private float _autoCannonRange;
    [SerializeField] private float _autoCannonFireRate;

    [Header("Tesla Coil")]
    [SerializeField] private float _teslaCoilPrice;
    [SerializeField] private float _teslaCoilUpgrade;
    [SerializeField] private float _teslaCoilUpkeep;
    [SerializeField] private float _teslaCoilRange;
    [SerializeField] private float _teslaCoilFireRate;

    [Header("SAM")]
    [SerializeField] private float _SAMPrice;
    [SerializeField] private float _SAMUpgrade;
    [SerializeField] private float _SAMUpkeep;
    [SerializeField] private float _SAMRange;
    [SerializeField] private float _SAMFireRate;

    [Header("Earthquake Tower")]
    [SerializeField] private float _EarthquakeTowerPrice;
    [SerializeField] private float _EarthquakeTowerUpgrade;
    [SerializeField] private float _EarthquakeTowerUpkeep;
    [SerializeField] private float _EarthquakeTowerRange;
    [SerializeField] private float _EarthquakeTowerFireRate;
    [SerializeField] private float _EarthquakeTowerDamage;

    [Header("HQ transform")]
    [SerializeField] public Transform hqTransform;

    [Header("[DEBUG]")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();
    [SerializeField] private bool autoCannonInitialized;
    [SerializeField] private int autoCannonLevel;
    [SerializeField] private bool teslaCoilInitialized;
    [SerializeField] private bool SAMInitialized;
    [SerializeField] private bool EarthquakeInitialized;

    public void AddTower(GameObject tower)
    {
        TowerParent _towerToAdd = tower.GetComponent<TowerParent>();

        if (powerNodeStats.SpendUpkeep(_towerToAdd._upkeep) && resourceStats.SpendResources(_towerToAdd._price))
        {
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
        ballista._fireRate = _autoCannonFireRate;
    }

    public void SetTeslaCoil(Tesla_Coil teslaCoil)
    {
        teslaCoil._price = _teslaCoilPrice;
        teslaCoil._upgrade = _teslaCoilUpgrade;
        teslaCoil._upkeep = _teslaCoilUpkeep;
        teslaCoil._range = _teslaCoilRange;
        teslaCoil._fireRate = _teslaCoilFireRate;
    }

    public void SetSAM(SAM sam)
    {
        sam._price = _SAMPrice;
        sam._upgrade = _SAMUpgrade;
        sam._upkeep = _SAMUpkeep;
        sam._range = _SAMRange;
        sam._fireRate = _SAMFireRate;
    }

    public void SetEarthquakeTower(Earthquake_Tower earthquakeTower)
    {
        earthquakeTower._price = _EarthquakeTowerPrice;
        earthquakeTower._upgrade = _EarthquakeTowerUpgrade;
        earthquakeTower._upkeep = _EarthquakeTowerUpkeep;
        earthquakeTower._range = _EarthquakeTowerRange;
        earthquakeTower._fireRate = _EarthquakeTowerFireRate;
        earthquakeTower.damagePoint = _EarthquakeTowerDamage;
    }

    public void UpgradeAutoCannon()
    {
        autoCannonLevel++;
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
}
