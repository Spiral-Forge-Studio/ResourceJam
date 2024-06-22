using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TowerStats", menuName ="TowerStats")]
public class TowerStats : ScriptableObject
{
    [Header("Auto Cannon")]
    [SerializeField] private float _autoCannonPrice;
    [SerializeField] private float _autoCannonUpgrade;
    [SerializeField] private float _autoCannonUpkeep;
    [SerializeField] private float _autoCannonDamage;
    [SerializeField] private float _autoCannonRange;
    [SerializeField] private float _autoCannonFireRate;

    [Header("Tesla Coil")]
    [SerializeField] private float _teslaCoilPrice;
    [SerializeField] private float _teslaCoilUpgrade;
    [SerializeField] private float _teslaCoilUpkeep;
    [SerializeField] private float _teslaCoilDamage;

    [Header("SAM")]
    [SerializeField] private float _SAMPrice;
    [SerializeField] private float _SAMUpgrade;
    [SerializeField] private float _SAMUpkeep;
    [SerializeField] private float _SAMDamage;
    [SerializeField] private float _SAMAOE;

    [Header("Earthquake Tower")]
    [SerializeField] private float _EarthquakeTowerPrice;
    [SerializeField] private float _EarthquakeTowerUpgrade;
    [SerializeField] private float _EarthquakeTowerUpkeep;
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
        tower.GetComponent<TowerParent>();
        _towers.Add(tower);
    }

    public void SetAutoCannon(Ballista ballista)
    {
        ballista._price = _autoCannonPrice;
        ballista._upgrade = _autoCannonUpgrade;
        ballista._upkeep = _autoCannonUpkeep;
        ballista._range = _autoCannonRange;
        ballista._fireRate = _autoCannonFireRate;
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
