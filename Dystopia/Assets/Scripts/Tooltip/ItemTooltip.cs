using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemAttributesText;

    public void ShowTooltip(Item item) {
        if(item==null) return;
        itemImage.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemAttributesText.text = "";
        if(item is EquippableItem) { //if it is an equippable item
            EquippableItem equippableItem = (EquippableItem)item;
            writeEquipmentType(equippableItem);
            if(equippableItem.warm != 0) writeAttribute("Warm", equippableItem.warm);
            if(equippableItem.protection != 0) writeAttribute("Protection", equippableItem.protection);
            if(equippableItem.damage != 0) writeAttribute("Damage", equippableItem.damage);
        }
        if(item is UsableItem) { //if it is a usable item
            writeEffects(item);
        }
        gameObject.SetActive(true);
    }

    private void writeEquipmentType(EquippableItem equippableItem) {
        itemAttributesText.text += string.Concat(
            "<color=#00FFFF>",
            equippableItem.equipmentType.ToString(),
            "</color>",
            "\n\n"
        );
    }

    private void writeAttribute(string attributeName, int attributeValue) {
        itemAttributesText.text += string.Concat(
            attributeValue > 0 ? "<color=#00FF00>+ ":"<color=#FF0000>- ",
            attributeValue.ToString(),
            " ",
            attributeName,
            "</color>",
            "\n"
        );
    }

    private void writeEffects(Item item) {
        UsableItem usableItem = (UsableItem)item;
        if(usableItem.IsConsumable) {
            itemAttributesText.text = string.Concat(
            "<color=#00FFFF>Consumable</color>",
            "\n\n"
        );
        //write effects
        foreach(UsableItemEffect effect in usableItem.listOfEffects) {
            itemAttributesText.text += string.Concat(
                effect.GetTooltipDescription(),
                "\n"
            );
        }
        
        }
    }

    public void HideTooltip() {
        gameObject.SetActive(false);
    }
}
