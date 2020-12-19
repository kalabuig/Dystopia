using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableCraftingInventory
{
    //Crafting Inventory items:
    public List<SerializableItem> items;

    public SerializableCraftingInventory(GameObject inventory) {
        items = new List<SerializableItem>();
        DoSerialization(inventory);
    }

    public void DoSerialization(GameObject inventory) {
        CraftingPanel craftingPanelComponent = inventory.GetComponent<CraftingPanel>();
        if(craftingPanelComponent!=null) {
            //Items in the inventory:
            ComponentSlot[] componentSlots = craftingPanelComponent.componentSlots;
            if(componentSlots!=null && componentSlots.Length>0) {
                foreach(ComponentSlot componentSlot in componentSlots) {
                    if(componentSlot.item!=null && componentSlot.amount>0) {
                        items.Add(new SerializableItem(componentSlot.item, componentSlot.amount));
                    }
                }
            }
        }
    }
}
