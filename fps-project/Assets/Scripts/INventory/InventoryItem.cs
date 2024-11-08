using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public int quantity;
}

