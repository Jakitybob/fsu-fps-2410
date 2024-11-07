using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public int maxCapacity = 10;

    public void AddItem(InventoryItem item)
    {
        if (inventoryItems.Count < maxCapacity)
        {
            inventoryItems.Add(item);
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        inventoryItems.Remove(item);
    }

    public void DisplayInventory()
    {
        // Update the UI to display the current inventory items
        // This might involve creating UI elements for each item and updating their quantity and icon
    }
}