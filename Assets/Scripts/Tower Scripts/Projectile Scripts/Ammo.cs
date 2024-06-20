using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : BulletParent
{
    private Transform target;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * _bulletSpeed;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0,0,rotation);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy hitEnemy = collision.gameObject.GetComponent<Enemy>();
            hitEnemy.takeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
