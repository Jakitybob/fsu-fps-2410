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
        PersistantInventory.Instance.ListItems();
    }

    public void Add(InventoryItem item)
    {
        Items.Add(item);

    }

    public void Remove(InventoryItem item)
    {
        Items.Remove(item);
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
            Debug.LogError("Clicked item is null!");
            return;
        }

        Debug.Log("Inventory item clicked: " + item.itemName);
        
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
            Debug.LogError("DetailViewPanel component not found on prefab!");
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
            Debug.Log("Close Button Clicked");
            Destroy(detailPanel);
            detailPanel = null;
        }
    }

}
