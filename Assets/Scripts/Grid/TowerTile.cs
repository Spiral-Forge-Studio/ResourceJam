using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private Collider2D _collider;


    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _occupied = false;
    }

    private void Update()
    {
        if (_occupied)
        {
            checkStatus();
        }
        else
        {
            _collider.enabled = true;
            _canvasGroup.blocksRaycasts = true;
        }
    }

    private void checkStatus()
    {
 
        if (_nodeInplace.IsDestroyed())
        {
            Debug.Log("Destroyed");
            Debug.Log("value: " + _nodeInplace);
            _occupied = false;
            _collider.enabled = false;
            _canvasGroup.blocksRaycasts = false;
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
