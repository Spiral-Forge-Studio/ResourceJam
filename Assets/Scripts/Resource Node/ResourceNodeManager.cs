using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeManager : MonoBehaviour
{

    [Header("Node List")]
    [SerializeField] public ResourceNodeScript[] nodes;

    [Header("Resource Node Scriptable Object")]
    [SerializeField] public ResourceStats resourceStats;

    [Header("Adjiustable Variables")]
    [SerializeField] private float _minTickRate;

    [Header("[DEBUG] internal variables")]
    [SerializeField] private float _totalRPMFromNodes;
    [SerializeField] private int _totalResources;
    [SerializeField] private float _tickRate;
    [SerializeField] private float _elapsedTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private bool _printed = false;

    void Start()
    {
        InitializeNodeManager();
    }

    private void Update()
    {
        updateTotalRPM();
        UpdateTotalResources();
        updateResourceStats();
    }

    private void InitializeNodeManager()
    {
        foreach (ResourceNodeScript node in nodes)
        {
            node.gameObject.SetActive(false);
        }

        resourceStats.totalResources = 0;
        resourceStats.totalRPM = 0;
    }
    private void updateResourceStats()
    {
        resourceStats.nodes = nodes;

        resourceStats.totalRPM = _totalRPMFromNodes;
        _totalResources = resourceStats.totalResources;
    }

    private void updateTotalRPM()
    {
        _totalRPMFromNodes = 0;

        foreach (ResourceNodeScript node in nodes)
        {
            if (node.gameObject.activeSelf) _totalRPMFromNodes += node.getRPM();
        }
    }
    private void UpdateTotalResources()
    {
        _elapsedTime += Time.deltaTime;

        float _totalRPS = _totalRPMFromNodes / 60.0f;

        _tickRate = Mathf.Max(1/_totalRPS, 1/_minTickRate);

        if (_elapsedTime >= _tickRate)
        {
            resourceStats.totalResources += Mathf.CeilToInt(_totalRPS * _tickRate);
            
            _elapsedTime = 0;
        }

        if (Time.time >= 60 && _printed == false)
        {
            //Debug.Log("Time: " + Time.time + ", TotalResources: " + _totalResources + ", RPI: " + Mathf.CeilToInt(_totalRPS * _tickRate));
            _printed = true;
        }
    }
}
