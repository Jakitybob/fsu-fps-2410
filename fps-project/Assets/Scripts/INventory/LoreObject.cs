using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LoreObject : MonoBehaviour, IInteractable
{
    
    public bool isPickable = true;
    public InventoryItem inventoryItem;

    
    

    public void Interact(Interact interactor)
    {
        
        if (isPickable)
        {
            InventoryManager.Instance.Add(inventoryItem);
            //gameManager.instance.inventory.Add(inventoryItem);
            // Destroy the LoreObject
            Destroy(gameObject);  
        }
        
    } 
    
    

}
