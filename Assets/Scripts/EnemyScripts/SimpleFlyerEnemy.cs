using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimpleFlyerEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject flyerBullet;
    [SerializeField] private LayerMask structMask;
    [SerializeField] private Transform firePoint;

    [Header("Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float flyerDamage;
    [SerializeField] private float fireRate;
    [SerializeField] private float flyerRange;

    private Transform target;
    public Transform mininigTarget;
    private Transform powerTarget;
    private bool miningDead;
    private bool powerDead;
    void Start()
    {
        target = LevelManage.main.Path[17];
    }

    
    void Update()
    {
        FindMiningNode();

        if (!miningDead)
        {
            Debug.Log("I'm moving towards the target");
            MoveTowardsMiningNode();

            if (Vector2.Distance(transform.position, mininigTarget.position) <= 1f)
            {
                Debug.Log("Im gonna shoot'em");
                Shoot(mininigTarget);
                miningDead = true;
            }

        }
        else
        {
            Debug.Log("Im moving towards the objective");
            Move();
        }

    }

    private void Move()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
            return;
        }
    }

    private void Shoot(Transform miningTarget)
    {
        GameObject temptbullet = Instantiate(flyerBullet, firePoint.position, Quaternion.identity);
        FlyerAmmo flyerAmmo = temptbullet.GetComponent<FlyerAmmo>();
        flyerAmmo.SetTarget(miningTarget);
    }
    private void FindMiningNode()
    {
        RaycastHit2D [] acquired = Physics2D.CircleCastAll(transform.position, flyerRange, (Vector2)transform.position, 0f, structMask);

        if (acquired.Length > 0)
        {
            mininigTarget = acquired[0].transform;
            //powerTarget = acquired[1].transform;
        }
        else
        {
            miningDead = true;
        }

    }

    private void CheckForNearbyPowerNode()
    {
        //move towards if its nearby
    }

    private void MoveTowardsMiningNode()
    {
        Vector2 direction = (mininigTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;

        

    }

    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, flyerRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
