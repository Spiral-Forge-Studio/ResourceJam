using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Earthquake_Tower : TowerParent
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [SerializeField] private int damagePoint;

    private float timeToFire;
    void Start()
    {
        
    }

    void Update()
    {
        timeToFire += Time.deltaTime;
        if (timeToFire >= 1f / _fireRate)
        {
            Debug.Log("quake");
            timeToFire = 0f;
            StartQuake();
        }
    }

    private void StartQuake()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _range, enemyMask);

        foreach (Collider2D c in hits)
        {
            if (c.GetComponent<Health>())
            {
                c.GetComponent<Health>().TakeDamage(damagePoint);
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.green;
    //    Handles.DrawWireDisc(transform.position, transform.forward, _range);
    //}
}
