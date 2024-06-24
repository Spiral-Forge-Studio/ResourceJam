using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TowerParent : MonoBehaviour
{
    [Header("Tower Attributes")]
    [SerializeField] public float _cost;
    [SerializeField] public float _upkeepCost;

    [SerializeField] public float _range;
    [SerializeField] public float _baseFireRate;
    [SerializeField] public float _fireRateMultiplier;
    [SerializeField] public float _baseDamage;
    [SerializeField] public float _damage;


    [Header("Upgrade Parameters")]
    [SerializeField] public int _maxUpgradeLevel;
    [SerializeField] public int _upgradeLevel;
    [SerializeField] public float[] _upgradeResourceCost;
    [SerializeField] public float _upgradePowerCostPercent;
    [SerializeField] public float[] _damageIncreasePercentPerLvl;

    [Header("[DEBUG] Attributes")]
    [SerializeField] public float _fireRate;
    [SerializeField] public bool _powerActive;
    [SerializeField] public float _modifiedUpkeep;
    [SerializeField] public float _additionalUpkeep;

    [Header("[REFERENCES]")]
    [SerializeField] public TowerStats towerStats;
    [SerializeField] public PowerNodeStats powerNodeStats;
    [SerializeField] private GameState gameState;
    [SerializeField] private Button _upgradeButton;

    [Header("[DEBUG]")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();
    

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _upgradeLevel = -1;
        _fireRateMultiplier = 1;
        towerStats.SetTowersList(_towers);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateTowerList();
    }

    public void UpdateTowerList()
    {
        _towers = towerStats.GetTowersList();
    }

    public void UpdateUpgradeRadialButtonState()
    {
    
        if (_upgradeLevel+1 > _maxUpgradeLevel)
        {
            _upgradeButton.interactable = false;
            _upgradeButton.enabled = false;
            return;
        }

        if (towerStats.CanSpendMoneyForUpgrade(_upgradeResourceCost[_upgradeLevel+1]))
        {
            _upgradeButton.interactable = true;
            _upgradeButton.enabled = true;
        }
        else
        {
            _upgradeButton.interactable = false;
            _upgradeButton.enabled = false;
        }
    }

    public void UpgradeTower()
    {
        _upgradeLevel++;
        powerNodeStats.GainUpkeep(_additionalUpkeep);

        _additionalUpkeep = (_upkeepCost * (_upgradeLevel+1) * (_upgradePowerCostPercent / 100));
        _modifiedUpkeep =  _upkeepCost + _additionalUpkeep;
        powerNodeStats.SpendUpkeep(_additionalUpkeep);

        towerStats.SpendMoneyForUpgrade(_upgradeResourceCost[_upgradeLevel]);
    }

    public void SellTower()
    {
        towerStats.SellTower(gameObject);
    }

    public void TogglePowerOn()
    {
        //_togglePowerOffButton.enabled = false;
        _powerActive = true;
        _baseFireRate = 1;
        powerNodeStats.SpendUpkeep(_modifiedUpkeep);
    }
    public void TogglePowerOff()
    {
        //_togglePowerOnButton.gameObject.SetActive();
        _powerActive = false;
        _baseFireRate = 0;
        powerNodeStats.GainUpkeep(_modifiedUpkeep);
    }

    public void UpdateDamage()
    {
        if (_upgradeLevel == -1)
        {
            _damage = _baseDamage;
        }
        else
        {
            _damage = (_baseDamage) + (_baseDamage * (_damageIncreasePercentPerLvl[_upgradeLevel] / 100));
        }
    }

    public void UpdateFirerate()
    {
        if (powerNodeStats.GetMaxUpkeep() <= 0)
        {
            //Debug.Log("0 upkeep");
            _fireRate = 0;
        }
        else if (powerNodeStats.IsOverCapped())
        {
            _fireRate = (_baseFireRate * _fireRateMultiplier) - (_baseFireRate * towerStats.GetPenaltyMultiplier());
            //Debug.Log("firing at -" + (100 * towerStats.GetPenaltyMultiplier()) + "%");
            //Debug.Log("base fire rate: " + _baseFireRate);
            //Debug.Log("fire rate: " + _fireRate);
            //Debug.Log("fire rate mult: " + _fireRateMultiplier);

        }
        else
        {
            //Debug.Log("not overcapped base fire rate: " + _baseFireRate);
            //Debug.Log("not overcapped fire rate: " + _fireRate);
            _fireRate = _baseFireRate * _fireRateMultiplier;
        }
    }

}
