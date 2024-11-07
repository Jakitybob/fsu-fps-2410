using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreObject : MonoBehaviour, IInteractable
{
    public string loreText;
    public GameObject lorePanel;
    public TMPro.TextMeshProUGUI loreTextDisplay;
    public bool isPickable = true;

    private float displayTime = 5f;

    public void Interact(Interact interactor)
    {
        
        
        // Display lore text using a UI panel
        lorePanel.SetActive(true);
        loreTextDisplay.text = loreText;
        StartCoroutine(ClosePanelAfterTime());
        
    }
    private IEnumerator ClosePanelAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        lorePanel.SetActive(false);
        loreTextDisplay.text = "";
    }

}
