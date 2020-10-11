using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

    private ItemSlot draggedSlot; //what slot we started with to drag

    private void Awake() {
        //Right Click (Equip / Unequip)
        inventory.OnRightClickEvent += Equip;
        equipmentPanel.OnRightClickEvent += Unequip;
        //Tooltip (Show / Hide)
        inventory.OnPointerEnterEvent += ShowToolTip;
        inventory.OnPointerExitEvent += HideToolTip;
        equipmentPanel.OnPointerEnterEvent += ShowToolTip;
        equipmentPanel.OnPointerExitEvent += HideToolTip;
        //Drag & Drop
        inventory.OnBeginDragEvent += BeginDrag;
        inventory.OnEndDragEvent += EndDrag;
        inventory.OnDragEvent += Drag;
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        equipmentPanel.OnDragEvent += Drag;
        equipmentPanel.OnDropEvent += Drop;
    }

    private void Equip(ItemSlot itemSlot) {
        EquippableItem equippableItem = itemSlot.item as EquippableItem;
        if(equippableItem != null) {
            Equip(equippableItem);
        }
    }

    private void Unequip(ItemSlot itemSlot) {
        EquippableItem equippableItem = itemSlot.item as EquippableItem;
        if(equippableItem != null) {
            Unequip(equippableItem);
        }
    }

    public void Equip(EquippableItem item) {
        if(inventory.RemoveItem(item)) { //Remove item from inventory
            EquippableItem previousItem;
            if(equipmentPanel.AddItem(item, out previousItem)) { //Add item to equipment panel
                if (previousItem != null) { //if it was something in that slot
                    inventory.AddItem(previousItem); //add the previous item to the inventory
                }
            } else { //if something goes wrong with addint the item to the equipment panel
                inventory.AddItem(item); //add item again to the inventory
            }
        }
    }

    public void Unequip(EquippableItem item) {
        //if inventory is not full, remove the item from equipment panel
        if(inventory.IsFull() == false && equipmentPanel.RemoveItem(item)) {
            inventory.AddItem(item); //Add item to inventory
        }
    }

    private void ShowToolTip(ItemSlot itemSlot) {
        if(itemSlot != null) {
            itemTooltip.ShowTooltip(itemSlot.item);
        }
    }

    private void HideToolTip(ItemSlot itemSlot) {
            itemTooltip.HideTooltip();
    }

    private void BeginDrag(ItemSlot itemSlot) {
        if(itemSlot.item != null) {
            draggedSlot = itemSlot; //remembering the item to drag
            //Setting the draggable temp item:
            draggableItem.sprite = itemSlot.item.icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(ItemSlot itemSlot) {
        draggedSlot = null; //don't remembre the item to drag (we are not dragging anymore)
        draggableItem.enabled = false; //disabble the draggable temp item
    }

    private void Drag(ItemSlot itemSlot) {
        if(draggableItem.enabled)
            draggableItem.transform.position = Input.mousePosition;
    }

    private void Drop(ItemSlot dropItemSlot) {
        //if destination slot can receive and original slot can receive
        if(dropItemSlot.CanReceiveItem(draggedSlot.item) && draggedSlot.CanReceiveItem(dropItemSlot.item)) {
            //Check if slots are from inventory or equipment
            EquippableItem dragItem = draggedSlot.item as EquippableItem;
            EquippableItem dropItem = dropItemSlot.item as EquippableItem;
            //if both items are in invenotry slots, swap them
            if(draggedSlot is EquipmentSlot == false && dropItemSlot is EquipmentSlot == false) {
                Item draggedItem = draggedSlot.item; //remember the dragged item
                draggedSlot.item = dropItemSlot.item;
                dropItemSlot.item = draggedItem;
            }
            else { //at least on slot is an equipment slot
                if(draggedSlot is EquipmentSlot) { //If origin is equipment slot
                    if(dragItem != null) Unequip(dragItem);
                    if(dropItem != null) Equip(dropItem);
                }
                if(dropItemSlot is EquipmentSlot) { //If destination is equipment slot
                    if(dragItem != null) Equip(dragItem);
                    if(dropItem != null) Unequip(dropItem);
                }
            }
            
        }
    }
}
