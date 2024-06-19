using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Attributes")]
    public float health;
    public float damage;
    public float range;
    public int cost;
    public int upgradeCost;

    [Header("[REFERENCES]")]
    [SerializeField] public TowerStats towerStats;

    [Header("[DEBUG]")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TowerSetup(TowerScript tower)
    {

    }
}
