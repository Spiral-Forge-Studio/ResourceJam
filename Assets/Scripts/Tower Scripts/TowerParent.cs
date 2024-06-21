using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerParent : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public float _price;
    [SerializeField] public float _upkeep;
    [SerializeField] public float _upgrade;
    [SerializeField] public float _range;
    [SerializeField] public float _fireRate;

    [Header("[REFERENCES]")]
    [SerializeField] public TowerStats towerStats;
    [SerializeField] public Headquarters headquarters;

    [Header("[DEBUG]")]
    [SerializeField] private List<GameObject> _towers = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        towerStats.SetTowersList(_towers);
    }

    private void Start()
    {
        towerStats.SetHQTransform(headquarters.transform);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTowerList();
    }

    public void UpdateTowerList()
    {
        _towers = towerStats.GetTowersList();
    }
}
