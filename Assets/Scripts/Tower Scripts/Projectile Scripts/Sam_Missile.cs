using System.Collections;
using UnityEngine;

public class Sam_Missile : BulletParent
{
    [Header("References")]
    [SerializeField] public BulletStats bulletStats;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] public GameObject explosionPrefab;
    [SerializeField] private ParticleSystem parti;

    [Header("Speed Controls")]
    [SerializeField] public float initialSpeed;
    [SerializeField] public float slowSpeed;
    [SerializeField] public float finalSpeed;
    [SerializeField] public float initialBurstDuration;
    [SerializeField] public float slowDuration;
    [SerializeField] public float accelerationDuration;

    [Header("Size Control")]
    [SerializeField] public Vector3 initialScale;
    [SerializeField] public Vector3 finalScale;

    [Header("Other Specs")]
    [SerializeField] public float areaOfEffect;
    [SerializeField] public float lifeSpan;

    private Transform target;
    private float _currentSpeed;
    private Vector2 lastDirection;

    private void Awake()
    {
        towerStats.SetSAMMissileStats(this);
    }

    void Start()
    {
        explosionPrefab.SetActive(false);
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

        Enemy enemy = target.gameObject.GetComponent<Enemy>();

        if (enemy != null && enemy.isDead)
        {
            // Maintain the current velocity
            rb.velocity = lastDirection * _currentSpeed;
        }
        else
        {
            // Calculate new direction and velocity
            Vector2 direction = (target.position - transform.position).normalized;
            lastDirection = direction;
            rb.velocity = direction * _currentSpeed;

            float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
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
                //parti.Play();
                explosionPrefab.SetActive(true);
                Enemy hitEnemy = c.GetComponent<Enemy>();
                hitEnemy.takeDamage(_damage);
                Destroy(gameObject);
            }
            
        }

        parti.Play();
        Destroy(gameObject, lifeSpan);
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, areaOfEffect);
    }*/
}
