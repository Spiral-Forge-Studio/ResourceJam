using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceNodeManager : MonoBehaviour
{

    [Header("[BALANCING]")]
    [SerializeField] private float _minTickRate;
    [SerializeField] private float _initialResources;
    [SerializeField] private float _initialRPI;

    [Header("[REFERENCES]")]
    [SerializeField] public ResourceStats resourceStats;

    [Header("[DEBUG]")]
    [SerializeField] public List<GameObject> _nodes = new List<GameObject>();
    [SerializeField] private float _totalRPMFromNodes;
    [SerializeField] private float _totalResources;
    [SerializeField] private float _tickRate;
    [SerializeField] private float _elapsedTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private float _actualRPI;

    void Awake()
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
        resourceStats.SetTotalResources(_initialResources);
        resourceStats.SetTotalRPM(_initialRPI);
    }
    private void updateResourceStats()
    {
        resourceStats.nodes = _nodes;

        resourceStats.SetTotalRPM(_totalRPMFromNodes);
        _totalResources = resourceStats.GetTotalResources();
    }

    private void updateTotalRPM()
    {
        _totalRPMFromNodes = 0;

        if (_nodes.Count > 0)
        {
            foreach (GameObject node in _nodes)
            {
                if (!node.IsUnityNull())
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
            resourceStats.AddToTotalResources(_totalRPS * _tickRate);
            _elapsedTime = 0;
        }
    }
}
