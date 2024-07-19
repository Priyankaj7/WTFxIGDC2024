using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardItem 
{
    bool NeedsRepair();

    bool IsBoosted();
    float GetEarnRate();
}
