using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] public GameObject _canUpgradeButton;
    [SerializeField] public GameObject _cannotUpgradeButton;
    [SerializeField] public GameObject _powerUpButton;
    [SerializeField] public GameObject _powerDownButton;
    [SerializeField] private SpriteRenderer _towerSprite;
    [SerializeField] private TowerRangeIndicator _towerRangeIndicator;
    [SerializeField] private GameObject[] upgradeLevelSprites;

    [Header("[DEBUG]")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private bool _forcedPowerDown;



    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _forcedPowerDown = false;
        _powerActive = true;
        spriteRenderers.Clear();
        CollectSpriteRenderersRecursive(transform);
        _upgradeLevel = 0;
        _fireRateMultiplier = 1;
        towerStats.SetTowersList(_towers);
    }

    protected virtual void Start()
    {
        powerNodeStats._hardCapPenaltyMultiplier = towerStats._fireratePenaltyPercentCap;
        _towerRangeIndicator.range = _range;
        _towerRangeIndicator.gameObject.SetActive(true);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateTowerList();

        if (powerNodeStats.IsOverHardCap())
        {
            ForceTogglePowerOff();
        }
        else
        {
            _forcedPowerDown = false;
        }

        if (_baseFireRate != 0)
        {
            _powerActive = true;
        }

        ChangeSpritePowerState(_powerActive);
    }

    public void UpdateTowerList()
    {
        _towers = towerStats.GetTowersList();
    }

    public void UpdateUpgradeRadialButtonState()
    {
    
        if (_upgradeLevel+1 > _maxUpgradeLevel)
        {
            _canUpgradeButton.SetActive(false);
            _cannotUpgradeButton.SetActive(true);
            return;
        }

        if (towerStats.CanSpendMoneyForUpgrade(_upgradeResourceCost[_upgradeLevel+1]))
        {
            _canUpgradeButton.SetActive(true);
            _cannotUpgradeButton.SetActive(false);
        }
        else
        {
            _canUpgradeButton.SetActive(false);
            _cannotUpgradeButton.SetActive(true);
        }
    }

    public void UpgradeTower()
    {
        AudioManager.instance.PlayInGameUISFX(5);

        _upgradeLevel++;

        for (int i = 1; i < upgradeLevelSprites.Length+1; i++)
        {
            upgradeLevelSprites[i-1].SetActive(false);

            if (i == _upgradeLevel)
            {
                upgradeLevelSprites[i-1].SetActive(true);
            }
        }
        
        powerNodeStats.GainUpkeep(_additionalUpkeep);

        _additionalUpkeep = (_upkeepCost * (_upgradeLevel+1) * (_upgradePowerCostPercent / 100));
        _modifiedUpkeep =  _upkeepCost + _additionalUpkeep;
        powerNodeStats.SpendUpkeep(_additionalUpkeep);

        towerStats.SpendMoneyForUpgrade(_upgradeResourceCost[_upgradeLevel]);
    }

    public void ClickingNoUpgradeButton()
    {
        AudioManager.instance.PlayInGameUISFX(9);
    }

    public void SellTower()
    {
        AudioManager.instance.PlayInGameUISFX(6);
        towerStats.SellTower(gameObject);
    }

    public void TogglePowerOn()
    {
        AudioManager.instance.PlayInGameUISFX(2, 0.5f);
        _powerActive = true;
        _fireRateMultiplier = 1;
        powerNodeStats.SpendUpkeep(_modifiedUpkeep);
    }

    public void TogglePowerOff()
    {
        AudioManager.instance.PlayInGameUISFX(3, 0.5f);
        _powerActive = false;
        _fireRateMultiplier = 0;
        powerNodeStats.GainUpkeep(_modifiedUpkeep);
    }

    
    public void ForceTogglePowerOff()
    {
        _forcedPowerDown = true;
        _powerActive = false;
        //if (_powerDownButton && _powerUpButton != null)
        //{
        //    _powerDownButton.SetActive(false);
        //    _powerUpButton.SetActive(true);
        //    _powerUpButton.GetComponent<Button>().interactable = false;
        //}
    }

    public void LeaveForcedPowerDown()
    {
        if (_powerUpButton != null) _powerUpButton.GetComponent<Button>().interactable = true;
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
            Debug.Log("firing at -" + (100 * towerStats.GetPenaltyMultiplier()) + "%");
            Debug.Log("base fire rate: " + _baseFireRate);
            Debug.Log("fire rate: " + _fireRate);
            Debug.Log("fire rate mult: " + _fireRateMultiplier);

        }
        else
        {
            //Debug.Log("not overcapped base fire rate: " + _baseFireRate);
            //Debug.Log("not overcapped fire rate: " + _fireRate);
            _fireRate = _baseFireRate * _fireRateMultiplier;
        }
    }

    public void ChangeSpritePowerState(bool isActive)
    {
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            if (_forcedPowerDown)
            {
                sprite.color = new Color(0.4f, 0.4f, 0.4f, 0.97f);
            }
            else
            {
                if (isActive)
                {
                    sprite.color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    sprite.color = new Color(0.6f, 0f, 0f, 0.97f);
                }
            }

        }
    }

    void CollectSpriteRenderersRecursive(Transform parent)
    {
        // Get SpriteRenderer component from the current GameObject
        SpriteRenderer spriteRenderer = parent.GetComponent<SpriteRenderer>();

        // Add to the list if found
        if (spriteRenderer != null)
        {
            spriteRenderers.Add(spriteRenderer);
        }

        // Recursively iterate through all children
        foreach (Transform child in parent)
        {
            CollectSpriteRenderersRecursive(child);
        }
    }

}
