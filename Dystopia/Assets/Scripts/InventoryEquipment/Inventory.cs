using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> startingItems; //Starting items in the inventory
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
        for(; i < startingItems.Count && i < itemSlots.Length; i++) {
            itemSlots[i].item = startingItems[i];
        }
        //Populate slots with empty
        for(; i < itemSlots.Length; i++) {
            itemSlots[i].item = null;
        }
    }

    public bool AddItem(Item item) {
        for(int i = 0; i < itemSlots.Length; i++) {
            if(itemSlots[i].item == null) {
                itemSlots[i].item = item;
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item) {
        for(int i = 0; i < itemSlots.Length; i++) {
            if(itemSlots[i].item == item) {
                itemSlots[i].item = null;
                return true;
            }
        }
        return false;
    }

    public bool IsFull() {
        for(int i = 0; i < itemSlots.Length; i++) {
            if(itemSlots[i].item == null) { //if there is one slot empty
                return false; //inventory is not full
            }
        }
        return true; //otherwise, inventory is full
    }
}
