using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Firing : MonoBehaviour
{
    public SAM SAM;
    public LayerMask enemyMask;
    public GameObject missilePrefab;
    public float resultingDamage;
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
        
        //turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
        GameObject missileObj = Instantiate(missilePrefab, firePoint.position, Quaternion.identity);

        Sam_Missile samMissile = missileObj.GetComponent<Sam_Missile>();

        if (SAM.target!= null)
        {
            AudioManager.instance.PlaySFX("SAMFire");
            samMissile._damage = resultingDamage;
            samMissile.SamSetTarget(SAM.target);
        }
        else
        {
            Destroy(missileObj);
        }
    }

    public void SamShoot2()
    {
        
        //turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
        GameObject missileObj = Instantiate(missilePrefab, firePoint2.position, Quaternion.identity);

        Sam_Missile samMissile = missileObj.GetComponent<Sam_Missile>();

        if (SAM.target != null)
        {
            AudioManager.instance.PlaySFX("SAMFire");
            samMissile._damage = resultingDamage;
            samMissile.SamSetTarget(SAM.target);
        }
        else
        {
            Destroy(missileObj);
        }
    }

    public void SamShoot3()
    {
        
        //turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
        GameObject missileObj = Instantiate(missilePrefab, firePoint3.position, Quaternion.identity);

        Sam_Missile samMissile = missileObj.GetComponent<Sam_Missile>();

        if (SAM.target != null)
        {
            AudioManager.instance.PlaySFX("SAMFire");
            samMissile._damage = resultingDamage;
            samMissile.SamSetTarget(SAM.target);
        }
        else
        {
            Destroy(missileObj);
        }
    }
}
