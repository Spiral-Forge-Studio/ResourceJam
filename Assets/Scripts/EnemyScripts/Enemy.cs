using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public LayerMask nodeMask;

    [Header("Enemy Attributes")]
    [SerializeField] public bool isFlying;
    [SerializeField] public float health;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float damage;
    [SerializeField] public float range;
    [SerializeField] public float attackSpeed;
    [SerializeField] public string attackAnimation;


    public void AttackHQ(GameObject collidedObject)
    {
        //if (collision.gameObject.tag == "PowerNode" && !coroutineStarted)
        //{
        //    targetDead = false;

        //    PowerNodeScript tempNode = collision.gameObject.GetComponent<PowerNodeScript>();

        //    coroutineStarted = true;

        //    attackOrder = StartCoroutine(AttackNode(tempNode));
        //}
    }


}
