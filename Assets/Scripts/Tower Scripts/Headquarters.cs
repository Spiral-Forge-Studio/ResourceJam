using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Headquarters : MonoBehaviour, INode
{
    [Header("References")]
    public GameState gameState;
    public Image _healthBar;

    [Header("Attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    private void Update()
    {
        updateHealthBar();
        if (health <= 0) 
        {
            Debug.Log("HQ hp: " + health);
            gameState.HqDead = true;
            AudioManager.instance.PlayInGameUISFX(8);
            Destroy(gameObject);
        }

    }

    public void takeHealthDamage(float amount)
    {
        health -= amount;
    }

    private void updateHealthBar()
    {
        _healthBar.fillAmount = health / maxHealth;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
