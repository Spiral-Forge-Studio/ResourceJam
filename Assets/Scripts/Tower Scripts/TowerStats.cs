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


    [Header("[DEBUG]")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();

    public void AddTower(GameObject tower)
    {
        _towers.Add(tower);
    }

    // price, upkeep
    public float[] getAutoCannonAttributes()
    {
        return new float[] { _autoCannonPrice, _autoCannonUpgrade, _autoCannonUpkeep , _autoCannonDamage};
    }
    // price, upkeep
    public float[] getTeslaCoilAttributes()
    {
        return new float[] { _teslaCoilPrice, _teslaCoilUpgrade, _teslaCoilUpkeep , _teslaCoilDamage};
    }
    // price, upkeep
    public float[] getSAMAttributes()
    {
        return new float[] { _SAMPrice, _SAMUpgrade, _SAMUpkeep , _SAMDamage};
    }
    // price, upkeep
    public float[] getEarthquakeTowerAttributes()
    {
        return new float[] { _EarthquakeTowerPrice, _EarthquakeTowerUpgrade, _EarthquakeTowerUpkeep , _EarthquakeTowerDamage};
    }
}
