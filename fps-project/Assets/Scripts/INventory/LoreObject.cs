using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LoreObject : MonoBehaviour, IInteractable
{
    public string loreText;
    public GameObject lorePanel;
    public TMPro.TextMeshProUGUI loreTextDisplay;
    public bool isPickable = true;
    public InventoryItem inventoryItem;

    private float displayTime = 5f;
    

    public void Interact(Interact interactor)
    {
        lorePanel.SetActive(true);
        loreTextDisplay.text = loreText;
        StartCoroutine(ClosePanelAfterTime());
        if (isPickable)
        {
            gameManager.instance.inventory.AddItem(inventoryItem);
            // Destroy the LoreObject
              
        }
        gameObject.SetActive(false);
    } 
    
    private IEnumerator ClosePanelAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        lorePanel.SetActive(false);
        loreTextDisplay.text = "";
    }

}
