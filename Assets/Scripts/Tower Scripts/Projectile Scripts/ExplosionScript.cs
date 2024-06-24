using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    
    void Awake()
    {
        GetComponent<Animator>().Play("Sam Explosion");
        AudioManager.instance.PlaySFX("Explosion");
        Destroy(gameObject, 2f);
    }
}
