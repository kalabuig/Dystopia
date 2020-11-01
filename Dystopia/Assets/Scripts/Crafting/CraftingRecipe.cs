using System.Collections;
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

/*
    public bool CanCraft(IItemsContainer itemsContainer) {
        foreach(ItemAmout itemAmout in materials) {
            if(itemsContainer.ItemCount(itemAmout.item.ID) < itemAmout.amount) {
                return false; //there are not enough materials
            }
        }
        return true; //there are enough materials, we can craft it
    }

    public void Craft(IItemsContainer itemsContainer) {
        if(CanCraft(itemsContainer)) {
            //Remove from the container every material used to craft
            foreach(ItemAmout itemAmout in materials) {
                for(int i = 0; i < itemAmout.amount; i++) {
                    Item oldItem = itemsContainer.RemoveItem(itemAmout.item.ID);
                    oldItem.Destroy();
                }
            }
            //Add to the container every result from the craft
            foreach(ItemAmout itemAmout in results) {
                for(int i = 0; i < itemAmout.amount; i++) {
                    itemsContainer.AddItem(itemAmout.item.GetCopy());
                    
                }
            }
        }
    }
*/

}
