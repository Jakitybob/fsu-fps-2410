using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantInventory : MonoBehaviour
{
    public static PersistantInventory Instance;

    public List<InventoryItem> Items = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        // Load inventory data from a file or player preferences
        LoadInventoryData();
    }

    public void ListItems()
    {
        // Find the InventoryManager instance (or pass it as a reference)
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.Items = Items;
            inventoryManager.ListItems();
        }
    }

    public void SaveInventoryData()
    {
        // Implement serialization logic to save the Items list to a file or player preferences
        string jsonData = JsonUtility.ToJson(Items);
        PlayerPrefs.SetString("InventoryData", jsonData);
        PlayerPrefs.Save();
    }

    public void LoadInventoryData()
    {
        // Implement deserialization logic to load the Items list from a file or player preferences
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string jsonData = PlayerPrefs.GetString("InventoryData");
            Items = JsonUtility.FromJson<List<InventoryItem>>(jsonData);
        }
    }
}
