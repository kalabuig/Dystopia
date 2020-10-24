
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : ItemSlot
{
    public EquipmentType equipmentType; // Head, Body, Legs, Weapon...

/*
    private void OnValidate() { //Only works in editor mode
        gameObject.name = equipmentType.ToString() + "_Slot";
    }
*/

    public override bool CanReceiveItem(Item item) {
        if(item == null) return true; //If equipment slot is empty, can receive an item
        EquippableItem equippableItem = item as EquippableItem;
        return equippableItem != null && equippableItem.equipmentType == equipmentType; //If equipmentType match, can receive an item
    }
}
