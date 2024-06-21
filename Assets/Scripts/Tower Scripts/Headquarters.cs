using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headquarters : MonoBehaviour, INode
{
    [Header("Attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    public void takeHealthDamage(float amount)
    {
        health -= amount;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
