using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public int maxCapacity = 10;

    public GameObject inventoryUI; // Reference to the Inventory GameObject in the scene
    public GameObject inventorySlotPrefab; // Reference to the inventory slot prefab

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
        // Clear the inventory UI
        foreach (Transform child in inventoryUI.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate the inventory UI with items from the list
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItem item = inventoryItems[i];

        
        }
    }
    
}