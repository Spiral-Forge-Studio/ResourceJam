using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTile : MonoBehaviour
{
    [Header("Public Variables and References")]
    [SerializeField] public GridStats gridStats;

    [Header("[DEBUG] Private variables and objects")]
    [SerializeField] private GameObject _nodeInplace;
    [SerializeField] private bool _occupied;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private CanvasGroup canvasGroup;


    private void Awake()
    {
        _occupied = false;
    }

    private void Update()
    {
        if (_occupied)
        {
            checkStatus();
        }
    }

    private void checkStatus()
    {
        if (_nodeInplace.activeSelf == false)
        {
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void SetOccupied(GameObject node)
    {
        _occupied = true;
        _nodeInplace = node;
    }

    public bool isOccupied() => _occupied;
}
