using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public InventoryItem lore;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(lore.itemName);
    }

   
}