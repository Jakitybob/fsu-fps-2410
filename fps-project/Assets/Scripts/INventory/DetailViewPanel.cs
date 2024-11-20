using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailViewPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemHealthText;
    [SerializeField] private TextMeshProUGUI itemDamageText;

    private void Awake()
    {
        // Ensure all required components are assigned
        if (itemNameText == null) itemNameText = transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();
        if (itemDescriptionText == null) itemDescriptionText = transform.Find("ItemDescription")?.GetComponent<TextMeshProUGUI>();
        if (itemHealthText == null) itemHealthText = transform.Find("ItemHealth")?.GetComponent<TextMeshProUGUI>();
        if (itemDamageText == null) itemDamageText = transform.Find("ItemDamage")?.GetComponent<TextMeshProUGUI>();
    }

    public void SetData(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogError("Attempted to set detail view data with null item!");
            return;
        }

        if (itemNameText) itemNameText.text = item.itemName;
        if (itemDescriptionText) itemDescriptionText.text = item.itemDescription;
        if (itemHealthText) itemHealthText.text = "Health: " + item.health;
        if (itemDamageText) itemDamageText.text = "Damage: " + item.Damage;
    }
}
