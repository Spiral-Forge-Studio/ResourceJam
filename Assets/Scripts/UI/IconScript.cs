using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class IconScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("[SPECIFICATIONS]")]
    [SerializeField] public string placementTag;
    [SerializeField] public string resourceTileTag = "ResourceTile";
    [SerializeField] public string powerTileTag = "PowerTile";
    [SerializeField] public string towerTileTag = "TowerTile";

    [Header("[REFERENCES]")]
    public ResourceStats resourceStats;
    public PowerNodeStats powerNodeStats;
    public TowerStats towerStats;
    public GridStats gridStats;
    public UIStats uiStats;
    public GameObject structurePrefab;
    public Canvas canvas;
    public CanvasGroup canvasGroup;
    public Camera cam;

    [Header("[DEBUG] Variables and Objects")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject structureInstance;
    [SerializeField] private Vector2 originalPosition;
    [SerializeField] private string hitTag;
    [SerializeField] private LayerMask ogLayers;
    [SerializeField] private Collider2D structureCol;

    void Awake()
    {
        cam = Camera.main;
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
        structureCol = structureInstance.GetComponent<Collider2D>();
        structureCol.enabled = false;
        structureInstance.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.anchoredPosition = originalPosition;

        LayerMask mask = LayerMask.GetMask("Placements");

        var rayHit = Physics2D.GetRayIntersection(cam.ScreenPointToRay(Mouse.current.position.ReadValue()), 20, mask);

        if (rayHit.collider)
        {
            hitTag = rayHit.collider.tag;
            //Debug.Log("Name: " + rayHit.collider.gameObject.name + "Tag:" + hitTag);
            //Debug.Log(rayHit.collider.gameObject.tag + " ?= " + placementTag);

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
        structureCol.enabled = true;

        if (tag == "ResourceTile")
        {
            ResourceNodeScript node;
            ResourceTile tile;
            structurePrefab.TryGetComponent<ResourceNodeScript>(out node);
            targetTile.TryGetComponent<ResourceTile>(out tile);

            if (!tile.isOccupied())
            {
                AudioManager.instance.PlayInGameUISFX(1, 0.6f);
                resourceStats.nodes.Add(structurePrefab);
                resourceStats.SpendResources(node.cost);
                node.increaseAdditional(tile.getRPMAdd());
                node.increaseMultiplier(tile.getRPMMult());
                tile.SetOccupied(structurePrefab);
            }
            else
            {
                Destroy(structurePrefab);
            }
        }

        else if (tag == "PowerTile")
        {
            //Debug.Log("In Power Tile: " + structureInstance.name);
            PowerNodeScript node;
            PowerTile tile;

            structurePrefab.TryGetComponent<PowerNodeScript>(out node);
            targetTile.TryGetComponent<PowerTile>(out tile);

            if (!tile.isOccupied())
            {
                //Debug.Log("Occupied: " + structureInstance.name);
                AudioManager.instance.PlayInGameUISFX(0, 0.9f);
                powerNodeStats.powerNodes.Add(structurePrefab);
                resourceStats.SpendResources(node._cost);
                node.MultiplyMaxEnergy(tile.GetMultiplier());
                node.AddMaxEnergy(tile.GetAdditional());
                
                tile.SetOccupied(structurePrefab);
            }
            else
            {
                //Debug.Log("Destroying: " + structureInstance.name);
                Destroy(structurePrefab);
            }
        }

        else if (tag == "TowerTile")
        {
            TowerTile tile;
            targetTile.TryGetComponent<TowerTile>(out tile);

            if (!tile.isOccupied())
            {
                AudioManager.instance.PlayInGameUISFX(4);
                //Debug.Log("Adding tower thought Iconscript");

                towerStats.AddTower(structurePrefab);
                tile.SetOccupied(structurePrefab);
            }
            else 
            {
                //Debug.Log("Tower tile occupied");
                Destroy(structurePrefab); 
            }

        }
        else
        {
            //Debug.Log("No tag detected iconscript");
            Destroy(structurePrefab);
        }
    }

}
