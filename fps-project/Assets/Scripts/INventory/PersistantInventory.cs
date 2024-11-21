using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PersistantInventory : MonoBehaviour
{
    public static PersistantInventory Instance;
    public List<InventoryItem> Items = new List<InventoryItem>();
    private bool hasCleared = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Only clear data when first starting the game
        if (!hasCleared)
        {
            ClearSavedData();
            hasCleared = true;
        }
        LoadInventoryData();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SaveInventoryData();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadInventoryData();
        SyncWithInventoryManager();
    }

    private void SyncWithInventoryManager()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.Items = new List<InventoryItem>(Items);
            inventoryManager.ListItems();
        }
    }

    public void AddItem(InventoryItem item)
    {
        if (item == null) return;
        
        bool isKeyCard = item.GetType().IsSubclassOf(typeof(KeyCardItem)) || item.GetType() == typeof(KeyCardItem);
        
        if (!isKeyCard && !Items.Contains(item))
        {
            Items.Add(item);
            SaveInventoryData();
            SyncWithInventoryManager();
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        if (Items.Remove(item))
        {
            SaveInventoryData();
            SyncWithInventoryManager();
        }
    }

    public void SaveInventoryData()
    {
        var nonKeyCardItems = Items.Where(item => 
            !(item.GetType().IsSubclassOf(typeof(KeyCardItem)) || item.GetType() == typeof(KeyCardItem))
        ).ToList();

        var wrapper = new ItemListWrapper { items = nonKeyCardItems };
        string jsonData = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("InventoryData", jsonData);
        PlayerPrefs.Save();
    }

    public void LoadInventoryData()
    {
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string jsonData = PlayerPrefs.GetString("InventoryData");
            var wrapper = JsonUtility.FromJson<ItemListWrapper>(jsonData);
            if (wrapper != null && wrapper.items != null)
            {
                Items = wrapper.items;
            }
        }
    }

    public void ClearSavedData()
    {
        Items.Clear();
        PlayerPrefs.DeleteKey("InventoryData");
        PlayerPrefs.Save();
        SyncWithInventoryManager();
    }
}

[System.Serializable]
public class ItemListWrapper
{
    public List<InventoryItem> items;
}
