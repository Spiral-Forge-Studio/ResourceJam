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

    [SerializeField] private Animator animator;
    private Coroutine attackOrder;
    [SerializeField]  PowerNodeScript nodeTarget;
    private Transform target;
    private int pathIndex = 0;
    private float timeToFire;
    void Start()
    {
        target = LevelManage.main.Path[pathIndex];
    }


    void Update()
    {
        

        if(!nodeTarget)
        {
            Move();
        }

        
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }

    IEnumerator Attack()
    {
        animator.Play("HeavyEnemySpinAttack",0,0);
        yield return new WaitForSeconds(meleeSpeed);
        attackOrder = StartCoroutine(Attack());
    }

    public void DoDamage()
    {
        nodeTarget.takeHealthDamage(meleeDamage);

        //bool nodeDied = nodeTarget.takeHealthDamage(meleeDamage);

        //if (nodeDied)
        //{
        //    nodeTarget = null;
        //    StopCoroutine(attackOrder);
        //}
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
        if (nodeTarget) return;

        if(collision.gameObject.tag == "PowerNode")
        {
            PowerNodeScript tempNode = collision.gameObject.GetComponent<PowerNodeScript>();
            Debug.Log("This is a node");
            //nodeTarget = collision.GetComponent<PowerNodeScript>();
            attackOrder = StartCoroutine(Attack());
        }
    }

    
}
