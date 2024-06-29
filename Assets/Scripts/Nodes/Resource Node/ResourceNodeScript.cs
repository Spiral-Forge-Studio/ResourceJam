using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNodeScript : MonoBehaviour, INode
{
    [Header("[BALANCING]")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] public float cost;
    [SerializeField] public float percentRepaired;
    [SerializeField] private float RPM;
    [SerializeField] private float baseRPM;
    [SerializeField] private float multiplier;
    [SerializeField] private float additional;

    [Header("[REFERENCES]")]
    [SerializeField] public ResourceStats resourceStats;
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] public Image healthBar;


    private void Awake()
    {
        health = maxHealth;
        multiplier = 1;
        additional = 0;

        updateRPM();
    }

    private void FixedUpdate()
    {
        checkHealthStatus();
        updateHealthBar();
    }

    private void Update()
    {
        updateRPM(); 
    }

    private void checkHealthStatus()
    {
        //Debug.Log(gameObject.name + " health: " +  health);
        if (health <= 0)
        {
            //Debug.Log("should die");
            canvasGroup.blocksRaycasts = true;
            Destroy(gameObject);
        }    
    }

    private void updateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }
    public float getHealth() => health;
    public float getMaxHealth() => maxHealth;
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
    
    public void gainHealth(float amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
