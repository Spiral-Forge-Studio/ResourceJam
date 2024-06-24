using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Earthquake_Tower : TowerParent
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem parti;

    [Header("Attributes")]
    [SerializeField] public float damagePoint;

    private float timeToFire;

    protected override void Awake()
    {
        base.Awake();
        towerStats.SetEarthquakeTower(this);
    }

    protected override void Update()
    {
        base.Update();
        timeToFire += Time.deltaTime;
        if (timeToFire >= 1f / _fireRate)
        {
            //Debug.Log("quake");
            timeToFire = 0f;
            animator.Play("EarthquakeHammerbuildup");
            
        }
    }

    private void StartQuake()
    {
        parti.Play();
        GetComponentInChildren<ParticleSystem>().Play();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _range, enemyMask);

        foreach (Collider2D c in hits)
        {
            if (c.GetComponent<Enemy>())
            {
                
                c.GetComponent<Enemy>().takeDamage(damagePoint);
            }
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.green;
    //    Handles.DrawWireDisc(transform.position, transform.forward, _range);
    //}
}
