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
    [SerializeField] private float _autoCannonCost;
    [SerializeField] private float[] _autoCannonUpgradeResourceCost;
    [SerializeField] private float _autoCannonUpgradePowerCostPercent;
    [SerializeField] private float _autoCannonUpkeep;
    [SerializeField] private float _autoCannonRange;
    [SerializeField] private float _autoCannonBaseFireRate;
    [SerializeField] private float _autoCannonBaseDamage;
    [SerializeField] private float _autoCannonBulletSpeed;
    [SerializeField] public float[] _autoCannonDamageIncreasePercentPerLvl;
    [SerializeField] public float _autoCannonRotationSpeed;

    [Header("Tesla Coil")]
    [SerializeField] private float _teslaCoilCost;
    [SerializeField] private float[] _teslaCoilUpgradeResourceCost;
    [SerializeField] private float _teslaCoilUpgradePowerCostPercent;
    [SerializeField] private float _teslaCoilUpkeep;
    [SerializeField] private float _teslaCoilRange;
    [SerializeField] private float _teslaCoilBaseFireRate;
    [SerializeField] private float _teslaCoilBaseDamage;
    [SerializeField] public int _teslaCoilAmountToChain;
    [SerializeField] public float[] _teslaCoilDamageIncreasePercentPerLvl;

    [Header("SAM")]
    [SerializeField] private float _SAMCost;
    [SerializeField] private float[] _SAMUpgradeResourceCost;
    [SerializeField] private float _SAMUpgradePowerCostPercent;
    [SerializeField] private float _SAMUpkeep;
    [SerializeField] private float _SAMRange;
    [SerializeField] private float _SAMBaseFireRate;
    [SerializeField] private float _SAMBaseDamage;
    [SerializeField] private float _SAMAreaOfEffect;
    [SerializeField] private float _SAMMissileLifespan;
    [SerializeField] public float[] _SAMDamageIncreasePercentPerLvl;
    [SerializeField] public float _SAMRotationSpeed;

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
    [SerializeField] private float _EarthquakeTowerCost;
    [SerializeField] private float[] _EarthquakeTowerUpgradeResourceCost;
    [SerializeField] private float _EarthquakeTowerUpgradePowerCostPercent;
    [SerializeField] private float _EarthquakeTowerUpkeep;
    [SerializeField] private float _EarthquakeTowerRange;
    [SerializeField] private float _EarthquakeTowerBaseFireRate;
    [SerializeField] private float _EarthquakeTowerBaseDamage;
    [SerializeField] private float[] _EarthquakeTowerDamageIncreasePercentPerLvl;

    [Header("Common Attributes")]
    [SerializeField] private float _fireRateMultiplier;
    [SerializeField] private int _maxUpgradeLevel;
    [SerializeField] private int _sellRefundPercentage;

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

        if (resourceStats.SpendResources(_towerToAdd._cost))
        {
            powerNodeStats.SpendUpkeep(_towerToAdd._upkeepCost);
            _towers.Add(tower);
        }
        else
        {
            Destroy(tower);
        }
    }

    public bool CanSpendMoneyForUpgrade(float amount)
    {
        if (resourceStats.GetTotalResources() - amount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpendMoneyForUpgrade(float amount)
    {
        resourceStats.SpendResources(amount);
    }

    public void SellTower(GameObject tower)
    {
        TowerParent _towerToRemove = tower.GetComponent<TowerParent>();

        float computedTowerRefundedResource;
        if (_towerToRemove._upgradeLevel >= 0)
        {
            computedTowerRefundedResource = _towerToRemove._cost +
                (_towerToRemove._upgradeResourceCost[_towerToRemove._upgradeLevel]
                * _sellRefundPercentage
                / 100);
        }
        else
        {
            computedTowerRefundedResource = _towerToRemove._cost;
        }


        resourceStats.AddToTotalResources(computedTowerRefundedResource);

        float totalTowerUpkeep = _towerToRemove._upkeepCost + (
            _towerToRemove._upkeepCost * 
            ((_towerToRemove._upgradeLevel + 1) * 
            (0.01f*_towerToRemove._upgradePowerCostPercent)));

        powerNodeStats.GainUpkeep((totalTowerUpkeep));

        Destroy(tower);
    }


    public void SetAutoCannon(Ballista ballista)
    {
        ballista._cost = _autoCannonCost;
        ballista._upgradeResourceCost = _autoCannonUpgradeResourceCost;
        ballista._upgradePowerCostPercent = _autoCannonUpgradePowerCostPercent;
        ballista._upkeepCost = _autoCannonUpkeep;
        ballista._range = _autoCannonRange;
        ballista._baseDamage = _autoCannonBaseDamage;
        ballista._baseFireRate = _autoCannonBaseFireRate;
        ballista._fireRateMultiplier = _fireRateMultiplier;
        ballista._damageIncreasePercentPerLvl = _autoCannonDamageIncreasePercentPerLvl;
        ballista._maxUpgradeLevel = _maxUpgradeLevel;
        ballista.rotationSpeed = _autoCannonRotationSpeed;
    }

    public void SetTeslaCoil(Tesla_Coil teslaCoil)
    {
        teslaCoil._cost = _teslaCoilCost;
        teslaCoil._upgradeResourceCost = _teslaCoilUpgradeResourceCost;
        teslaCoil._upgradePowerCostPercent = _teslaCoilUpgradePowerCostPercent;
        teslaCoil._upkeepCost = _teslaCoilUpkeep;
        teslaCoil._range = _teslaCoilRange;
        teslaCoil._baseDamage = _teslaCoilBaseDamage;
        teslaCoil._baseFireRate = _teslaCoilBaseFireRate;
        teslaCoil._fireRateMultiplier = _fireRateMultiplier;
        teslaCoil._damageIncreasePercentPerLvl = _teslaCoilDamageIncreasePercentPerLvl;
        teslaCoil._maxUpgradeLevel = _maxUpgradeLevel;
    }

    public void SetSAM(SAM sam)
    {
        sam._cost = _SAMCost;
        sam._upgradeResourceCost = _SAMUpgradeResourceCost;
        sam._upgradePowerCostPercent = _SAMUpgradePowerCostPercent;
        sam._upkeepCost = _SAMUpkeep;
        sam._range = _SAMRange;
        sam._baseDamage = _SAMBaseDamage;
        sam._baseFireRate = _SAMBaseFireRate;
        sam._fireRateMultiplier = _fireRateMultiplier;
        sam._damageIncreasePercentPerLvl = _SAMDamageIncreasePercentPerLvl;
        sam._maxUpgradeLevel = _maxUpgradeLevel;
        sam.rotationSpeed = _SAMRotationSpeed;
    }

    public void SetEarthquakeTower(Earthquake_Tower earthquakeTower)
    {
        earthquakeTower._cost = _EarthquakeTowerCost;
        earthquakeTower._upgradeResourceCost = _EarthquakeTowerUpgradeResourceCost;
        earthquakeTower._upgradePowerCostPercent = _EarthquakeTowerUpgradePowerCostPercent;
        earthquakeTower._upkeepCost = _EarthquakeTowerUpkeep;
        earthquakeTower._range = _EarthquakeTowerRange;
        earthquakeTower._baseFireRate = _EarthquakeTowerBaseFireRate;
        earthquakeTower._baseDamage = _EarthquakeTowerBaseDamage;
        earthquakeTower._fireRateMultiplier = _fireRateMultiplier;
        earthquakeTower._damageIncreasePercentPerLvl = _EarthquakeTowerDamageIncreasePercentPerLvl;
        earthquakeTower._maxUpgradeLevel = _maxUpgradeLevel;
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

        powerNodeStats._hardCapPenaltyMultiplier = 1 + (_fireratePenaltyPercentCap / 100);
        //Debug.Log("resulting penalty: 1");
        return 1f;
    }
}
