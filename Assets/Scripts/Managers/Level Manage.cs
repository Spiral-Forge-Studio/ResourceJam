using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManage : MonoBehaviour
{
    public static LevelManage main;

    public Transform startPoint;
    public Transform[] Path;

    private void Awake()
    {
        main = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
