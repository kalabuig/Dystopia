using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableEquipmentInventory
{
    //Equipment Inventory items:
    public List<SerializableItem> items;

    public SerializableEquipmentInventory(GameObject equipmentInventory) {
        items = new List<SerializableItem>();
        DoSerialization(equipmentInventory);
    }

    public void DoSerialization(GameObject equipmentInventory) {
        EquipmentPanel equipmentPanelComponent = equipmentInventory.GetComponent<EquipmentPanel>();
        if(equipmentPanelComponent!=null) {
            //Items in the equipment inventory:
            List<EquippableItem> equippedItems = equipmentPanelComponent.GetEquippedItems();
            foreach(Item item in equippedItems) {
                if(item!=null) {
                    items.Add(new SerializableItem(item, 1));
                }
            }
        }
    }
}
