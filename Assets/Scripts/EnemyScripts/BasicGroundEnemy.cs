using System.Collections;
using UnityEngine;

public class BasicGroundEnemy : GroundEnemy
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

    
    private Coroutine attackOrder;
    private Transform target;
    private int pathIndex = 0;
    private float timeToFire;

    void Start()
    {
        InitializeEnemy();
    }

    void Update()
    {
        if (targetDead)
        {
            CheckAndMove();
        }
    }

    private void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    IEnumerator Attack(PowerNodeScript targetNode)
    {
        if (targetNode == null)
        {
            targetDead = true;
        }
        else
        {
            
            yield return new WaitForSeconds(meleeSpeed);
            DoDamage(targetNode);
            attackOrder = StartCoroutine(Attack(targetNode));
        }
    }

    public void DoDamage(PowerNodeScript targetNode)
    {
        if (targetNode != null)
        {
            targetNode.takeHealthDamage(meleeDamage);
        }
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    private void CheckAndMove()
    {
        if (target != null && Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            UpdateTarget();
        }
    }

    private void UpdateTarget()
    {
        pathIndex++;

        if (pathIndex >= LevelManage.main.Path.Length)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        }
        else
        {
            target = LevelManage.main.Path[pathIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PowerNode"))
        {
            HandlePowerNodeCollision(collision);
        }
    }

    private void HandlePowerNodeCollision(Collider2D collision)
    {
        PowerNodeScript tempNode = collision.gameObject.GetComponent<PowerNodeScript>();

        if (tempNode != null)
        {
            targetDead = false;
            coroutineStarted = true;
            attackOrder = StartCoroutine(Attack(tempNode));
        }
    }

    private void InitializeEnemy()
    {
        coroutineStarted = false;
        targetDead = true;
        if (LevelManage.main.Path.Length > 0)
        {
            target = LevelManage.main.Path[pathIndex];
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "PowerNode" && !coroutineStarted)
    //    {
    //        targetDead = false;

    //        PowerNodeScript tempNode = collision.gameObject.GetComponent<PowerNodeScript>();

    //        coroutineStarted = true;

    //        attackOrder = StartCoroutine(AttackNode(tempNode));
    //    }
    //}
}
