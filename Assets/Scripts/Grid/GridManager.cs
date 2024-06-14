using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] public GridStats gridStats;

    // Start is called before the first frame update
    void Awake()
    {
        InitGridStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitGridStats()
    {

    }
}
