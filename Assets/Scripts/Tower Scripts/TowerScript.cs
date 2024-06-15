using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [Header("Tower Specific Attributes")]
    [SerializeField] public float _damage;
    [SerializeField] public float _fireRate;
    [SerializeField] public float _range;
    [SerializeField] public float _upkeep;
    [SerializeField] public float _cost;
    [SerializeField] public float _upgradeCost;

}
