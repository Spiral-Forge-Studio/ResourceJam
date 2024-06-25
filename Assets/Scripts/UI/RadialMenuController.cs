using UnityEngine;

public class RadialMenuController : MonoBehaviour
{
    public CanvasGroup radialMenuCanvasGroup;
    public Collider2D col;

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
        col.enabled = true;
        radialMenuCanvasGroup.alpha = 1;
        radialMenuCanvasGroup.interactable = true;
        radialMenuCanvasGroup.blocksRaycasts = true;
    }

    public void HideRadialMenu()
    {
        col.enabled = false;
        radialMenuCanvasGroup.alpha = 0;
        radialMenuCanvasGroup.interactable = false;
        radialMenuCanvasGroup.blocksRaycasts = false;
    }
}