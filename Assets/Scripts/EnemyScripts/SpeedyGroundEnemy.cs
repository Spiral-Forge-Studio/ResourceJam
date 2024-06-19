using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedyGroundEnemy : GroundEnemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Headquarters" && !coroutineStarted)
        {
            targetDead = false;

            Headquarters hq = collision.gameObject.GetComponent<Headquarters>();

            coroutineStarted = true;

            attackOrder = StartCoroutine(AttackNode(hq));
        }
    }
}
