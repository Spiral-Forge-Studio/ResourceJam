using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Firing : MonoBehaviour
{
    public SAM SAM;
    public LayerMask enemyMask;
    public GameObject missilePrefab;
    public float resultingDamage;
    public Transform[] firePoints;
    private Transform target;

    public void SamShoot(int index)
    {
        //turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
        GameObject missileObj = Instantiate(missilePrefab, firePoints[index].position, transform.rotation);

        if (!missileObj.IsUnityNull())
        {
            Sam_Missile samMissile = missileObj.GetComponent<Sam_Missile>();
            if (!SAM.target.IsUnityNull() && !missileObj.IsUnityNull())
            {
                samMissile._damage = resultingDamage;
                samMissile.SamSetTarget(SAM.target, SAM.transform.forward);
            }
            else
            {
                samMissile.FireStraight(SAM.transform.forward);
            }
        }
    }
}
