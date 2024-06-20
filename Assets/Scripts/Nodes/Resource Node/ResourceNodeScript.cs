using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNodeScript : MonoBehaviour, INode
{
    [Header("Node Attributes")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float RPM;
    [SerializeField] private float baseRPM;
    [SerializeField] private float multiplier;
    [SerializeField] private float additional;

    [Header("References")]
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] public Image healthBar;


    private void Awake()
    {
        health = maxHealth;
        multiplier = 1;
        additional = 0;

        updateRPM();
    }
    private void Update()
    {
        updateRPM();
        updateHealthBar();
        checkHealthStatus();
    }

    private void checkHealthStatus()
    {
        if (health <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            Destroy(gameObject);
        }    
    }

    private void updateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }
    public float getHealth() => health;
    public float getRPM() => RPM;
    private void updateRPM()
    {
        RPM = (baseRPM * multiplier) + additional;
    }
    public void increaseAdditional(float amount)
    {
        additional += amount;
    }
    public void decreaseAdditional(float amount)
    {
        additional -= amount;
    }
    public void increaseMultiplier(float amount)
    {
        if (amount != 0)
        {
            if (multiplier == 1)
            {
                multiplier *= amount;
            }
            else
            {
                multiplier += amount;
            }
        }
    }
    public void decreaseMultiplier(float amount)
    {
        multiplier -= amount;
    }
    public void divideMultiplier(float amount)
    {
        multiplier /= amount;
    }

    public void takeHealthDamage(float amount)
    {
        health -= amount;
    }
}
