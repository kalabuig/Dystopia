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
        }
    }

    public bool AddItem(EquippableItem item, out EquippableItem previousItem) {
        for(int i = 0; i < equipmentSlots.Length; i++) {
            if(equipmentSlots[i].equipmentType == item.equipmentType) {
                previousItem = (EquippableItem)equipmentSlots[i].item;
                equipmentSlots[i].item = item;
                return true;
            }
        }
        previousItem = null;
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
}
