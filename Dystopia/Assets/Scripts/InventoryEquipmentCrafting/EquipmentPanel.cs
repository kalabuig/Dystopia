using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent; //
    [SerializeField] EquipmentSlot[] equipmentSlots; //Equipment slots (UI)

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;
    public event Action OnItemChanged;

    private void OnValidate() { //Only works in editor mode
        if(equipmentSlotsParent!=null)
            equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    private void Start() {
        for(int i = 0; i < equipmentSlots.Length; i++) {
            equipmentSlots[i].OnPointerEnterEvent += OnPointerEnterEvent; //Subscribe to event (listening event)
            equipmentSlots[i].OnPointerExitEvent += OnPointerExitEvent; //Subscribe to event (listening event)
            equipmentSlots[i].OnRightClickEvent += OnRightClickEvent; //Subscribe to event (listening event)
            equipmentSlots[i].OnBeginDragEvent += OnBeginDragEvent; //Subscribe to event (listening event)
            equipmentSlots[i].OnEndDragEvent += OnEndDragEvent; //Subscribe to event (listening event)
            equipmentSlots[i].OnDragEvent += OnDragEvent; //Subscribe to event (listening event)
            equipmentSlots[i].OnDropEvent += OnDropEvent; //Subscribe to event (listening event)
            equipmentSlots[i].OnItemChanged += OnItemChanged;
        }
    }

    public bool AddItem(EquippableItem item, int amount, out EquippableItem previousItem, out int previousItemAmount) {
        for(int i = 0; i < equipmentSlots.Length; i++) {
            if(equipmentSlots[i].equipmentType == item.equipmentType) {
                previousItem = (EquippableItem)equipmentSlots[i].item;
                previousItemAmount = equipmentSlots[i].amount;
                equipmentSlots[i].item = item;
                equipmentSlots[i].amount = amount;
                return true;
            }
        }
        previousItem = null;
        previousItemAmount = 0;
        return false;
    }

    public bool RemoveItem(EquippableItem item) {
        for(int i = 0; i < equipmentSlots.Length; i++) {
            if(equipmentSlots[i].item == item) {
                equipmentSlots[i].item = null;
                return true;
            }
        }
        return false;
    }

    public List<EquippableItem> GetEquippedItems() {
        List<EquippableItem> itemsList = new List<EquippableItem>();
        foreach(EquipmentSlot slot in equipmentSlots) {
            if(slot != null && slot.item != null) {
                itemsList.Add((EquippableItem)slot.item);
            }
        }
        return itemsList;
    }

    public List<EquippableItem> GetEquippedItemsWithAmounts(out List<int> amounts) {
        List<EquippableItem> itemsList = new List<EquippableItem>();
        amounts = new List<int>();
        foreach(EquipmentSlot slot in equipmentSlots) {
            if(slot != null && slot.item != null) {
                itemsList.Add((EquippableItem)slot.item);
                amounts.Add(slot.amount);
            }
        }
        return itemsList;
    }

    public void EmptyInventory() {
        for(int i = 0; i < equipmentSlots.Length; i++) {
            equipmentSlots[i].item = null;
            equipmentSlots[i].amount = 0;
        }
    }
}
