using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TowerStats", menuName ="TowerStats")]
public class TowerStats : ScriptableObject
{
    [Header("Auto Cannon")]
    //[SerializeField] private float _autoCannonPrice;
    //[SerializeField] private float _autoCannonUpgrade;
    //[SerializeField] private float _autoCannonUpkeep;
    //[SerializeField] public float _autoCannonDamage;
    //[SerializeField] public float _autoCannonRange;
    [SerializeField] public float _autoCannonRotation;
    //[SerializeField] public float _autoCannonBulletSpeed;
    //[SerializeField] public float _autoCannonFireRate;



    [Header("Tesla Coil")]
    //[SerializeField] private float _teslaCoilPrice;
    //[SerializeField] private float _teslaCoilUpgrade;
    //[SerializeField] private float _teslaCoilUpkeep;
    //[SerializeField] public float _teslaCoilDamage;
    [SerializeField] public int _teslaCoilAmountToChain;
    //[SerializeField] public float _teslaCoilRange;
    //[SerializeField] public float _teslaCoilFireRate;

    [Header("SAM")]
    //[SerializeField] public float _SAMPrice;
    //[SerializeField] public float _SAMUpgrade;
    //[SerializeField] public float _SAMUpkeep;
    //[SerializeField] public float _SAMDamage;
    //[SerializeField] public float _SAMRange;
    [SerializeField] public float _SAMAOE;
    //[SerializeField] public float _SAMFireRate;
    //[SerializeField] public float _SAMBulletSpeed;

    [Header("Earthquake Tower")]
    //[SerializeField] private float _EarthquakeTowerPrice;
    //[SerializeField] private float _EarthquakeTowerUpgrade;
    //[SerializeField] private float _EarthquakeTowerUpkeep;
    [SerializeField] private float _EarthquakeTowerDamage;
    [SerializeField] private float _EarthquakeTowerAOE;
    [SerializeField] private float _EarthquakeTowerRange;


    [Header("[DEBUG]")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();

    public void AddTower(GameObject tower)
    {
        _towers.Add(tower);
    }

    // price, upkeep
    //public float[] getAutoCannonAttributes()
    //{
    //    return new float[] { _autoCannonPrice, _autoCannonUpgrade, _autoCannonUpkeep, _autoCannonDamage };
    //}

    //// price, upkeep
    //public float[] getTeslaCoilAttributes()
    //{
    //    return new float[] { _teslaCoilPrice, _teslaCoilUpgrade, _teslaCoilUpkeep , _teslaCoilDamage};
    //}

    //// price, upkeep
    //public float[] getSAMAttributes()
    //{
    //    return new float[] { _SAMPrice, _SAMUpgrade, _SAMUpkeep , _SAMDamage};
    //}

    //// price, upkeep
    //public float[] getEarthquakeTowerAttributes()
    //{
    //    return new float[] { _EarthquakeTowerPrice, _EarthquakeTowerUpgrade, _EarthquakeTowerUpkeep , _EarthquakeTowerDamage};
    //}
}
