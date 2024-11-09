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

    public List<InventoryItem> Items = new List<InventoryItem>();



    private void Awake()
    {
        Instance = this;

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

            itemName.text = item.itemName;
            itemIcon.sprite = item.itemIcon;

        }
            
    }

    



}
