using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;

    [Header("Enemy Attributes")]
    [SerializeField] public bool isFlying;
    [SerializeField] public float health;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float damage;
    [SerializeField] public float range;
    [SerializeField] public float attackSpeed;
    [SerializeField] public string attackAnimation;
}
