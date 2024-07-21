using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour, ICardItem
{
    [SerializeField] float _repairCost;
    [SerializeField] float _boostRate;
    [SerializeField] float _repairTimer;
    [SerializeField] float _earnRate;
    private bool isBoosted;
    bool needsRepair;
    public void BoostItem()
    {
        isBoosted = true;
    }

    public float GetEarnRate()
    {
        if (!needsRepair)
        {
            return /*this.isBoosted ? (_earnRate + _boostRate) :*/ _earnRate;
        }
        else
        {
            return 0f;
        }
    }

    public bool IsBoosted()
    {
        return this.isBoosted;
    }

    public bool NeedsRepair()
    {
        return needsRepair;
    }

    public void RepairMachine()
    {
        needsRepair = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
