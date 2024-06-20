using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class PowerNodeScript : MonoBehaviour, INode
{
    [Header("[BALANCING] Attributes")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxEnergy;

    [Header("[REFERENCES]")]
    [SerializeField] public CanvasGroup _canvasGroup;
    [SerializeField] public Image _healthBar;

    [Header("[DEBUG]")]
    [SerializeField] private float _health;
    [SerializeField] private float _energy;

    // Start is called before the first frame update
    void Awake()
    {
        _health = _maxHealth;
        _energy = 0;
    }

    private void Update()
    {
        checkHealthStatus();
        updateHealthBar();
    }

    private void checkHealthStatus()
    {
        if (_health <= 0)
        {
            _canvasGroup.blocksRaycasts = true;
            Destroy(gameObject);
        }
    }

    private void updateHealthBar()
    {
        _healthBar.fillAmount = _health / _maxHealth;
    }

    // taking damage
    public void takeHealthDamage(float damage)
    {
        _health -= damage;
        //Debug.Log("Taking Damage");
        
    }
    public void takeEnergyDamage(float damage)
    {
        _energy -= damage;
    }

    // regenerating
    public void gainHealth(float health)
    {
        _health += health;
    }
    public void gainEnergy(float energy)
    {
        _energy += energy;
    }

    // buffs?
    public void MultiplyMaxEnergy(float multiplier) => _maxEnergy *= multiplier;
    public void AddMaxEnergy(float additional) => _maxEnergy += additional;


    #region GETTERS AND SETTERS

    public float GetHealth() => _health;
    public float GetMaxHealth() => _maxHealth;
    public float GetEnergy() => _energy;
    public float GetMaxEnergy() => _maxEnergy;    
    public void SetMaxEnergy(float maxEnergy) => _maxEnergy = maxEnergy;

    #endregion
}
