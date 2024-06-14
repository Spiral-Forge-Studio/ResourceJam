using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class IconScripts : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Specific Attributes")]
    [SerializeField]
    public string placementTag;

    [Header("References")]
    public ResourceStats resourceStats;
    public PowerNodeStats powerNodeStats;
    public GridStats gridStats;
    public GameObject structurePrefab;
    public Canvas canvas;
    public CanvasGroup canvasGroup;
    public Camera cam;

    [Header("[DEBUG] Variables and Objects")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject structureInstance;
    [SerializeField] private bool isOverGameArea;
    [SerializeField] private Vector2 originalPosition;
    [SerializeField] private string hitTag;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        // Create an instance of the turretPrefab
        structureInstance = Instantiate(structurePrefab);
        structureInstance.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // Update the position of the turretInstance
        Vector3 worldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        structureInstance.transform.position = worldPosition;
        structureInstance.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.anchoredPosition = originalPosition;

        var rayHit = Physics2D.GetRayIntersection(cam.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (rayHit.collider)
        {
            hitTag = rayHit.collider.tag;
            Debug.Log("Name: " + rayHit.collider.gameObject.name + "Tag:" + hitTag);

            if (rayHit.collider.CompareTag(placementTag))
            {
                Vector3 placementPosition = rayHit.collider.transform.position;
                placementPosition.z = 0;
                structureInstance.transform.position = placementPosition;
                applyPlacementProcess(rayHit.collider.gameObject, placementTag, structureInstance);
                
            }
            else { Destroy(structureInstance);}
        }
        else { Destroy(structureInstance);}
    }

    // IMPORTANT: for setting up placed object
    // need to hard code the implementations for towers and power nodes sorry :/
    public void applyPlacementProcess(GameObject targetTile, string tag, GameObject structurePrefab)
    {
        if (tag == "ResourcePlacement")
        {
            ResourceNodeScript node;
            ResourceTile tile;
            structurePrefab.TryGetComponent<ResourceNodeScript>(out node);
            targetTile.TryGetComponent<ResourceTile>(out tile);

            if (!tile.isOccupied())
            {
                resourceStats.nodes.Add(structurePrefab);
                node.increaseAdditional(tile.getRPMAdd());
                node.increaseMultiplier(tile.getRPMMult());
                tile.SetOccupied(structurePrefab);
            }
            else
            {
                Destroy(structurePrefab);
            }
        }

        else if (tag == "PowerNodePlacement")
        {
            PowerNodeScript node;
            PowerTile tile;
            structurePrefab.TryGetComponent<PowerNodeScript>(out node);
            targetTile.TryGetComponent<PowerTile>(out tile);

            if (!tile.isOccupied())
            {
                powerNodeStats.powerNodes.Add(structurePrefab);
                tile.SetOccupied(structurePrefab);
            }
            else
            {
                Destroy(structurePrefab);
            }
        }
    }

}
