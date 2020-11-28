using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBook : MonoBehaviour, IItemsContainer
{
    [SerializeField] Transform itemsParent; //GridLayout (UI)
    [SerializeField] protected ItemSlot[] itemSlots; //Item Slots (UI)

        public bool AddItem(Item item, int amount = 1) {
        if(item!=null) {
            for(int i = 0; i < itemSlots.Length; i++) {
                // Check if (the slot is empty) or (it is the same ID and the amount in the slot is lower than the maximum stackable)
                if(itemSlots[i].item == null || (itemSlots[i].item.ID == item.ID && itemSlots[i].amount < item.MaximumStacks) ) {
                    itemSlots[i].item = item; //set item in the slot
                    itemSlots[i].amount++; //increase amount
                    return true;
                }
            }
        }
        return false;
    }

    public bool RemoveItem(Item item, int amount = 1) {
        for(int i = 0; i < itemSlots.Length; i++) {
            if(itemSlots[i].item == item) {
                itemSlots[i].amount -= amount; //decrease amount
                return true;
            }
        }
        return false;
    }

    public Item RemoveItem(string itemID, int amount = 1)
    {
        Item item = null;
        for(int i = 0; i < itemSlots.Length; i++) {
            item = itemSlots[i].item;
            if(item != null && item.ID == itemID) {
                itemSlots[i].amount -= amount; //decrease amount
                return item;
            }
        }
        return null;
    }

    public bool IsFull() {
        for(int i = 0; i < itemSlots.Length; i++) {
            if(itemSlots[i].item == null) { //if there is one slot empty
                return false; //inventory is not full
            }
        }
        return true; //otherwise, inventory is full
    }

    public int NumEmptySlots() {
        int numEmptySlots = 0;
        for(int i = 0; i < itemSlots.Length; i++) {
            if(itemSlots[i].item == null) { //if there is one slot empty
                numEmptySlots++;
            }
        }
        return numEmptySlots;
    }

    public int ItemCount(string itemID)
    {
        int counter = 0;
        for(int i = 0; i < itemSlots.Length; i++) {
            if(itemSlots[i].item.ID == itemID) {
                counter++;
            }
        }
        return counter;
    }
}
