using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Earthquake_Tower : TowerParent
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem parti;

    private Transform target;
    private float timeToFire;

    protected override void Awake()
    {
        base.Awake();
        towerStats.SetEarthquakeTower(this);

        _modifiedUpkeep = _upkeepCost;
    }

    protected override void Update()
    {
        base.Update();

        UpdateUpgradeRadialButtonState();
        UpdateDamage();
        UpdateFirerate();

        if (target == null)
        {
            EQTowerFindTarget();
            return;
        }

        if (!EQTowerCheckTargetInRange())
        {
            target = null;
        }
        else
        {
            timeToFire += Time.deltaTime;
            if (timeToFire >= 1f / _fireRate)
            {
                //Debug.Log("quake");
                timeToFire = 0f;
                animator.Play("EarthquakeHammerbuildup");
            }
        }
    }

    private void StartQuake()
    {
        parti.Play();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _range, enemyMask);

        foreach (Collider2D c in hits)
        {
            if (c.GetComponent<Enemy>())
            {
                
                c.GetComponent<Enemy>().takeDamage(_damage);
            }
        }
    }

    private bool EQTowerCheckTargetInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= _range;
    }

    private void EQTowerFindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _range, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.green;
    //    Handles.DrawWireDisc(transform.position, transform.forward, _range);
    //}
}
