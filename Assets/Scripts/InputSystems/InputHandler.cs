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
    private int towerLayerMask;

    #endregion

    private void Awake()
    {
        _mainCamera = Camera.main;
        // Assuming the "Tower" layer is named "Tower"
        towerLayerMask = LayerMask.GetMask("Tower");
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        var rayHit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, towerLayerMask);

        if (rayHit.collider)
        {
            Debug.Log("Raycast hit: " + rayHit.collider.gameObject.name);
            if (rayHit.collider.gameObject.CompareTag("Tower"))
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
            else
            {
                if (currentRadialMenu != null)
                {
                    currentRadialMenu.HideRadialMenu();
                    currentRadialMenu = null;
                }
            }
        }
        //else
        //{
        //    if (currentRadialMenu != null)
        //    {
        //        currentRadialMenu.HideRadialMenu();
        //        currentRadialMenu = null;
        //    }
        //}

    }
}
