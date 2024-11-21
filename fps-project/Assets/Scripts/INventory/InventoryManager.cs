using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameObject detailViewPrefab;

    public List<InventoryItem> Items = new List<InventoryItem>();

    private GameObject detailPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Load items from PersistantInventory if it exists
        if (PersistantInventory.Instance != null)
        {
            Items = PersistantInventory.Instance.Items;
            ListItems();
        }
    }

    public void Add(InventoryItem item)
    {
        if (item == null || Items.Contains(item)) return;

        Items.Add(item);
        if (PersistantInventory.Instance != null)
        {
            PersistantInventory.Instance.AddItem(item);
        }
        ListItems();
    }

    public void Remove(InventoryItem item)
    {
        Items.Remove(item);
        if (PersistantInventory.Instance != null)
        {
            PersistantInventory.Instance.RemoveItem(item);
        }
        ListItems();
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);

            var itemName = obj.transform.Find("itemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("itemIcon").GetComponent<Image>();
            var itemButton = obj.GetComponent<Button>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.itemIcon;
            //onclick listener
            itemButton.onClick.AddListener(() => InventoryManager.Instance.OnInventoryItemClick(item));
        }
    }

    public void OnInventoryItemClick(InventoryItem item)
    {
        if (item == null)
        {
            return;
        }

        // If detail panel exists, update it. Otherwise, create a new one
        if (detailPanel == null)
        {
            detailPanel = Instantiate(detailViewPrefab);
            detailPanel.SetActive(true);
        }

        // Get and update the detail panel
        DetailViewPanel detailViewPanelScript = detailPanel.GetComponent<DetailViewPanel>();
        if (detailViewPanelScript != null)
        {
            detailViewPanelScript.SetData(item);
        }
        else
        {
            return;
        }

        // Setup close button
        Button closeButton = detailPanel.GetComponentInChildren<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners(); // Clear previous listeners
            closeButton.onClick.AddListener(CloseDetailView);
        }
    }

    public void CloseDetailView()
    {
        if (detailPanel != null)
        {
            Destroy(detailPanel);
            detailPanel = null;
        }
    }
}
