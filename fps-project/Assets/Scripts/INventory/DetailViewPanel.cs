using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailViewPanel : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescriptionText;
    [SerializeField] private Text itemHealthText;
    [SerializeField] private Text itemDamageText;
    [SerializeField] private Image itemImage;

    

    public void SetData(string itemName, string itemDescription, string health, string damage, Sprite icon)
    {
        itemNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemHealthText.text = "Health: " + health;
        itemDamageText.text = "Damage: " + damage;
        itemImage.sprite = icon;
    }
}
