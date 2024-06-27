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
    [SerializeField] public string deathAnimation;
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

    protected override void Awake()
    {
        base.Awake();
        isFlying = true;
        engagingTarget = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coroutineStarted = false;
        targetDead = true;
    }

    protected override void Start()
    {
        base.Start();
        setTargetPath(pathIndex);
    }

    protected virtual void Update()
    {
        updateLogic();
    }

    protected virtual void FixedUpdate()
    {
        fixedUpdateLogic();
    }

    public void fixedUpdateLogic()
    {
        if (health <= 0)
        {
            if (attackOrder != null)
            {
                StopCoroutine(attackOrder);
            }
        }
        else
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
    }

    public void updateLogic()
    {
        if (health <= 0)
        {
            if (attackOrder != null)
            {
                StopCoroutine(attackOrder);
            }
            
            Die();
            Destroy(gameObject, 2.0f);
        }

        else if (targetDead)
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

    private void Die()
    {
        if (isDead) return;
        onEnemyDestroy?.Invoke();

        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        isDead = true;

        animator.SetTrigger("Die");
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Get the length of the death animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName(deathAnimation)) // Replace "Death" with the actual name of your death animation state
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Wait for the length of the animation
        yield return new WaitForSeconds(stateInfo.length);

        // Now destroy the enemy object
        Destroy(gameObject);
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
        //Debug.Log("pgn in setTargetPath: " + pathAssignment + ", index: " + _pathIndex);
        //Debug.Break();
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
