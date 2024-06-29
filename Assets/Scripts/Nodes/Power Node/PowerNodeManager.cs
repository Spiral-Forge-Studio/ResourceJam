using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNodeManager : MonoBehaviour
{
    [Header("[REFERENCES]")]
    [SerializeField] public PowerNodeStats powerNodeStats;
    [SerializeField] public GameState gameState;

    [Header("[DEBUG]")]
    [SerializeField] public List<GameObject> _powerNodes = new List<GameObject>();
    [SerializeField] private float _totalHealth;
    [SerializeField] private float _totalMaxHealth;
    [SerializeField] private float _totalUpkeepEnergy;
    [SerializeField] private float _totalMaxUpkeep;

    // Start is called before the first frame update
    void Awake()
    {
        InitPowerNodeManager();
    }

    private void Start()
    {
        powerNodeStats.powerNodes = _powerNodes;
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        powerNodeStats.powerNodes = _powerNodes;
        UpdateStats();

        if (gameState._repairPowerNodes == true)
        {
            RepairNodes();
            gameState._repairPowerNodes = false;
        }
    }

    public void InitPowerNodeManager()
    {
        ResetAttributes();

        powerNodeStats.SetOverCapped(false);
        powerNodeStats.powerNodes = _powerNodes;
        powerNodeStats.SetTotalHealth(_totalHealth);
        powerNodeStats.SetMaxUpkeep(_totalMaxUpkeep);
        powerNodeStats.SetUpkeep(_totalUpkeepEnergy);
    }

    public void UpdateStats()
    {
        ResetAttributes();

        foreach (GameObject node in _powerNodes)
        {
            if (node.gameObject != null)
            {
                PowerNodeScript powerNode;
                node.TryGetComponent<PowerNodeScript>(out powerNode);

                _totalHealth += powerNode.GetHealth();
                _totalMaxUpkeep += powerNode.GetMaxEnergy();
            }
        }

        powerNodeStats.SetTotalHealth(_totalHealth);
        powerNodeStats.SetMaxUpkeep(_totalMaxUpkeep);
        _totalUpkeepEnergy = powerNodeStats.GetUpkeep();
        powerNodeStats.CheckOverCapped();
    }

    public void ResetAttributes()
    {
        _totalHealth = 0;
        _totalMaxHealth = 0;
        _totalMaxUpkeep = 10;
    }

    public void RepairNodes()
    {
        foreach (GameObject node in _powerNodes)
        {
            if (node.gameObject != null)
            {
                PowerNodeScript powerNode;
                node.TryGetComponent<PowerNodeScript>(out powerNode);

                powerNode.gainHealth(powerNode.GetMaxHealth() * (powerNode._percentRepaired/100));
            }
        }
    }
}
