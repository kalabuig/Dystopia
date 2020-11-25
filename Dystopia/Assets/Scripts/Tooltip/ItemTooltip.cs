using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
            //Foreach modifier
            foreach (StatsModifiers.Modifier modifier in Enum.GetValues(typeof(StatsModifiers.Modifier))) {
                object obj = StatsModifiers.GetFieldValue(equippableItem, StatsModifiers.EnumToString(modifier));
                int intValue = 0;
                if(Int32.TryParse(obj.ToString(), out intValue)) {
                    if(intValue!=0) writeAttribute(EnumNameToNiceName(StatsModifiers.EnumToString(modifier)), intValue);
                }
            }
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

    private string EnumNameToNiceName(string enumName) {
        switch(enumName) {
            case("damage"):
                return "Damage";
            case("criticalChance"):
                return "Critical Chance";
            case("moveSpeed"):
                return "Movement Speed";
            case("protection"):
                return "Defense";
            case("craftSpeed"):
                return "Crafting Speed";
            case("investigationSpeed"):
                return "Investigation Speed";
            case("scavengingSpeed"):
                return "Scavenging Speed";
            case("fillWaterSpeed"):
                return "Water Filling Speed";
            case("useFireSpeed"):
                return "Fire Use Speed";
            case("warm"):
                return "Warm";
            default:
                return enumName;
        }
    }

    private void writeAttribute(string attributeName, int attributeValue) {
        itemAttributesText.text += string.Concat(
            attributeValue > 0 ? "<color=#00FF00>+ ":"<color=#FF0000>- ",
            Math.Abs(attributeValue).ToString(),
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
