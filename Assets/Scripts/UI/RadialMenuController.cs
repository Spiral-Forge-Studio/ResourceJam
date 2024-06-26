using UnityEngine;

public class RadialMenuController : MonoBehaviour
{
    public CanvasGroup radialMenuCanvasGroup;
    public Collider2D col;
    public GameObject _rangeRadius;

    private void Awake()
    {
        col.enabled = false;
    }

    private void Start()
    {
        HideRadialMenu();
    }

    public void ShowRadialMenu()
    {
        _rangeRadius.SetActive(true);
        col.enabled = true;
        radialMenuCanvasGroup.alpha = 1;
        radialMenuCanvasGroup.interactable = true;
        radialMenuCanvasGroup.blocksRaycasts = true;
    }

    public void HideRadialMenu()
    {
        _rangeRadius.SetActive(false);
        col.enabled = false;
        radialMenuCanvasGroup.alpha = 0;
        radialMenuCanvasGroup.interactable = false;
        radialMenuCanvasGroup.blocksRaycasts = false;
    }
}