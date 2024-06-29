using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceStats", menuName = "ResourceStats")]
public class ResourceStats : ScriptableObject
{
    [Header("[DEBUG]")]
    [SerializeField] public List<GameObject> nodes; //NOTICE: Only input/access ResourceNodeScript object should be stored inside
    [SerializeField] private float _totalRPM;
    [SerializeField] private float _totalResources;
    [SerializeField] public bool _insufficientFunds;

    //call this when buying towers
    public bool SpendResources(float amount)
    {
        if (_totalResources - amount < 0)
        {
            //Debug.Log("Not enough money to spend");
            _insufficientFunds = true;
            return false;
        }
        else
        {
            _totalResources -= amount;
            _insufficientFunds = false;
            return true;
        }
    }

    //call this when selling towers
    public void AddToTotalResources(float amount)
    {
        _totalResources += amount;
    }

    #region GETTERS AND SETTERS

    public float GetTotalResources() => _totalResources;
    public float GetTotalRPM() => _totalRPM;

    public void SetTotalResources(float totalResources) => _totalResources = totalResources;
    public void SetTotalRPM(float totalRPM) => _totalRPM = totalRPM;

    #endregion
}
