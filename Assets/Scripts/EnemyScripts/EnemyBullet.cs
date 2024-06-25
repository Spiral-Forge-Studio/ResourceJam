using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;


    void Start()
    {
        StartCoroutine(DestroyAfterDelay(lifeSpan));
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * speed;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        else if (collision.gameObject.CompareTag("ResourceNode"))
        {
            ResourceNodeScript node = collision.gameObject.GetComponent<ResourceNodeScript>();
            node.takeHealthDamage(damage);
            Destroy(gameObject);
        }        
        else if (collision.gameObject.CompareTag("PowerNode"))
        {
            PowerNodeScript node = collision.gameObject.GetComponent<PowerNodeScript>();
            node.takeHealthDamage(damage);
            Destroy(gameObject);
        }        
        else if (collision.gameObject.CompareTag("Headquarters"))
        {
            Headquarters node = collision.gameObject.GetComponent<Headquarters>();
            node.takeHealthDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(gameObject);
    }
}
