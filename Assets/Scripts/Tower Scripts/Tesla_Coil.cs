using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tesla_Coil : TowerParent
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Attributes")]
    [SerializeField] private float teslaRange = 10f;
    [SerializeField] private float fireRate = 1f;

    private Transform target;
    private float timeToFire;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (target == null)
        {
            TeslaFindTarget();
            return;
        }

        if (!TeslaCheckTargetInRange())
        {
            target = null;
        }
        else
        {
            timeToFire += Time.deltaTime;
            if (timeToFire >= 1f / _range)
            {
                timeToFire = 0f;
                TeslaShoot();
            }

        }
    }

    private void TeslaShoot()
    {
        GameObject lightningObj = Instantiate(lightningPrefab, firePoint.position, Quaternion.identity);
        ChainLightningScript lightningScript = lightningObj.GetComponent<ChainLightningScript>();

        lightningScript.SetTarget(target);
    }
    private bool TeslaCheckTargetInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= _range;
    }

    private void TeslaFindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _range, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, teslaRange);
    }*/
}
