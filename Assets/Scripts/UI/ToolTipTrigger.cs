using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTipInfo toolTipInfo;
    public TowerStats towerStats;
    public GameObject powerNodePrefab;
    public GameObject resourceNodePrefab;


    private void Start()
    {
        toolTipInfo.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateInfo();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipInfo.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipInfo.gameObject.SetActive(false);
    }

    public void SetToolTipInfo(float resourceCost, float upkeepCost)
    {
        toolTipInfo._resourceCost.text = resourceCost.ToString();
        toolTipInfo._upkeepCost.text = upkeepCost.ToString();
    }

    public void UpdateInfo()
    {
        if (gameObject.name == "AutocannonIcon")
        {
            SetToolTipInfo(towerStats._autoCannonCost, towerStats._autoCannonUpkeep);
        }
        else if (gameObject.name == "SAMIcon")
        {
            SetToolTipInfo(towerStats._SAMCost, towerStats._SAMUpkeep);
        }
        else if (gameObject.name == "TeslaIcon")
        {
            SetToolTipInfo(towerStats._teslaCoilCost, towerStats._teslaCoilUpkeep);
        }
        else if (gameObject.name == "EarthquakeIcon")
        {
            SetToolTipInfo(towerStats._EarthquakeTowerCost, towerStats._EarthquakeTowerUpkeep);
        }
        else if (gameObject.name == "PowerIcon")
        {
            PowerNodeScript nodeScript = powerNodePrefab.GetComponent<PowerNodeScript>();
            SetToolTipInfo(nodeScript._cost, 0f);
            
        }
        else if (gameObject.name == "ResourceIcon")
        {
            ResourceNodeScript nodeScript = resourceNodePrefab.GetComponent<ResourceNodeScript>();
            SetToolTipInfo(nodeScript.cost, 0f);
        }

    }

}