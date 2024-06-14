using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ResourceTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Public Variables and References")]
    [SerializeField] public GridStats gridStats;
    [SerializeField] public float _rpmMultiplier;
    [SerializeField] public float _rpmAdditional;

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

    public float getRPMMult() => _rpmMultiplier;
    public float getRPMAdd() => _rpmAdditional;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
        //Debug.Log("Entered");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
        //Debug.Log("Exited");
    }
}
