using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SAM : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private Transform turretRotation; //SAve this for sprite if needed
    [SerializeField] private TowerStats towerStats;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Attributes")]
    [SerializeField] private float tartgetInRange = 10f;
   // [SerializeField] private float rotationSpeed = 2.0f; //Save this for sprites if needed
    [SerializeField] private float fireRate = 1f;

    [SerializeField] private float purchasePrice;
    [SerializeField] private float upgradePrice;
    [SerializeField] private float upkeepCost;
    [SerializeField] private float damage;

    private Transform target;
    private float timeToFire;

    void Start()
    {
        purchasePrice = towerStats.getSAMAttributes()[0];
        upgradePrice = towerStats.getSAMAttributes()[1];
        upkeepCost = towerStats.getSAMAttributes()[2];
        damage = towerStats.getSAMAttributes()[3];
    }


    void Update()
    {
        if (target == null)
        {
            SamFindTarget();
            return;
        }

        if (!SamCheckTargetInRange())
        {
            target = null;
        }
        else
        {
            timeToFire += Time.deltaTime;
            if (timeToFire >= 1f / fireRate)
            {
                timeToFire = 0f;
                SamShoot();
            }

        }
    }

    private void SamShoot()
    {
        GameObject missileObj = Instantiate(missilePrefab, firePoint.position, Quaternion.identity);

        Sam_Missile samMissile = missileObj.GetComponent<Sam_Missile>();

        samMissile.SamSetTarget(target);
    }

    private bool SamCheckTargetInRange()
    {
        return Vector2.Distance(target.position, firePoint.position) <= tartgetInRange;
    }

    private void SamFindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(firePoint.position, tartgetInRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    /*private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(firePoint.position, firePoint.forward, tartgetInRange);
    }*/
}
