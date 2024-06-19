using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChainLightningScript : BulletParent
{
    public TowerStats towerstats;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CircleCollider2D coll;
    [SerializeField] private GameObject chainLightningEffect;
    [SerializeField] private GameObject beenStruck;
    private GameObject startObject;
    private GameObject endObject;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem parti;


    private float damage;
    private int amountToChain;


    private int singleSpawns;

    private Transform target;

    private void Awake()
    {   
        amountToChain = towerstats._teslaCoilAmountToChain;
    }
    void Start()
    {

        if (amountToChain == 0) Destroy(gameObject);

        coll = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        parti = GetComponent<ParticleSystem>();

        startObject = gameObject;

        singleSpawns = 1;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction;

    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemyMask == (enemyMask | (1 << collision.gameObject.layer)) && !collision.GetComponentInChildren<EnemyStruck>()){

            if (singleSpawns != 0)
            {

                endObject = collision.gameObject;

                amountToChain -= 1;

                Instantiate(chainLightningEffect, collision.gameObject.transform.position, Quaternion.identity);

                Instantiate(beenStruck, collision.gameObject.transform);

                collision.gameObject.GetComponent<Health>().TakeDamage(_damage);

                anim.StartPlayback();

                coll.enabled = false;

                singleSpawns--;

                parti.Play();

                var emitParams = new ParticleSystem.EmitParams();
                emitParams.position = startObject.transform.position;
                parti.Emit(emitParams, 1);

                emitParams.position = endObject.transform.position;
                parti.Emit(emitParams, 1);

                emitParams.position = (startObject.transform.position + endObject.transform.position) / 2;
                parti.Emit(emitParams, 1);

                Destroy(gameObject, 1f);
            }
        }
    }
}
