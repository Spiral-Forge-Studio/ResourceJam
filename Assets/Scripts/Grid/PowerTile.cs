using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("[BALANCING] Variables and References")]
    [SerializeField] private float _purchaseCost;
    [SerializeField] private float _upgradeCost;
    [SerializeField] private float _multiplier;
    [SerializeField] private float _additional;

    [Header("[REFERENCES]")]
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
        if (_nodeInplace == null)
        {
            _occupied = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.blocksRaycasts = true;
        }
    }


    #region GETTERS AND SETTERS

    public float GetPurchaseCost() => _purchaseCost;
    public float GetUpgradeCost() => _upgradeCost;
    public float GetMultiplier() => _multiplier;
    public float GetAdditional() => _additional;
    public bool isOccupied() => _occupied;
    public void SetOccupied(GameObject node)
    {
        _occupied = true;
        _nodeInplace = node;
    }

    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
    }
}
