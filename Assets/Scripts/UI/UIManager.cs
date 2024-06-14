using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] public UIStats UIStats;

    // Start is called before the first frame update

    private void Awake()
    {
        TryGetComponent<Canvas>(out UIStats.canvas);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
