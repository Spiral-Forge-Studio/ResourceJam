using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;

    [Header("Enemy Attributes")]
    [SerializeField] public bool isFlying;
    [SerializeField] public float maxHealth;
    public float health;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float damage;
    [SerializeField] public float range;
    [SerializeField] public float attackSpeed;
    [SerializeField] public string attackAnimation;
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

}
