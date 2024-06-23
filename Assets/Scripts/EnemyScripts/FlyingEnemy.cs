using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [Header("Pathing Specifications")]
    [SerializeField] public PathStats pathStats;
    [SerializeField] public float pathDistanceTrigger = 0.1f;
    [SerializeField] public float minMoveAgainDelay = 0.1f;
    [SerializeField] public float maxMoveAgainDelay = 1f;

    [Header("Other Specs")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("DEBUG")]
    [SerializeField] public Animator animator;
    public Coroutine attackOrder;
    [SerializeField] public bool targetDead;
    [SerializeField] public bool engagingTarget;
    [SerializeField] public bool coroutineStarted;
    public float distanceToTarget;
    public Transform target;
    public int pathIndex;

    [Header("Deviation Settings")]
    [SerializeField] private float deviationRadius = 0.5f; // Maximum deviation radius
    private Vector3 targetPositionWithDeviation;

    private void Awake()
    {
        isFlying = true;
        engagingTarget = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        setTargetPath(pathIndex);
        coroutineStarted = false;
        targetDead = true;
    }

    private void Update()
    {
        updateLogic();
    }

    private void FixedUpdate()
    {
        fixedUpdateLogic();
    }

    public void fixedUpdateLogic()
    {
        Vector3 direction = (targetPositionWithDeviation - transform.position).normalized;


        if (target != null) faceDirection(target);

        if (targetDead)
        {
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void updateLogic()
    {
        if (targetDead)
        {
            if (coroutineStarted)
            {
                StopCoroutine(attackOrder);
                coroutineStarted = false;
            }
            pathMove();
        }
    }

    public IEnumerator AttackNode(INode targetNode, Transform[] firepoints)
    {
        while (gameState.IsPaused()) yield return null;

        if (targetNode.IsUnityNull())
        {
            yield return new WaitForSecondsRealtime(Random.Range(minMoveAgainDelay, maxMoveAgainDelay));

            setTargetPath(pathIndex);
            targetDead = true;
        }
        else
        {
            target = targetNode.GetTransform();
            animator.Play(attackAnimation, 0, 0);

            for (int i = 0; i < firepoints.Length; i++)
            {
                Shoot(targetNode.GetTransform(), bulletPrefab, firepoints[i]);
            }

            yield return new WaitForSecondsRealtime(attackSpeed);
            attackOrder = StartCoroutine(AttackNode(targetNode, firepoints));
        }
    }

    public void DoDamage(INode targetNode)
    {
        //Debug.Log("Dealing damage: " + damage);
        targetNode.takeHealthDamage(damage);
    }

    public void pathMove()
    {
        distanceToTarget = Vector2.Distance(targetPositionWithDeviation, transform.position);

        if (distanceToTarget <= pathDistanceTrigger)
        {
            pathIndex++;

            if (pathIndex >= GetPathLength())
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                setTargetPath(pathIndex);
            }
        }
    }

    public void setTargetPath(int _pathIndex)
    {
        target = pathStats.GetFlyingPath(pathAssignment).pointList[_pathIndex];
        targetPositionWithDeviation = GetDeviatedPosition(target.position);
    }

    public int GetPathLength()
    {
        return pathStats.GetFlyingPath(pathAssignment).GetPathLength();
    }

    private Vector3 GetDeviatedPosition(Vector3 originalPosition)
    {
        // Calculate a random offset within a circle
        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Random.Range(0f, deviationRadius);
        Vector3 deviation = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        return originalPosition + deviation;
    }
}
