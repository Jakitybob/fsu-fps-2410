using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

            GameObject Slot = Instantiate(inventorySlotPrefab, inventoryUI.transform);

            // Get references to the slot's image and text components
            UnityEngine.UI.Image itemIcon = Slot.GetComponentInChildren<UnityEngine.UI.Image>();
            TextMeshProUGUI itemNameText = Slot.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI itemQuantityText = Slot.GetComponentInChildren<TextMeshProUGUI>();

            // Set the item icon, name, and quantity in the slot
            itemIcon.sprite = item.itemIcon;
            itemNameText.text = item.itemName;
            itemQuantityText.text = item.quantity.ToString();

        }
    }

}