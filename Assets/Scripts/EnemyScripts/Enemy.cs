using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public GameState gameState;
    [SerializeField] public UnityEvent onEnemyDestroy;
    [SerializeField] public Slider healthSlider;

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

    private float damageTimer = 0f;
    protected virtual void Awake()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        healthSlider.gameObject.SetActive(false);
    }

    protected virtual void Start()
    {
        GameObject levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        if (levelManager != null)
        {
            EnemySpawner enemySpawner = levelManager.GetComponent<EnemySpawner>();
            if (enemySpawner != null)
            {
                onEnemyDestroy.AddListener(enemySpawner.EnemyDestroyed);
                //Debug.Log("Listener added to onEnemyDestroy event.");
            }
            else
            {
                //Debug.LogError("EnemySpawner component not found on LevelManager.");
            }
        }
        else
        {
            //Debug.LogError("LevelManager not found.");
        }
    }

    private void Update()
    {
        damageTimer -= Time.deltaTime;
        if (damageTimer > 0)
        {
            
            if (damageTimer <= 0)
            {
                healthSlider.gameObject.SetActive(false);
            }
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            onEnemyDestroy?.Invoke();
            Destroy(gameObject);
        }

        healthSlider.value = health;
        healthSlider.gameObject.SetActive(true);
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
