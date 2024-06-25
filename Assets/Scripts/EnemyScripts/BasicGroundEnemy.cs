using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicGroundEnemy : GroundEnemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PowerNode" && !coroutineStarted)
        {
            targetDead = false;

            PowerNodeScript tempNode = collision.gameObject.GetComponent<PowerNodeScript>();

            coroutineStarted = true;

            attackOrder = StartCoroutine(AttackNode(tempNode));
        }

        else if (collision.gameObject.tag == "Headquarters" && !coroutineStarted)
        {
            targetDead = false;

            Headquarters hq = collision.gameObject.GetComponent<Headquarters>();

            coroutineStarted = true;

            attackOrder = StartCoroutine(AttackNode(hq));
        }
    }
}
