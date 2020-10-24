using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CraftingPanel : MonoBehaviour
{
    [SerializeField] Transform craftingSlotsParent; //
    [SerializeField] ComponentSlot[] componentSlots; //Components slots (UI)
    [SerializeField] ResultSlot resultSlot; //Result slot (UI)

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
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

    public bool AddItem(Item item) {
        for(int i = 0; i < componentSlots.Length; i++) {
            // Check if (the slot is empty) or (it is the same ID and the amount in the slot is lower than the maximum stackable)
            if(componentSlots[i].item == null || (componentSlots[i].item.ID == item.ID && componentSlots[i].amount < item.MaximumStacks) ) {
                componentSlots[i].item = item; //set item in the slot
                componentSlots[i].amount++; //increase amount
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item) {
        for(int i = 0; i < componentSlots.Length; i++) {
            if(componentSlots[i].item == item) {
                componentSlots[i].amount--; //decrease amount
                return true;
            }
        }
        return false;
    }

    public Item RemoveItem(string itemID)
    {
        Item item = null;
        for(int i = 0; i < componentSlots.Length; i++) {
            item = componentSlots[i].item;
            if(item != null && item.ID == itemID) {
                componentSlots[i].amount--; //decrease amount
                return item;
            }
        }
        return null;
    }
}
