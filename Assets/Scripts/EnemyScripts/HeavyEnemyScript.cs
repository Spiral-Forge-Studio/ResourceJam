using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float meleeDamage;
    [SerializeField] private float meleeSpeed;
    [SerializeField] private bool targetDead;
    [SerializeField] private bool coroutineStarted;

    [Header("Pathing Specifications")]
    [SerializeField] private PathStats pathStats;
    [SerializeField] private float pathDistanceTrigger = 0.1f;
    [SerializeField] private float minMoveAgainDelay = 0.1f;
    [SerializeField] private float maxMoveAgainDelay = 1f;


    [Header("DEBUG]")]
    [SerializeField] private Animator animator;
    private Coroutine attackOrder;
    [SerializeField] private Transform target;
    [SerializeField] private EnemyPathAssignment pathAssignment;
    [SerializeField] private int pathIndex = 0;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float timeToFire;

    void Start()
    {
        pathAssignment = GetComponent<EnemyPathAssignment>();

        coroutineStarted = false;
        targetDead = true;

        setTargetPath(pathIndex);
        Debug.Log("assigned path: " + pathAssignment.GetAssignedPath());
        //target = LevelManage.main.Path[pathIndex];
    }

    void Update()
    {
        if(targetDead)
        {
            if (coroutineStarted == true)
            {
                StopCoroutine(attackOrder);
                coroutineStarted = false;
            }

            Move();
        }
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

    IEnumerator Attack(PowerNodeScript targetNode)
    {
        if (targetNode == null)
        {
            yield return new WaitForSecondsRealtime(
                Random.Range(minMoveAgainDelay, maxMoveAgainDelay));
            targetDead = true;
        }

        else
        {
            animator.Play("HeavyEnemySpinAttack", 0, 0);
            DoDamage(targetNode);
            yield return new WaitForSecondsRealtime(meleeSpeed);
            attackOrder = StartCoroutine(Attack(targetNode));
        }
    }

    public void DoDamage(PowerNodeScript targetNode)
    {
        targetNode.takeHealthDamage(meleeDamage);
    }

    private void Move()
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

    private int GetPathLength()
    {
        return pathStats.GetPath(pathAssignment.GetAssignedPath()).GetPathLength();
    }

    private void setTargetPath(int _pathIndex)
    {
        target = pathStats.GetPath(pathAssignment.GetAssignedPath()).pointList[_pathIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("collided");

        if (collision.gameObject.tag == "PowerNode" && !coroutineStarted)
        {
            targetDead = false;

            PowerNodeScript tempNode = collision.gameObject.GetComponent<PowerNodeScript>();

            //Debug.Log("This is a node");

            coroutineStarted = true;

            attackOrder = StartCoroutine(Attack(tempNode));
        }
    }
}
