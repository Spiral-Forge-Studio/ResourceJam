using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class PowerNodeScript : MonoBehaviour
{
    [Header("Power Node Attributes")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxEnergy;

    [Header("[DEBUG] Status and References")]
    [SerializeField] private float _health;
    [SerializeField] private float _energy;
    [SerializeField] public CanvasGroup _canvasGroup;
    [SerializeField] public Image _healthBar;


    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
        _energy = 0;
    }

    private void checkHealthStatus()
    {
        if (_health <= 0)
        {
            _canvasGroup.blocksRaycasts = true;
            gameObject.SetActive(false);
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

    // read values
    public float getHealth() => _health;
    public float getMaxHealth() => _maxHealth;
    public float getEnergy() => _energy;
    public float getMaxEnergy() => _maxEnergy;
}
