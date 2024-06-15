using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeavyEnemyScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask nodeMask;


    [Header("Enemy Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float meleeRange;
    [SerializeField] private int meleeDamage;
    [SerializeField] private float meleeSpeed;

    private Transform nodeTarget;
    private Transform target;
    private int pathIndex = 0;
    private float timeToFire;
    void Start()
    {
        target = LevelManage.main.Path[pathIndex];
    }


    void Update()
    {
        //if (nodeTarget == null)
        //{
        //    FindTarget();
        //    return;
        //}

        //if (!CheckTargetInRange())
        //{
        //    nodeTarget = null;
        //}
        //else
        //{
        //    timeToFire += Time.deltaTime;
        //    if (timeToFire >= 1f / meleeSpeed)
        //    {
        //        timeToFire = 0f;
        //        Attack();
        //    }

        //}

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;



            if (pathIndex >= LevelManage.main.Path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManage.main.Path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }

    private void Attack()
    {
        Debug.Log("Shing shing");
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(nodeTarget.position, transform.position) <= meleeRange;
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, meleeRange, (Vector2)transform.position, 0f, nodeMask);

        if (hits.Length > 0)
        {
            nodeTarget = hits[0].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, meleeRange);
    }
}
