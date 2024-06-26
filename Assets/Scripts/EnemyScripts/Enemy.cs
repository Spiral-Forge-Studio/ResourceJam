using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    #region references
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public GameState gameState;
    [SerializeField] public UnityEvent onEnemyDestroy;
    #endregion

    #region enemy attributes
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
    #endregion

    #region Materials
    [Header("Materials")]
    [SerializeField] public Material flashMaterial;
    [SerializeField] public Material originalMaterial;
    [SerializeField] private Coroutine flashRoutine;
    [SerializeField] public float flashDuration;
    #endregion

    [Header("DEBUG")]
    [SerializeField] private Coroutine slowRoutine;
    [SerializeField] public bool isDead;
    [SerializeField] public bool takingDamage;
    [SerializeField] public bool takingDamageAnim;

    private MaterialPropertyBlock _propertyBlock;
    private SpriteRenderer _spriteRenderer;
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    protected virtual void Awake()
    {
        isDead = false;
        health = maxHealth;
        takingDamageAnim = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _propertyBlock = new MaterialPropertyBlock();
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
                Debug.Log("Listener added to onEnemyDestroy event.");
            }
            else
            {
                Debug.LogError("EnemySpawner component not found on LevelManager.");
            }
        }
        else
        {
            Debug.LogError("LevelManager not found.");
        }

        originalMaterial = _spriteRenderer.material;
    }

    public void takeDamage(float damage)
    {
        takingDamage = true;
        health -= damage;

        Flash();
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(takingDamageAnimation());
    }

    protected virtual IEnumerator takingDamageAnimation()
    {
        Debug.Log("Starting takingDamageAnimation coroutine");

        _spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(flashDuration);

        _spriteRenderer.material = originalMaterial;

        flashRoutine = null;
        takingDamage = false;
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

    protected virtual IEnumerator slowDown(float percentage, float duration)
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
