using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Firing : MonoBehaviour
{
    public SAM SAM;
    public LayerMask enemyMask;
    public GameObject missilePrefab;
    public Transform firePoint;
    public Transform firePoint2;
    public Transform firePoint3;
    private Transform target;

    private void Update()
    {
        //RaycastHit2D[] hits = Physics2D.CircleCastAll(firePoint.position, 100f, (Vector2)transform.position, 0f, enemyMask);

        //if (hits.Length > 0)
        //{
        //    target = hits[0].transform;
        //}
    }
    public void SamShoot()
    {
        AudioManager.instance.PlaySFX(1);
        //turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
        GameObject missileObj = Instantiate(missilePrefab, firePoint.position, Quaternion.identity);

        Sam_Missile samMissile = missileObj.GetComponent<Sam_Missile>();

        if (SAM.target!= null)
        {
            samMissile.SamSetTarget(SAM.target);
        }
        else
        {
            Destroy(missileObj);
        }
    }

    public void SamShoot2()
    {
        AudioManager.instance.PlaySFX(2);
        //turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
        GameObject missileObj = Instantiate(missilePrefab, firePoint2.position, Quaternion.identity);

        Sam_Missile samMissile = missileObj.GetComponent<Sam_Missile>();

        if (SAM.target != null)
        {
            samMissile.SamSetTarget(SAM.target);
        }
        else
        {
            Destroy(missileObj);
        }
    }

    public void SamShoot3()
    {
        AudioManager.instance.PlaySFX(4);
        //turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
        GameObject missileObj = Instantiate(missilePrefab, firePoint3.position, Quaternion.identity);

        Sam_Missile samMissile = missileObj.GetComponent<Sam_Missile>();

        if (SAM.target != null)
        {
            samMissile.SamSetTarget(SAM.target);
        }
        else
        {
            Destroy(missileObj);
        }
    }
}
