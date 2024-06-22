using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headquarters : MonoBehaviour, INode
{
    [Header("References")]
    public GameState gameState;

    [Header("Attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health <= 0) { Destroy(gameObject); }
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
