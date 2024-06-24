using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerParent : MonoBehaviour
{
    [Header("Child Tower Attributes")]
    [SerializeField] public float _price;
    [SerializeField] public float _upkeep;
    [SerializeField] public float _upgrade;
    [SerializeField] public float _range;
    [SerializeField] public float _baseFireRate;
    [SerializeField] public float _fireRateMultiplier;

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
        _fireRateMultiplier = 1;
        towerStats.SetTowersList(_towers);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateTowerList();
        UpdateFirerate();

    }

    public void UpdateTowerList()
    {
        _towers = towerStats.GetTowersList();
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
            //Debug.Log("firing at -" + (100*towerStats.GetPenaltyMultiplier()) + "%");
            //Debug.Log("base fire rate: " + _baseFireRate);
            //Debug.Log("fire rate: " + _fireRate);
            //Debug.Log("fire rate mult: " + _fireRateMultiplier);
            _fireRate = (_baseFireRate * _fireRateMultiplier) - (_baseFireRate * towerStats.GetPenaltyMultiplier());
        }
        else
        {
            //Debug.Log("not overcapped base fire rate: " + _baseFireRate);
            //Debug.Log("not overcapped fire rate: " + _fireRate);
            _fireRate = _baseFireRate * _fireRateMultiplier;
        }
    }
}
