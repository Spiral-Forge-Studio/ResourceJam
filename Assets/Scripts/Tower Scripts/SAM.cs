using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SAM : TowerParent
{
    [Header("References")]
    [SerializeField] private Transform turretRotation; //SAve this for sprite if needed
    //[SerializeField] private TowerStats towerStats;
    [SerializeField] private LayerMask enemyMask; // add a layer mask called flying enemy to detect it on raycast
    [SerializeField] private Transform firePoint;
    [SerializeField] public float rotationSpeed; //Save this for sprites if needed
    [SerializeField] private Firing firingTube; //Save this for sprites if needed
    [SerializeField] private Animator firingTubeAnimator; //Save this for sprites if needed
    [SerializeField] private string[] firingAnimations; //Save this for sprites if needed
    [SerializeField] private int animIndex; //Save this for sprites if needed
    [SerializeField] private string animState; //Save this for sprites if needed
    [SerializeField] private string idleAnimState; //Save this for sprites if needed
    

    public Transform target;
    private float timeToFire;

    protected override void Awake()
    {
        base.Awake();
        animIndex = 0;
        towerStats.SetSAM(this);
        firingTubeAnimator = turretRotation.GetComponentInChildren<Animator>();
        _modifiedUpkeep = _upkeepCost;
    }

    private void Start()
    {
        animState = ChangeAnimationsState(firingTubeAnimator, idleAnimState, idleAnimState);
    }


    protected override void Update()
    {
        base.Update();

        UpdateUpgradeRadialButtonState();
        UpdateDamage();
        UpdateFirerate();


        firingTube.resultingDamage = _damage;

        if (target == null)
        {
            SamFindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!SamCheckTargetInRange())
        {
            target = null;
            animState = ChangeAnimationsState(firingTubeAnimator, idleAnimState, animState);
        }
        else
        {
            timeToFire += Time.deltaTime;
            if (timeToFire >= 1f / _fireRate)
            {
                timeToFire = 0f;

                if (animIndex == firingAnimations.Length)
                {
                    animIndex = 0;
                }

                //Debug.Log("animState: " + animState + ", " + animIndex);
                animState = ChangeAnimationsState(firingTubeAnimator, firingAnimations[animIndex], idleAnimState);
                firingTube.SamShoot(animIndex);

                animIndex++;
                //turretRotation.GetComponentInChildren<Animator>().Play("SAMFiringMissile");
            }
        }
    }

    string ChangeAnimationsState(Animator anim, string newState, string currentState)
    {
        if (currentState == newState)
        {
            return currentState;
        }

        anim.Play(newState);

        return newState;
    }

    private void RotateTowardsTarget()
    {
        if (!_powerActive) return;

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        turretRotation.rotation = Quaternion.RotateTowards(turretRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private bool SamCheckTargetInRange()
    {
        return Vector2.Distance(target.position, firePoint.position) <= _range;
    }

    private void SamFindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(firePoint.position, _range, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            Enemy hitEnemy = hits[0].collider.gameObject.GetComponent<Enemy>();
            if (hitEnemy != null && !hitEnemy.isDead)
            {
                target = hits[0].transform;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(firePoint.position, _range);
    }
}
