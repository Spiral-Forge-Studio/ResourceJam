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
    [SerializeField] private float _autoCannonBaseDamage;
    [SerializeField] private float _autoCannonBulletSpeed;
    [SerializeField] public float _autoCannonDamageIncreaseLvl1;
    [SerializeField] public float _autoCannonDamageIncreaseLvl2;
    [SerializeField] public float _autoCannonDamageIncreaseLvl3;

    [Header("Tesla Coil")]
    [SerializeField] private float _teslaCoilPrice;
    [SerializeField] private float _teslaCoilUpgrade;
    [SerializeField] private float _teslaCoilUpkeep;
    [SerializeField] private float _teslaCoilRange;
    [SerializeField] private float _teslaCoilBaseFireRate;
    [SerializeField] private float _teslaCoilBaseDamage;
    [SerializeField] public int _teslaCoilAmountToChain;
    [SerializeField] public float _teslaCoilDamageIncreaseLvl1;
    [SerializeField] public float _teslaCoilDamageIncreaseLvl2;
    [SerializeField] public float _teslaCoilDamageIncreaseLvl3;

    [Header("SAM")]
    [SerializeField] private float _SAMPrice;
    [SerializeField] private float _SAMUpgrade;
    [SerializeField] private float _SAMUpkeep;
    [SerializeField] private float _SAMRange;
    [SerializeField] private float _SAMBaseFireRate;
    [SerializeField] private float _SAMBaseDamage;
    [SerializeField] private float _SAMAreaOfEffect;
    [SerializeField] private float _SAMMissileLifespan;
    [SerializeField] public float _SAMDamageIncreaseLvl1;
    [SerializeField] public float _SAMDamageIncreaseLvl2;
    [SerializeField] public float _SAMDamageIncreaseLvl3;

    [Header("SAM Missile Speed Controls")]
    [SerializeField] private float _initialSpeed;
    [SerializeField] private float _slowSpeed;
    [SerializeField] private float _finalSpeed;
    [SerializeField] private float _initialBurstDuration;
    [SerializeField] private float _slowDuration;
    [SerializeField] private float _accelerationDuration;

    [Header("SAM Missile Size Control")]
    [SerializeField] private Vector3 _initialScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 _finalScale = new Vector3(2f, 2f, 2f);

    [Header("Earthquake Tower")]
    [SerializeField] private float _EarthquakeTowerPrice;
    [SerializeField] private float _EarthquakeTowerUpgrade;
    [SerializeField] private float _EarthquakeTowerUpkeep;
    [SerializeField] private float _EarthquakeTowerRange;
    [SerializeField] private float _EarthquakeTowerBaseFireRate;
    [SerializeField] private float _EarthquakeTowerBaseDamage;
    [SerializeField] private int _EarthquakeTowerDamageIncreaseLvl1;
    [SerializeField] private int _EarthquakeTowerDamageIncreaseLvl2;
    [SerializeField] private int _EarthquakeTowerDamageIncreaseLvl3;

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
        ballista._baseDamage = _autoCannonBaseDamage;
        ballista._baseFireRate = _autoCannonBaseFireRate;
        ballista._fireRateMultiplier = _fireRateMultiplier;
        ballista._damageIncreaseLvl1 = _autoCannonDamageIncreaseLvl1;
        ballista._damageIncreaseLvl2 = _autoCannonDamageIncreaseLvl2;
        ballista._damageIncreaseLvl3 = _autoCannonDamageIncreaseLvl3;
    }

    public void SetTeslaCoil(Tesla_Coil teslaCoil)
    {
        teslaCoil._price = _teslaCoilPrice;
        teslaCoil._upgrade = _teslaCoilUpgrade;
        teslaCoil._upkeep = _teslaCoilUpkeep;
        teslaCoil._range = _teslaCoilRange;
        teslaCoil._baseDamage = _teslaCoilBaseDamage;
        teslaCoil._baseFireRate = _teslaCoilBaseFireRate;
        teslaCoil._fireRateMultiplier = _fireRateMultiplier;
        teslaCoil._damageIncreaseLvl1 = _teslaCoilDamageIncreaseLvl1;
        teslaCoil._damageIncreaseLvl2 = _teslaCoilDamageIncreaseLvl2;
        teslaCoil._damageIncreaseLvl3 = _teslaCoilDamageIncreaseLvl3;
    }

    public void SetSAM(SAM sam)
    {
        sam._price = _SAMPrice;
        sam._upgrade = _SAMUpgrade;
        sam._upkeep = _SAMUpkeep;
        sam._range = _SAMRange;
        sam._baseDamage = _SAMBaseDamage;
        sam._baseFireRate = _SAMBaseFireRate;
        sam._fireRateMultiplier = _fireRateMultiplier;
        sam._damageIncreaseLvl1 = _SAMDamageIncreaseLvl1;
        sam._damageIncreaseLvl2 = _SAMDamageIncreaseLvl2;
        sam._damageIncreaseLvl3 = _SAMDamageIncreaseLvl3;
    }

    public void SetEarthquakeTower(Earthquake_Tower earthquakeTower)
    {
        earthquakeTower._price = _EarthquakeTowerPrice;
        earthquakeTower._upgrade = _EarthquakeTowerUpgrade;
        earthquakeTower._upkeep = _EarthquakeTowerUpkeep;
        earthquakeTower._range = _EarthquakeTowerRange;
        earthquakeTower._baseFireRate = _EarthquakeTowerBaseFireRate;
        earthquakeTower._baseDamage = _EarthquakeTowerBaseDamage;
        earthquakeTower._fireRateMultiplier = _fireRateMultiplier;
        earthquakeTower._damageIncreaseLvl1 = _EarthquakeTowerDamageIncreaseLvl1;
        earthquakeTower._damageIncreaseLvl2 = _EarthquakeTowerDamageIncreaseLvl2;
        earthquakeTower._damageIncreaseLvl3 = _EarthquakeTowerDamageIncreaseLvl3;
    }


    public void SetAutoCannonBulletStats(Ammo autoCannonAmmo, Ballista autoCannon)
    {
        autoCannonAmmo._damage = autoCannon._damage;
        autoCannonAmmo._bulletSpeed = _autoCannonBulletSpeed;
    }

    public void SetTeslaBulletStats(ChainLightningScript teslaAmmo, Tesla_Coil tesla)
    {
        teslaAmmo._damage = tesla._damage;
        teslaAmmo.amountToChain = _teslaCoilAmountToChain;
    }

    public void SetSAMMissileStats(Sam_Missile samMissile)
    {
        samMissile.areaOfEffect = _SAMAreaOfEffect;
        samMissile.initialSpeed = _initialSpeed;
        samMissile.initialBurstDuration = _initialBurstDuration;
        samMissile.slowSpeed = _slowSpeed;
        samMissile.slowDuration = _slowDuration;
        samMissile.finalSpeed = _finalSpeed;
        samMissile.accelerationDuration = _accelerationDuration;
        samMissile.lifeSpan = _SAMMissileLifespan;
        samMissile.initialScale = _initialScale;
        samMissile.finalScale = _finalScale;
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
