using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour, IItemsContainer
{
    [SerializeField] Item[] startingItems; //Starting items in the inventory
    [SerializeField] Transform itemsParent; //GridLayout (UI)
    [SerializeField] ItemSlot[] itemSlots; //Item Slots (UI)

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    private void Start() {
        for(int i = 0; i < itemSlots.Length; i++) {
            itemSlots[i].OnPointerEnterEvent += OnPointerEnterEvent; //Subscribe to event (listening event)
            itemSlots[i].OnPointerExitEvent += OnPointerExitEvent; //Subscribe to event (listening event)
            itemSlots[i].OnRightClickEvent += OnRightClickEvent; //Subscribe to event (listening event)
            itemSlots[i].OnBeginDragEvent += OnBeginDragEvent; //Subscribe to event (listening event)
            itemSlots[i].OnEndDragEvent += OnEndDragEvent; //Subscribe to event (listening event)
            itemSlots[i].OnDragEvent += OnDragEvent; //Subscribe to event (listening event)
            itemSlots[i].OnDropEvent += OnDropEvent; //Subscribe to event (listening event)
        }
        SetStartingItems(); // to set to null all the slots (disable image)
    }

    private void OnValidate() { //Only works in editor mode
        if(itemsParent != null) 
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
    }

    private void SetStartingItems() {
        int i = 0;
        //Populate slots with items
        for(; i < startingItems.Length && i < itemSlots.Length; i++) {
            itemSlots[i].item = startingItems[i].GetCopy(); //Instantiate a new item based on the scriptobject
            itemSlots[i].amount = 1; //Setting the amount to 1
        }
        //Populate slots with empty
        for(; i < itemSlots.Length; i++) {
            itemSlots[i].item = null;
            itemSlots[i].amount = 0; //Setting the amount to 0
        }
    }

    public bool AddItem(Item item) {
        for(int i = 0; i < itemSlots.Length; i++) {
            // Check if (the slot is empty) or (it is the same ID and the amount in the slot is lower than the maximum stackable)
            if(itemSlots[i].item == null || (itemSlots[i].item.ID == item.ID && itemSlots[i].amount < item.MaximumStacks) ) {
                itemSlots[i].item = item; //set item in the slot
                itemSlots[i].amount++; //increase amount
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item) {
        for(int i = 0; i < itemSlots.Length; i++) {
            if(itemSlots[i].item == item) {
                itemSlots[i].amount--; //decrease amount
                return true;
            }
        }
        return false;
    }

    public Item RemoveItem(string itemID)
    {
        Item item = null;
        for(int i = 0; i < itemSlots.Length; i++) {
            item = itemSlots[i].item;
            if(item != null && item.ID == itemID) {
                itemSlots[i].amount--; //decrease amount
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
