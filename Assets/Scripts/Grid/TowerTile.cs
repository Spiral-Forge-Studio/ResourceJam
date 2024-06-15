using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("[REFERENCES]")]
    
    [SerializeField] private GameObject _highlight;
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("[DEBUG]")]
    [SerializeField] private bool _occupied;
    [SerializeField] private GameObject _nodeInplace;


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
            _canvasGroup.blocksRaycasts = false;
        }
        else
        {
            _canvasGroup.blocksRaycasts = true;
        }
    }

    public void SetOccupied(GameObject node)
    {
        _occupied = true;
        _nodeInplace = node;
    }
    public bool isOccupied() => _occupied;

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
