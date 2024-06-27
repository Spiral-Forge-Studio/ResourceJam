using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Ballista : TowerParent
{
    [Header("References")]
    [SerializeField] private Transform turretRotation;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] public float rotationSpeed = 2.0f;
   

    private Transform target;
    private float timeToFire;

    protected override void Awake()
    {
        base.Awake();

        towerStats.SetAutoCannon(this);

        _modifiedUpkeep = _upkeepCost;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        UpdateUpgradeRadialButtonState();
        UpdateDamage();
        UpdateFirerate();

        if (target == null) {
            FindTarget();
            return;
        }

        if (target.gameObject.GetComponentInChildren<Enemy>().isDead)
        {
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

    void Shoot()
    {   
        if (target.gameObject.GetComponentInChildren<Enemy>().isDead)
        {
            return;
        }

        turretRotation.GetComponentInChildren<Animator>().Play("AutoCannonBarrelFiring");
        AudioManager.instance.PlaySFX("Autocannon");

        GameObject ammoObj = Instantiate(ammoPrefab, firePoint.position, Quaternion.identity);
        Ammo ammoScript = ammoObj.GetComponent<Ammo>();

        towerStats.SetAutoCannonBulletStats(ammoScript, this);

        ammoScript.SetTarget(target);
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(target.position, turretRotation.position) <= _range;
    }

    private void RotateTowardsTarget()
    {
        if (!_powerActive) return;

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotation.rotation = Quaternion.RotateTowards(turretRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private void FindTarget()
    {
        List<RaycastHit2D> allHits = new List<RaycastHit2D>();

        // Perform CircleCast for the enemyMask LayerMask and collect results
        RaycastHit2D[] hits = Physics2D.CircleCastAll(turretRotation.position, _range, Vector2.zero, 0f, enemyMask);
        allHits.AddRange(hits);

        // If there are hits, prioritize the target based on distance to HQ
        if (allHits.Count > 0)
        {
            RaycastHit2D closestHit = allHits[0];
            float closestDistanceToHQ = Vector2.Distance(closestHit.transform.position, towerStats.GetHQTransform().position);

            foreach (RaycastHit2D hit in allHits)
            {
                float distanceToHQ = Vector2.Distance(hit.transform.position, towerStats.GetHQTransform().position);
                if (distanceToHQ < closestDistanceToHQ && !hit.collider.gameObject.GetComponentInChildren<Enemy>().isDead)
                {
                    closestHit = hit;
                    closestDistanceToHQ = distanceToHQ;
                }
            }

            target = closestHit.transform;
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.red;
    //    Handles.DrawWireDisc(turretRotation.position, turretRotation.forward, _range);
    //}
}
