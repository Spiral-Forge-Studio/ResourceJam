using System.Collections;
using UnityEngine;

public class Sam_Missile : BulletParent
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] public GameObject explosionPrefab;

    [Header("Speed Controls")]
    [SerializeField] private float initialSpeed = 20f;
    [SerializeField] private float slowSpeed = 5f;
    [SerializeField] private float finalSpeed = 15f;
    [SerializeField] private float initialBurstDuration = 0.5f;
    [SerializeField] private float slowDuration = 1.5f;
    [SerializeField] private float accelerationDuration = 2.0f;

    [Header("Size Control")]
    [SerializeField] private Vector3 initialScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 finalScale = new Vector3(2f, 2f, 2f);

    [Header("Other Specs")]
    [SerializeField] private float areaOfEffect = 2f;
    [SerializeField] private float lifeSpan;

    private Transform target;
    private float _currentSpeed;

    void Start()
    {
        Destroy(gameObject, lifeSpan);
        _currentSpeed = initialSpeed;
        transform.localScale = initialScale; // Set initial scale
        StartCoroutine(SpeedControlCoroutine());
    }

    void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }
        

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * _currentSpeed;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void SamSetTarget(Transform _target)
    {
        target = _target;
    }

    private IEnumerator SpeedControlCoroutine()
    {
        // Initial burst of speed
        _currentSpeed = initialSpeed;
        yield return new WaitForSeconds(initialBurstDuration);

        // Slow down
        _currentSpeed = slowSpeed;
        yield return new WaitForSeconds(slowDuration);

        // Gradual speed up and size increase
        float elapsedTime = 0f;
        while (elapsedTime < accelerationDuration)
        {
            _currentSpeed = Mathf.Lerp(slowSpeed, finalSpeed, elapsedTime / accelerationDuration);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / accelerationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final speed and size are set correctly at the end
        _currentSpeed = finalSpeed;
        transform.localScale = finalScale;
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= areaOfEffect;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, areaOfEffect, enemyMask);

        foreach (Collider2D c in hits)
        {
            if (c.GetComponent<Enemy>())
            {
                Instantiate(explosionPrefab, c.transform.position, Quaternion.identity);
                Enemy hitEnemy = c.GetComponent<Enemy>();
                hitEnemy.takeDamage(_damage);
            }
        }

        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject, 0.2f);
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, areaOfEffect);
    }*/
}
