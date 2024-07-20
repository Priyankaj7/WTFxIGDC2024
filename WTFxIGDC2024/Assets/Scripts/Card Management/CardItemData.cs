using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Card Item Data")]
public class CardItemData : ScriptableObject
{
    public ItemData[] AllItems;
   
}
[Serializable]
public class ItemData
{
   public string Name;
    public string Description;
    public float Cost;
    public Sprite Icon;
}
