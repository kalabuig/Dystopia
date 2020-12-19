using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableInventory
{
    //Inventory items:
    public List<SerializableItem> items;

    public SerializableInventory(GameObject inventory) {
        items = new List<SerializableItem>();
        DoSerialization(inventory);
    }

    public void DoSerialization(GameObject inventory) {
        Inventory inventoryComponent = inventory.GetComponent<Inventory>();
        if(inventoryComponent!=null) {
            //Items in the inventory:
            ItemSlot[] itemSlots = inventoryComponent.ItemSlots;
            if(itemSlots!=null && itemSlots.Length>0) {
                foreach(ItemSlot itemSlot in itemSlots) {
                    if(itemSlot.item!=null && itemSlot.amount>0) {
                        items.Add(new SerializableItem(itemSlot.item, itemSlot.amount));
                    }
                }
            }
        }
    }
}
