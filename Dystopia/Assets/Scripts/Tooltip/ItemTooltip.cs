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
            if(equippableItem.warm != 0) writeAttribute("Warm", equippableItem.warm);
            if(equippableItem.protection != 0) writeAttribute("Protection", equippableItem.protection);
            if(equippableItem.damage != 0) writeAttribute("Damage", equippableItem.damage);
        }
        gameObject.SetActive(true);
    }

    private void writeAttribute(string attributeName, int attributeValue) {
        itemAttributesText.text = string.Concat(
            itemAttributesText.text,
            attributeValue > 0 ? "+ " : "- ",
            attributeValue.ToString(),
            " ",
            attributeName,
            "\n"
        );
    }

    public void HideTooltip() {
        gameObject.SetActive(false);
    }
}
