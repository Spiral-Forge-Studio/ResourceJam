using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    #region Variables

    private Camera _mainCamera;
    private RadialMenuController currentRadialMenu;
    public GameState gameState;
    private GameObject currentGameobject;
    private int towerLayerMask;

    #endregion

    private void Awake()
    {
        _mainCamera = Camera.main;
        // Assuming the "Tower" layer is named "Tower"
        towerLayerMask = LayerMask.GetMask("RadialCollider","Tower");
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started || gameState.IsPaused())
        {
            return;
        }

        var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        var rayHit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, towerLayerMask);

        if (rayHit.collider)
        {
            //Debug.Log("Name: " + rayHit.collider.gameObject.name + "Tag: " + rayHit.collider.gameObject.tag);

            if (rayHit.collider.gameObject.CompareTag("RadialCollider"))
            {
                return;
            }

            else if (rayHit.collider.gameObject.CompareTag("Tower"))
            {
                RadialMenuController radialMenu = rayHit.collider.gameObject.GetComponent<RadialMenuController>();

                if (currentRadialMenu != null && currentRadialMenu != radialMenu)
                {
                    currentRadialMenu.HideRadialMenu();
                }

                if (radialMenu != null)
                {
                    radialMenu.ShowRadialMenu();
                    currentRadialMenu = radialMenu;
                }
            }

        }
        else
        {
            if (currentRadialMenu != null)
            {
                currentRadialMenu.HideRadialMenu();
                currentRadialMenu = null;
            }
        }

    }
}
