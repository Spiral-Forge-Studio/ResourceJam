using UnityEngine;

public class RadialMenuController : MonoBehaviour
{
    public CanvasGroup radialMenuCanvasGroup;

    private void Start()
    {
        HideRadialMenu();
    }

    public void ShowRadialMenu()
    {
        radialMenuCanvasGroup.alpha = 1;
        radialMenuCanvasGroup.interactable = true;
        radialMenuCanvasGroup.blocksRaycasts = true;
    }

    public void HideRadialMenu()
    {
        radialMenuCanvasGroup.alpha = 0;
        radialMenuCanvasGroup.interactable = false;
        radialMenuCanvasGroup.blocksRaycasts = false;
    }
}