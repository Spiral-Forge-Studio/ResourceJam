using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunshipFlyerEnemy : FlyingEnemy
{
    [Header("References")]
    [SerializeField] public ResourceStats resourceStats;
    [SerializeField] private LayerMask nodeMask;
    [SerializeField] private Transform[] firepoints;

    //[Header("Debug")]

    private void FixedUpdate()
    {
        fixedUpdateLogic();

        if (targetDead)
        {
            FindMiningNode();
        }
    }

    private void FindMiningNode()
    {
        RaycastHit2D[] acquired = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, 0f, nodeMask);

        if (acquired.Length < 1) return;

        for (int i = 0; i < acquired.Length; i++)
        {
            //Debug.Log(acquired[i].collider.gameObject.tag);

            if (acquired[i].collider.gameObject.CompareTag("ResourceNode"))
            {

                targetDead = false;

                ResourceNodeScript detectedTarget = acquired[i].collider.gameObject.GetComponent<ResourceNodeScript>();

                coroutineStarted = true;

                attackOrder = StartCoroutine(AttackNode(detectedTarget, firepoints));

                //Debug.Log("attacking node commenced");

                return;

            }
            else if (acquired[i].collider.gameObject.CompareTag("Headquarters"))
            {

                targetDead = false;

                Headquarters detectedTarget = acquired[i].collider.gameObject.GetComponent<Headquarters>();

                coroutineStarted = true;

                attackOrder = StartCoroutine(AttackNode(detectedTarget, firepoints));

                return;
            }
        }

    }

    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.yellow;
    //    Handles.DrawWireDisc(transform.position, transform.forward, range);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.green;
        if (target != null)
        {
            Gizmos.DrawLine(transform.position, transform.position + (target.position - transform.position).normalized * range);
        }
    }
}
