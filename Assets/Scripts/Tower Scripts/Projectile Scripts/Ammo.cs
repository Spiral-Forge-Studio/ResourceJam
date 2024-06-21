using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ammo : BulletParent
{
    private Transform target;
    [SerializeField] private float lifeSpan;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    
    void Start()
    {
        StartCoroutine(DestroyAfterDelay(lifeSpan));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy hitEnemy = collision.gameObject.GetComponent<Enemy>();
            //hitEnemy.takeDamage(_damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(gameObject);
    }
}
