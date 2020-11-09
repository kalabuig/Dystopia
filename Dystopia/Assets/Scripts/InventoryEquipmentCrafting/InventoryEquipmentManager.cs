using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEquipmentManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] ScavengingInventory scavengingInventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] CraftingPanel craftingPanel;
    [SerializeField] CraftingBookPanel craftingBookPanel;
    [SerializeField] WaterFillerInventory waterFillerInventory;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;
    Character character;

    private ItemSlot draggedSlot; //what slot we started with to drag

    private void Awake() {
        character = GameObject.Find("Player")?.GetComponent<Character>(); //Get Character script from the player
        //Right Click (Equip / Unequip)
        inventory.OnRightClickEvent += InventoryRightClick;
        equipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
        craftingPanel.OnRightClickEvent += CraftingPanelRightClick;
        scavengingInventory.OnRightClickEvent += ScavengingPanelRightClick;
        waterFillerInventory.OnRightClickEvent += WaterFillerPanelRightClick;
        //Tooltip (Show / Hide)
        inventory.OnPointerEnterEvent += ShowToolTip;
        inventory.OnPointerExitEvent += HideToolTip;
        equipmentPanel.OnPointerEnterEvent += ShowToolTip;
        equipmentPanel.OnPointerExitEvent += HideToolTip;
        craftingPanel.OnPointerEnterEvent += ShowToolTip;
        craftingPanel.OnPointerExitEvent += HideToolTip;
        scavengingInventory.OnPointerEnterEvent += ShowToolTip;
        scavengingInventory.OnPointerExitEvent += HideToolTip;
        craftingBookPanel.OnPointerEnterEvent += ShowToolTip;
        craftingBookPanel.OnPointerExitEvent += HideToolTip;
        waterFillerInventory.OnPointerEnterEvent += ShowToolTip;
        waterFillerInventory.OnPointerExitEvent += HideToolTip;
        //Drag & Drop
        inventory.OnBeginDragEvent += BeginDrag;
        inventory.OnEndDragEvent += EndDrag;
        inventory.OnDragEvent += Drag;
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        equipmentPanel.OnDragEvent += Drag;
        equipmentPanel.OnDropEvent += Drop;
        craftingPanel.OnBeginDragEvent += BeginDrag;
        craftingPanel.OnEndDragEvent += EndDrag;
        craftingPanel.OnDragEvent += Drag;
        craftingPanel.OnDropEvent += Drop;
        scavengingInventory.OnBeginDragEvent += BeginDrag;
        scavengingInventory.OnEndDragEvent += EndDrag;
        scavengingInventory.OnDragEvent += Drag;
        scavengingInventory.OnDropEvent += Drop;
        waterFillerInventory.OnBeginDragEvent += BeginDrag;
        waterFillerInventory.OnEndDragEvent += EndDrag;
        waterFillerInventory.OnDragEvent += Drag;
        waterFillerInventory.OnDropEvent += Drop;
    }

    private void InventoryRightClick(ItemSlot itemSlot) {
        if(itemSlot.item is EquippableItem) {
            Equip((EquippableItem)itemSlot.item); //Equip item
        }
        else if(itemSlot.item is UsableItem) {
            UsableItem usableItem = (UsableItem)itemSlot.item;
            usableItem.Use(character); //Use item
            if(usableItem.IsConsumable) { //Remove item from inventory if it is a consumable item
                if(itemSlot.amount>1) {
                    itemSlot.amount--;
                } else {
                    Item subproduct = usableItem.GetSubProduct(); //get the subproduct after use this item
                    inventory.RemoveItem(usableItem);
                    usableItem.Destroy();
                    if(subproduct!=null) {
                        inventory.AddItem(subproduct); //Add the subproduct to the inventory
                    }
                }
            }
        }
    }

    private void EquipmentPanelRightClick(ItemSlot itemSlot) {
        if(itemSlot.item is EquippableItem) {
            Unequip((EquippableItem)itemSlot.item); //Unequip item
        }
    }

    private void CraftingPanelRightClick(ItemSlot itemSlot) {
        if(itemSlot!=null) {
            ItemFromCraftingToInventory(itemSlot.item); //Return item
        }
    }

    private void ScavengingPanelRightClick(ItemSlot itemSlot) {
        if(itemSlot!=null) {
            ItemFromScavengingToInventory(itemSlot.item); //Send item to inventory
        }
    }

    private void WaterFillerPanelRightClick(ItemSlot itemSlot) {
        if(itemSlot!=null) {
            ItemFromWaterFillerToInventory(itemSlot.item); //Send item to inventory
        }
    }

    private void ItemFromCraftingToInventory(Item item) {
        //if inventory is not full, remove the item from crafting panel
        if(inventory.IsFull() == false && craftingPanel.RemoveItem(item)) {
            inventory.AddItem(item); //Add item to inventory
        }
    }

    private void ItemFromScavengingToInventory(Item item) {
        //if inventory is not full, remove the item from scavenging panel
        if(inventory.IsFull() == false && scavengingInventory.RemoveItem(item)) {
            inventory.AddItem(item); //Add item to inventory
        }
    }

    private void ItemFromWaterFillerToInventory(Item item) {
        //if inventory is not full, remove the item from water filler panel
        if(inventory.IsFull() == false) {
            for(int i = 0; i < item.maxMultiUses; i++) {
                if(waterFillerInventory.RemoveItem(item)) { //remove item
                    inventory.AddItem(item); //Add item to inventory
                }
            }
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

    private void ShowToolTip(RecipeSlot recipeSlot) {
        if(recipeSlot != null) {
            itemTooltip.ShowTooltip(recipeSlot.item);
        }
    }

    private void HideToolTip(RecipeSlot recipeSlot) {
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
        if(draggedSlot == null) return;
        if(dropItemSlot == null) return;
        //Are the same items and are stackable? (and is not a tool item)
        if(dropItemSlot.item != null && dropItemSlot.item.MaximumStacks - dropItemSlot.amount > 0 && dropItemSlot.item.ID == draggedSlot.item.ID
            && dropItemSlot.item.isMultiUsable == false) {
            AddStacks(dropItemSlot);
        } //We can swap if destination slot can receive and original slot can receive
        else if(dropItemSlot.CanReceiveItem(draggedSlot.item) && draggedSlot.CanReceiveItem(dropItemSlot.item)) {
            SwapItems(dropItemSlot);
        }
    }

    private void AddStacks(ItemSlot dropItemSlot) {
        int howManyToAdd = Mathf.Min(dropItemSlot.item.MaximumStacks - dropItemSlot.amount, draggedSlot.amount); //check how many we can stack in this slot
        dropItemSlot.amount += howManyToAdd; //Add stacks until slot is full
        draggedSlot.amount -= howManyToAdd; //remove the moved items from the dragged slot (only the amount moved)
    }

    private void SwapItems(ItemSlot dropItemSlot) {
        //Check if slots are from inventory or equipment
        EquippableItem dragItem = draggedSlot.item as EquippableItem;
        EquippableItem dropItem = dropItemSlot.item as EquippableItem;
        //if both items are in invenotry slots, swap them
        if(draggedSlot is EquipmentSlot == false && dropItemSlot is EquipmentSlot == false) {
             //Temporary data needed to swap
            Item draggedItem = draggedSlot.item; //remember the dragged item
            int draggedItemAmount = draggedSlot.amount; //remember the amout
            //Swap:
            draggedSlot.item = dropItemSlot.item;
            draggedSlot.amount = dropItemSlot.amount;
            dropItemSlot.item = draggedItem;
            dropItemSlot.amount = draggedItemAmount;
        }
        else { //at least one slot is an equipment slot
            if(draggedSlot is EquipmentSlot) { //If origin is an equipment slot
                if(dragItem != null) Unequip(dragItem);
                if(dropItem != null) Equip(dropItem);
            }
            if(dropItemSlot is EquipmentSlot) { //If destination is an equipment slot
                if(dragItem != null) Equip(dragItem);
                if(dropItem != null) Unequip(dropItem);
            }
        }
    }
}
