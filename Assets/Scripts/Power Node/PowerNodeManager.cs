using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNodeManager : MonoBehaviour
{
    [Header("Power Nodes")]
    [SerializeField] public List<GameObject> _powerNodes = new List<GameObject>();

    [Header("Power Node Stats Scriptable Object")]
    [SerializeField] public PowerNodeStats powerNodeStats;

    [Header("[DEBUG] Variables")]
    [SerializeField] private float _totalHealth;
    [SerializeField] private float _totalMaxHealth;
    [SerializeField] private float _upkeepEnergy;
    [SerializeField] private float _totalMaxEnergy;

    // Start is called before the first frame update
    void Awake()
    {
        InitPowerNodeManager();
    }

    // Update is called once per frame
    void Update()
    {
        powerNodeStats.powerNodes = _powerNodes;

        UpdateStats();
    }

    public void InitPowerNodeManager()
    {
        ResetAttributes();

        powerNodeStats.powerNodes = _powerNodes;

        powerNodeStats.totalHealth = _totalHealth;
        powerNodeStats.maxEnergy = _totalMaxEnergy;
    }

    public void UpdateStats()
    {
        ResetAttributes();

        foreach (GameObject node in _powerNodes)
        {
            if (node.gameObject.activeSelf)
            {
                PowerNodeScript powerNode;
                node.TryGetComponent<PowerNodeScript>(out powerNode);

                _totalHealth += powerNode.getHealth();
                _totalMaxEnergy += powerNode.getMaxEnergy();
            }
        }

        powerNodeStats.totalHealth = _totalHealth;
        powerNodeStats.maxEnergy = _totalMaxEnergy;
    }

    public void ResetAttributes()
    {
        _totalHealth = 0;
        _totalMaxHealth = 0;
        _totalMaxEnergy = 20;
    }
}
