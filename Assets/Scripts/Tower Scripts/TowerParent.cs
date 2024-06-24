using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerParent : MonoBehaviour
{
    [Header("Tower Attributes")]
    [SerializeField] public float _price;
    [SerializeField] public float _upkeep;
    [SerializeField] public float _upgrade;
    [SerializeField] public float _range;
    [SerializeField] public float _baseFireRate;
    [SerializeField] public float _fireRateMultiplier;
    [SerializeField] public float _baseDamage;
    [SerializeField] public float _damage;


    [Header("Upgrade Parameters")]
    [Range(0, 2)]
    [SerializeField] public int _upgradeLevel;
    [SerializeField] public float _damageIncreaseLvl1;
    [SerializeField] public float _damageIncreaseLvl2;
    [SerializeField] public float _damageIncreaseLvl3;

    [Header("[DEBUG] Attributes")]
    [SerializeField] public float _fireRate;

    [Header("[REFERENCES]")]
    [SerializeField] public TowerStats towerStats;
    [SerializeField] public PowerNodeStats powerNodeStats;
    [SerializeField] private GameState gameState;

    [Header("[DEBUG]")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();
    

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _upgradeLevel = 0;
        _fireRateMultiplier = 1;
        towerStats.SetTowersList(_towers);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateTowerList();
        UpdateDamage();
        UpdateFirerate();
    }

    public void UpdateTowerList()
    {
        _towers = towerStats.GetTowersList();
    }

    public void UpgradeTower()
    {
        if (_upgradeLevel > 3)
        {
            return;
        }
        else
        {
            _upgradeLevel++;
        }
    }

    public void UpdateDamage()
    {
        if (_upgradeLevel == 0)
        {
            _damage = _baseDamage;
        }
        else if (_upgradeLevel == 1)
        {
            _damage = (_baseDamage) + (_baseDamage * (_damageIncreaseLvl1/100));
        }
        else if (_upgradeLevel == 2)
        {
            _damage = (_baseDamage) + (_baseDamage * (_damageIncreaseLvl2 / 100));
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
