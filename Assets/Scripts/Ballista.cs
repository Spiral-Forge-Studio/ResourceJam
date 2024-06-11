using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Ballista : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotation;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float tartgetInRange = 10f;
    [SerializeField] private float rotationSpeed = 2.0f;

    private Transform target;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetInRange()){
            target = null;
        }
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(target.position, turretRotation.position) <= tartgetInRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotation.rotation = Quaternion.RotateTowards(turretRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(turretRotation.position, tartgetInRange, (Vector2)transform.position, 0f,enemyMask );

        if (hits.Length > 0) { 
            target = hits[0].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(turretRotation.position, turretRotation.forward, tartgetInRange);
    }
}
