using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GroundEnemy : Enemy
{
    [Header("Pathing Specifications")]
    [SerializeField] public PathStats pathStats;
    [SerializeField] public int pathAssignment;
    [SerializeField] public float pathDistanceTrigger = 0.1f;
    [SerializeField] public float minMoveAgainDelay = 0.1f;
    [SerializeField] public float maxMoveAgainDelay = 1f;


    [Header("DEBUG]")]
    [SerializeField] public Animator animator;
    public Coroutine attackOrder;
    [SerializeField] public bool targetDead;
    [SerializeField] public bool coroutineStarted;
    public float distanceToTarget;
    public Transform target;
    public int pathIndex;

    private void Start()
    {
        setTargetPath(pathIndex);
        coroutineStarted = false;
        targetDead = true;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        if (targetDead)
        {
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (targetDead)
        {
            if (coroutineStarted == true)
            {
                Debug.Log("Stopped coroutine");
                StopCoroutine(attackOrder);
                coroutineStarted = false;
            }
            Move();
        }
    }

    public IEnumerator AttackNode(INode targetNode)
    {
        Debug.Log("Target node: " + targetNode);

        if (targetNode.IsUnityNull())
        {
            Debug.Log("went in loop: " + targetNode);
            yield return new WaitForSecondsRealtime(
                Random.Range(minMoveAgainDelay, maxMoveAgainDelay));
            targetDead = true;
        }

        else
        {
            animator.Play("HeavyEnemySpinAttack", 0, 0);
            DoDamage(targetNode);
            yield return new WaitForSecondsRealtime(attackSpeed);
            attackOrder = StartCoroutine(AttackNode(targetNode));
        }
    }

    public void DoDamage(INode targetNode)
    {
        targetNode.takeHealthDamage(damage);
    }

    public void Move()
    {
        distanceToTarget = Vector2.Distance(target.position, transform.position);

        if (distanceToTarget <= pathDistanceTrigger)
        {
            pathIndex++;

            if (pathIndex >= GetPathLength())
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                setTargetPath(pathIndex);
                //target = LevelManage.main.Path[pathIndex];
            }
        }
    }
    public void setTargetPath(int _pathIndex)
    {
        target = pathStats.GetPath(pathAssignment).pointList[_pathIndex];
    }
    public int GetPathLength()
    {
        return pathStats.GetPath(pathAssignment).GetPathLength();
    }


}
