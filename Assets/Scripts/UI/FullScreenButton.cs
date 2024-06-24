using UnityEngine;
using UnityEngine.UI;

public class FullScreenClickHandler : MonoBehaviour
{
    public RadialMenuController radialMenuController;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(HideRadialMenu);
    }

    private void HideRadialMenu()
    {
        radialMenuController.HideRadialMenu();
        gameObject.SetActive(false); // Hide the full-screen button after click
    }
}
