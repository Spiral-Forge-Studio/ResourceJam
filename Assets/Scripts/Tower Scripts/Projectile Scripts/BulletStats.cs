using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletStats", menuName = "BulletStats")]
public class BulletStats : ScriptableObject
{
    [Header("Auto Cannon")]
    [SerializeField] private float _autoCannonDamage;
    [SerializeField] private float _autoCannonBulletSpeed;

    [Header("Tesla Coil")]
    [SerializeField] private float _teslaCoilDamage;
    [SerializeField] private int _teslaCoilAmountToChain;

    [Header("SAM Specs")]
    [SerializeField] private float _SAMDamage;
    [SerializeField] private float _areaOfEffect;
    [SerializeField] private float _lifeSpan;

    [Header("SAM Missile Speed Controls")]
    [SerializeField] private float _initialSpeed;
    [SerializeField] private float _slowSpeed;
    [SerializeField] private float _finalSpeed;
    [SerializeField] private float _initialBurstDuration;
    [SerializeField] private float _slowDuration;
    [SerializeField] private float _accelerationDuration;

    [Header("SAM Missile Size Control")]
    [SerializeField] private Vector3 _initialScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 _finalScale = new Vector3(2f, 2f, 2f);



    public void SetAutoCannonBulletStats(Ammo autoCannonAmmo)
    {
        autoCannonAmmo._damage = _autoCannonDamage;
        autoCannonAmmo._bulletSpeed = _autoCannonBulletSpeed;
    }    
    
    public void SetTeslaBulletStats(ChainLightningScript teslaAmmo)
    {
        teslaAmmo._damage = _teslaCoilDamage;
        teslaAmmo.amountToChain = _teslaCoilAmountToChain;
    }    
    public void SetSAMMissileStats(Sam_Missile samMissile)
    {
        samMissile._damage = _SAMDamage;
        samMissile.areaOfEffect = _areaOfEffect;
        samMissile.initialSpeed = _initialSpeed;
        samMissile.initialBurstDuration = _initialBurstDuration;
        samMissile.slowSpeed = _slowSpeed;
        samMissile.slowDuration = _slowDuration;
        samMissile.finalSpeed = _finalSpeed;
        samMissile.accelerationDuration = _accelerationDuration;
        samMissile.lifeSpan = _lifeSpan;
        samMissile.initialScale = _initialScale;
        samMissile.finalScale = _finalScale;
    }
}
