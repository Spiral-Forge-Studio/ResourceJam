using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text upgradeDisplayInfo;
    public GameObject bg;
    public float cost;

    private void Start()
    {
        bg.gameObject.SetActive(false);
    }

    private void Update()
    {
        upgradeDisplayInfo.text = cost.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bg.SetActive(true);
        Debug.Log("Entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bg.SetActive(false);
        Debug.Log("Exited");
    }
}
