using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeManager : MonoBehaviour
{

    [Header("Node List")]
    [SerializeField] public List<GameObject> nodes = new List<GameObject>();

    [Header("Scriptable Objects")]
    [SerializeField] public ResourceStats resourceStats;

    [Header("Adjiustable Variables")]
    [SerializeField] private float _minTickRate;

    [Header("[DEBUG] internal variables")]
    [SerializeField] private float _totalRPMFromNodes;
    [SerializeField] private float _totalResources;
    [SerializeField] private float _tickRate;
    [SerializeField] private float _elapsedTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private float _actualRPI;

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

        if (nodes.Count > 0)
        {
            foreach (GameObject node in nodes)
            {
                if (node.activeSelf && node != null)
                {
                    ResourceNodeScript resourceNode = node.GetComponent<ResourceNodeScript>();
                    _totalRPMFromNodes += resourceNode.getRPM();
                }
            }
        }

    }
    private void UpdateTotalResources()
    {
        _elapsedTime += Time.deltaTime;

        float _totalRPS = _totalRPMFromNodes / 60.0f;

        _tickRate = Mathf.Max(1/_totalRPS, 1/_minTickRate);

        _actualRPI = (_totalRPS * _tickRate);

        if (_elapsedTime >= _tickRate)
        {
            resourceStats.totalResources += (_totalRPS * _tickRate);
            
            _elapsedTime = 0;
        }
    }
}
