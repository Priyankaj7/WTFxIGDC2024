using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMachine : MonoBehaviour,ICardItem
{

    float _earnRate;
    bool isBoosted;
    bool needsRepair;
    public float GetEarnRate()
    {
        return _earnRate;
    }

    public bool IsBoosted()
    {
       return isBoosted;
    }

    public bool NeedsRepair()
    {
        return needsRepair;
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
