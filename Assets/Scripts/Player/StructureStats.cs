using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="StructureStats", menuName ="StructureStats")]
public class StructureStats : ScriptableObject
{
    [Header("Tower Resource Prices")]
    [SerializeField] private float _autoCannonPrice;
    [SerializeField] private float _teslaCoilPrice;
    [SerializeField] private float _SAMPrice;
    [SerializeField] private float _EarthquakeTowerPrice;

    [Header("Tower Upkeep")]
    [SerializeField] private float _autoCannonUpkeep;
    [SerializeField] private float _teslaCoilUpkeep;
    [SerializeField] private float _SAMUpkeep;
    [SerializeField] private float _EarthquakeTowerUpkeep;


    // price, upkeep
    public float[] getAutoCannonCost()
    {
        return new float[] { _autoCannonPrice, _autoCannonUpkeep };
    }
    // price, upkeep
    public float[] getTeslaCoilCost()
    {
        return new float[] { _teslaCoilPrice, _teslaCoilUpkeep };
    }
    // price, upkeep
    public float[] getSAMCost()
    {
        return new float[] { _SAMPrice, _SAMUpkeep };
    }
    // price, upkeep
    public float[] getEarthquakeTowerCost()
    {
        return new float[] { _EarthquakeTowerPrice, _EarthquakeTowerUpkeep };
    }
}
