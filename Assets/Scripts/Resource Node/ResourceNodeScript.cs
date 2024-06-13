using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeScript : MonoBehaviour
{
    [Header("Node Attributes")]
    [SerializeField] private int health;
    [SerializeField] private float RPM;
    [SerializeField] private float baseRPM;
    [SerializeField] private float multiplier;
    [SerializeField] private float additional;

    private void Start()
    {
        multiplier = 1;
        additional = 0;

        updateRPM();
    }
    private void Update()
    {
        updateRPM();
    }

    public int getHealth() => health;
    public float getRPM() => RPM;
    private void updateRPM()
    {
        RPM = (baseRPM * multiplier) + additional;
    }
    public void increaseAdditional(float amount)
    {
        additional += amount;
    }
    public void increaseMultiplier(float amount)
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
