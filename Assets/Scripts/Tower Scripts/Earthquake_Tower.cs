using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Earthquake_Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float quakeRange = 10f;
    [SerializeField] private float quakePerSecond = 1f;
    [SerializeField] private int damagePoint;

    private float timeToFire;
    void Start()
    {
        
    }

    void Update()
    {
        timeToFire += Time.deltaTime;
        if (timeToFire >= 1f / quakePerSecond)
        {
            Debug.Log("quake");
            timeToFire = 0f;
            StartQuake();
        }
    }

    private void StartQuake()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, quakeRange, enemyMask);

        foreach (Collider2D c in hits)
        {
            if (c.GetComponent<Health>())
            {
                c.GetComponent<Health>().TakeDamage(damagePoint);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.forward, quakeRange);
    }
}
