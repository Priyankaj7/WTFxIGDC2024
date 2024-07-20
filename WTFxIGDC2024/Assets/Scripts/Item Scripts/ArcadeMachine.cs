using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMachine : MonoBehaviour,ICardItem
{
    [SerializeField] float _repairCost;
    [SerializeField] float _boostRate;
    [SerializeField] float _repairTimer;
    [SerializeField]float _earnRate;
    bool isBoosted;
    bool needsRepair;
    public float GetEarnRate()
    {
        if (!needsRepair)
        {
            return isBoosted ? _earnRate + _boostRate : _earnRate;
        }
        else
        {
            return 0f;
        }
    }

    public bool IsBoosted()
    {
       return isBoosted;
    }

    public bool NeedsRepair()
    {
        return needsRepair;
    }

    // Update is called once per frame
    void Update()
    {
        if(!needsRepair)
        _repairTimer-= Time.deltaTime;

        if (_repairTimer <= 0)
        {
            needsRepair = true;
        }
    }
}
