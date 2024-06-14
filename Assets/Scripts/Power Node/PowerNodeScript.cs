using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PowerNodeScript : MonoBehaviour
{
    [Header("Power Node Attributes")]
    [SerializeField] private float _maxHealth;

    [Header("[DEBUG] Status")]
    [SerializeField] private float _health;
    [SerializeField] private float _energy;

    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
        _energy = 0;
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
}
