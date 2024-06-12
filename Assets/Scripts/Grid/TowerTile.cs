using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _highlight;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
        Debug.Log("Entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
        Debug.Log("Exited");
    }
}
