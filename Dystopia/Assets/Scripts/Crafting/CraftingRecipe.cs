﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct ItemAmount {
    public Item item;
    [Range(1, 9999)]
    public int amount;
}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public List<ItemAmount> materials;
    public List<ItemAmount> results;

    public bool CanCraft(Inventory inventory) {
        foreach(ItemAmount itemAmount in materials) {
            if(inventory.ItemCount(itemAmount.item.ID) < itemAmount.amount) {
                return false; //there are not enough materials
            }
        }
        return true; //there are enough materials, we can craft it
    }

    public void Craft(Inventory inventory) {
        if(CanCraft(inventory)) {
            //Remove from the inventory every material used to craft
            foreach(ItemAmount itemAmount in materials) {
                for(int i = 0; i < itemAmount.amount; i++) {
                    /*
                    if(itemAmount.item.isTool == true) { //if it is a tool
                        itemAmount.item.DecreaseRemainUses(1); //One use
                        if(itemAmount.item.GetRemainUses()<=0) {
                            Item oldItem = inventory.RemoveItem(itemAmount.item.ID); //remove or decrease item amount
                            oldItem.Destroy();
                        }
                    } else { //if it is not a tool
                    */
                        Item oldItem = inventory.RemoveItem(itemAmount.item.ID, itemAmount.amount); //remove or decrease item amount
                        oldItem.Destroy();
                    /*
                    }
                    */
                }
            }
            //Add to the inventory every result from the craft
            foreach(ItemAmount itemAmount in results) {
                //for(int i = 0; i < itemAmount.amount; i++) {
                    inventory.AddItem(itemAmount.item.GetCopy(), itemAmount.amount);
                //}
            }
        }
    }

}
