using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowerManager : MonoBehaviour
{
    public static BuildTowerManager main;

    [Header("References")]
    [SerializeField] private GameObject[] buildingPrefabs;

    private int currentselectedTower = 0;

    private void Awake()
    {
        main = this;
    }
    public GameObject GetSelectedTower()
    {
        return buildingPrefabs[currentselectedTower];   
    }
}
