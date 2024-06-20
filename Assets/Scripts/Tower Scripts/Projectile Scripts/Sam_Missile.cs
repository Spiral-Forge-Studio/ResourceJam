using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Sam_Missile : BulletParent
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyMask;

    
    [SerializeField] private float areaOfEffect = 2f;

    private Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * _bulletSpeed;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, rotation);

    }

    public void SamSetTarget(Transform _target)
    {
        target = _target;
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= areaOfEffect;
    }
    
    /*
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, areaOfEffect);
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, areaOfEffect, enemyMask);

        foreach (Collider2D c in hits)
        {
            if (c.GetComponent<Health>())
            {
                c.GetComponent<Health>().TakeDamage(_damage);
            }
        }
       
        Destroy(gameObject);
    }

}
