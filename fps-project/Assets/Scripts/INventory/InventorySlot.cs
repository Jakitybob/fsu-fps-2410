using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InventorySlot : MonoBehaviour
{
    public InventoryItem item;

    public GameObject TooltipPrefab;
    private GameObject tooltipInstance;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the tooltip with a brief description
        ShowTooltip(item.itemDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the tooltip
        HideTooltip();
    }

    public void OnClicked()
    {
        // Show the full description in a popup
        ShowPopup(item.itemIcon);
    }

    private void ShowTooltip(string description)
    {
        if (tooltipInstance == null)
        {
            tooltipInstance = Instantiate(TooltipPrefab);
        }

        tooltipInstance.GetComponent<TMPro.TextMeshProUGUI>().text = description;
        //tooltipInstance.transform.position = Position;
    }

    private GameObject Instantiate(object tooltipPrefab)
    {
        throw new NotImplementedException();
    }

    private void HideTooltip()
    {
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
        }
    }
    private void ShowPopup(Sprite Story)
    {

    }
    
}
