using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimpleFlyerEnemy : FlyingEnemy
{
    [Header("References")]
    //[SerializeField] private GameObject flyerBullet;
    //[SerializeField] private LayerMask powerNodeMask;
    //[SerializeField] private LayerMask resourceNodeMask;
    //[SerializeField] private LayerMask hqMask;
    //[SerializeField] private Transform firePoint;

    [Header("Debug")]
    [SerializeField] private int combinedMask;

    private void Start()
    {
        //combinedMask = powerNodeMask | resourceNodeMask | hqMask;
    }

    private void Shoot(Transform miningTarget)
    {
        //raycast
    }

    //private void FindMiningNode()
    //{
    //    RaycastHit2D[] acquired = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, combinedMask);

    //    if (acquired.Length > 0)
    //    {
    //        target = acquired[0].transform;
    //        engagingTarget = false;
    //    }

    //}

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("ResourceNode"))
        {
            targetDead = false;

            ResourceNodeScript tempNode = collision.gameObject.GetComponent<ResourceNodeScript>();

            coroutineStarted = true;

            attackOrder = StartCoroutine(AttackNode(tempNode));
        }
    }
}

