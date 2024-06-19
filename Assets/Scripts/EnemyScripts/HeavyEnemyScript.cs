using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HeavyEnemyScript : GroundEnemy
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
    }
}
