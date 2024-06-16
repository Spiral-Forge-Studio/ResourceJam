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

    [SerializeField] private Animator animator;
    private Coroutine attackOrder;
    private Transform target;
    private int pathIndex = 0;
    private float timeToFire;

    void Start()
    {
        coroutineStarted = false;
        targetDead = true;
        target = LevelManage.main.Path[pathIndex];
    }

    void Update()
    {
        if(targetDead)
        {
            if (coroutineStarted == true)
            {
                StopCoroutine(attackOrder);
            }

            Move();
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }

    IEnumerator Attack(PowerNodeScript targetNode)
    {
        if (targetNode == null)
        {
            targetDead = true;
        }

        animator.Play("HeavyEnemySpinAttack",0,0);
        yield return new WaitForSeconds(meleeSpeed);
        DoDamage(targetNode);

        attackOrder = StartCoroutine(Attack(targetNode));
    }

    public void DoDamage(PowerNodeScript targetNode)
    {
        targetNode.takeHealthDamage(meleeDamage);
    }

    private void Move()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PowerNode")
        {
            targetDead = false;

            PowerNodeScript tempNode = collision.gameObject.GetComponent<PowerNodeScript>();

            Debug.Log("This is a node");

            coroutineStarted = true;

            attackOrder = StartCoroutine(Attack(tempNode));
        }
    }
}
