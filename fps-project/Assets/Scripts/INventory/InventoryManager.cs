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

        Debug.Log("Inventory item clicked: " + item.itemName);
        if (detailPanel == null)
        {
            detailPanel = Instantiate(detailViewPrefab);
            detailPanel.SetActive(true);
            /* DetailViewPanel detailViewPanelScript = detailPanel.GetComponent<DetailViewPanel>();
            detailViewPanelScript.SetData(item); */
        }
        DetailViewPanel detailPanelScript = detailPanel.GetComponent<DetailViewPanel>();
        detailPanelScript.SetData(item.itemName, item.itemDescription, item.health, item.Damage, item.itemIcon);
         
 
        Button closeButton = detailPanel.GetComponentInChildren<Button>();
        closeButton.onClick.AddListener(CloseDetailView);
    }

    public void CloseDetailView()
    {
        if (detailPanel != null)
        {
            Debug.Log("Close Button Clicked");

            // Ensure the DetailPanel is active before attempting to deactivate or destroy it
            if (detailPanel.activeSelf)
            {
                detailPanel.SetActive(false);
            }

            // Destroy the DetailPanel after a short delay to allow for potential UI updates
            Destroy(detailPanel, 0.1f);
            detailPanel = null;
        }




    }







}
