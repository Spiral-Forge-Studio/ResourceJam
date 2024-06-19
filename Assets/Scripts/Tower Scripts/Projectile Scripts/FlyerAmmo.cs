using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAmmo : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDamage;

    private Transform miningtarget;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!miningtarget) return;

        Vector2 direction = (miningtarget.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    public void SetTarget(Transform _target)
    {
        miningtarget = _target;
    }

    private void OnCollisionTrigger2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision == null) return;

        if (collision.gameObject.CompareTag("ResourceNode"))
        {
            Debug.Log("Take dmg plap plap");
            ResourceNodeScript temp = collision.gameObject.GetComponent<ResourceNodeScript>();
            temp.takeHealthDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
