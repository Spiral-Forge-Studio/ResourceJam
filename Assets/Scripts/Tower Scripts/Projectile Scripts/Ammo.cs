using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : BulletParent
{
    public TowerStats towerstats;
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

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0,0,rotation);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Health>().TakeDamage(_damage);
        Destroy(gameObject);
    }
}
