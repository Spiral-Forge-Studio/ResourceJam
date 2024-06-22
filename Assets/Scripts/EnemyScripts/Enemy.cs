using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public GameState gameState;

    [Header("Enemy Attributes")]
    [SerializeField] public bool isFlying;
    [SerializeField] public float maxHealth;
    public float health;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float damage;
    [SerializeField] public float range;
    [SerializeField] public float attackSpeed;
    [SerializeField] public string attackAnimation;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public int pathAssignment;

    [Header("DEBUG")]
    [SerializeField] private Coroutine slowRoutine;

    private void Awake()
    {
        health = maxHealth;
    }

    public void takeDamage(float damage)
    {
        
        health -= damage;
        if (health <= 0) destroyEnemy();
    }

    public void startSlowDownCoroutine(float percentage, float duration)
    {
        
        if (slowRoutine == null)
        {
            slowRoutine = StartCoroutine(slowDown(percentage, duration));
        }
        else
        {
            StopCoroutine(slowRoutine);
            slowRoutine = StartCoroutine(slowDown(percentage, duration));
        }
    }

    private IEnumerator slowDown(float percentage, float duration)
    {
        float ogMovespeed = moveSpeed;
        moveSpeed = moveSpeed - (moveSpeed * (percentage / 100));
        yield return new WaitForSecondsRealtime(duration);
        moveSpeed = ogMovespeed;
    }

    private void destroyEnemy()
    {
        Destroy(gameObject);
    }

    public void faceDirection(Transform target)
    {
        // Calculate the rotation angle as before
        Vector2 lookDir = target.position - transform.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.deltaTime);
    }

    public void Shoot(Transform _targetToKill, GameObject _bulletPrefab, Transform _firePoint)
    {
        GameObject spawnedBulletPrefab = Instantiate(_bulletPrefab, _firePoint.position, transform.rotation);
        EnemyBullet bullet = spawnedBulletPrefab.GetComponent<EnemyBullet>();
        bullet.SetTarget(_targetToKill);
    }

}
