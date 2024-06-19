using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Ballista : TowerParent
{
    public TowerStats towerstats;

    [Header("References")]
    [SerializeField] private Transform turretRotation;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Animator animator;


    //private float tartgetInRange;
    private float rotationSpeed;
    //private float fireRate;

    private Transform target;
    private float timeToFire;

    

    void Start()
    {
        //_range = towerstats._autoCannonRange;
        rotationSpeed = towerstats._autoCannonRotation;
        //_fireRate = towerstats._autoCannonFireRate;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (target == null) {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetInRange()){
            target = null;
        }
        else
        {
            timeToFire += Time.deltaTime;
            if (timeToFire >= 1f / _fireRate)
            {
                timeToFire = 0f;
                Shoot();
            }

        }
    }

    private void Shoot()
    {
        animator.GetComponentInChildren<Animator>().Play("BarrelFiring",0,0);
        GameObject ammoObj = Instantiate(ammoPrefab, firePoint.position, Quaternion.identity);
        Ammo ammoScript = ammoObj.GetComponent<Ammo>();

        ammoScript.SetTarget(target);
    }
    private bool CheckTargetInRange()
    {
        return Vector2.Distance(target.position, turretRotation.position) <= _range;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotation.rotation = Quaternion.RotateTowards(turretRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(turretRotation.position, _range, (Vector2)transform.position, 0f, enemyMask );

        if (hits.Length > 0) { 
            target = hits[0].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(turretRotation.position, turretRotation.forward, _range);
    }
}
