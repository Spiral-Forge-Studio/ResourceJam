using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    private GameObject tower;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (tower != null) return;
        {
            GameObject tempTower = BuildTowerManager.main.GetSelectedTower();
            tower = Instantiate(tempTower, transform.position, Quaternion.identity);

        }
    }
}
