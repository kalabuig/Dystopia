using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Investigation Panel
public class CraftingPanel : MonoBehaviour, IItemsContainer  
{
    [SerializeField] Transform craftingSlotsParent; //
    [SerializeField] public ComponentSlot[] componentSlots; //Components slots (UI)

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    private void Start() {
        for(int i = 0; i < componentSlots.Length; i++) {
            componentSlots[i].OnPointerEnterEvent += OnPointerEnterEvent; //Subscribe to event (listening event)
            componentSlots[i].OnPointerExitEvent += OnPointerExitEvent; //Subscribe to event (listening event)
            componentSlots[i].OnRightClickEvent += OnRightClickEvent; //Subscribe to event (listening event)
            componentSlots[i].OnBeginDragEvent += OnBeginDragEvent; //Subscribe to event (listening event)
            componentSlots[i].OnEndDragEvent += OnEndDragEvent; //Subscribe to event (listening event)
            componentSlots[i].OnDragEvent += OnDragEvent; //Subscribe to event (listening event)
            componentSlots[i].OnDropEvent += OnDropEvent; //Subscribe to event (listening event)
        }
        ConfigureSlots();
    }

    private void ConfigureSlots() {
        for(int i = 0; i < componentSlots.Length; i++) {
            componentSlots[i].item = null;
            componentSlots[i].amount = 0; //Setting the amount to 0
        }
    }

    public bool AddItem(Item item, int amount = 1) {
        if(item!=null) {
            if(item.isMultiUsable) { //can not be stackable and the amount is the % of its integrity
                for(int i = 0; i < componentSlots.Length; i++) {
                    if(componentSlots[i].item == null) {
                        componentSlots[i].item = item; //set item in the slot
                        componentSlots[i].amount = amount; //set amount
                        return true;
                    }
                }
            } else { //is not multiusable
                for(int i = 0; i < componentSlots.Length; i++) {
                    // Check if (the slot is empty) or (it is the same ID and the amount in the slot is lower than the maximum stackable and is not multiusable)
                    if(componentSlots[i].item == null) { // || (itemSlots[i].item.ID == item.ID && itemSlots[i].amount < item.MaximumStacks) ) {
                        componentSlots[i].item = item; //set item in the slot
                        componentSlots[i].amount = amount; //increase amount
                        return true;
                    }
                }
            }
        }
        return false;
        /*
        for(int i = 0; i < componentSlots.Length; i++) {
            // Check if (the slot is empty) or (it is the same ID and the amount in the slot is lower than the maximum stackable)
            if(componentSlots[i].item == null || (componentSlots[i].item.ID == item.ID && componentSlots[i].amount < item.MaximumStacks) ) {
                componentSlots[i].item = item; //set item in the slot
                componentSlots[i].amount += amount; //increase amount
                return true;
            }
        }
        return false;
        */
    }

    public bool RemoveItem(Item item, int amount = 1) {
        for(int i = 0; i < componentSlots.Length; i++) {
            if(componentSlots[i].item == item) {
                componentSlots[i].amount -= amount; //decrease amount
                return true;
            }
        }
        return false;
    }

    public Item RemoveItem(string itemID, int amount = 1)
    {
        Item item = null;
        for(int i = 0; i < componentSlots.Length; i++) {
            item = componentSlots[i].item;
            if(item != null && item.ID == itemID) {
                componentSlots[i].amount -= amount; //decrease amount
                return item;
            }
        }
        return null;
    }

    public int ItemCount(string itemID)
    {
        if(itemID==null) return 0;
        int counter = 0;
        for(int i = 0; i < componentSlots.Length; i++) {
            if(componentSlots[i]!=null && componentSlots[i].item!=null) {
                if(componentSlots[i].item.ID == itemID) {
                    counter++;
                }
            }
        }
        return counter;
    }

    public bool IsFull()
    {
        for(int i = 0; i < componentSlots.Length; i++) {
            if(componentSlots[i].item == null) { //if there is one slot empty
                return false; //craftin panel is not full
            }
        }
        return true; //otherwise, crafting panel is full
    }

    public int NumEmptySlots() {
        int numEmptySlots = 0;
        for(int i = 0; i < componentSlots.Length; i++) {
            if(componentSlots[i].item == null) { //if there is one slot empty
                numEmptySlots++;
            }
        }
        return numEmptySlots;
    }


    public void EmptyInventory() {
        for(int i = 0; i < componentSlots.Length; i++) {
            componentSlots[i].item = null;
            componentSlots[i].amount = 0;
        }
    }

}
