using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////// DEPRECATED ////////
public class Crafting : MonoBehaviour
{
    private ComponentSlot[] componentSlots;
    //public ResultSlot[] resultSlots;
    public RecipeAssets recipeAssets;
    
    private List<CraftingRecipe> listCraftingRecipes;
    private Inventory inventory; 

    private void Awake() {
        componentSlots = GetComponent<CraftingPanel>()?.componentSlots;
        listCraftingRecipes = recipeAssets.GetCraftingRecipesList();
        inventory = GameObject.Find("InventoryPanel")?.GetComponent<Inventory>();
    }

    public void Craft() {
        //If there are no components don't do anything
        if(ComponentSlotsEmpty() == true) return;
        //Check crafting recipes
        foreach(CraftingRecipe cr in listCraftingRecipes) {
            //Check if all the materials of the recipe are in the component slots
            if(CheckAllComponents(cr)) { 
                //We have a candidate, Check if all the component slots have a component that is part of the recipe
                if(CheckAllMaterials(cr)) {
                    CreateItems(cr);//Create crafted item
                    return;
                }
            }
        }
    }

    private void CreateItems(CraftingRecipe cr) {
        //Check how many different items are we creating (how many slots we need to store it)
        int numItems = 0;
        foreach(ItemAmount itemAmount in cr.results) {
            if(itemAmount.item != null && itemAmount.amount != 0) {
                numItems++;
            }
        }
        if(numItems>inventory.NumEmptySlots()) return; //We don't have enough slots in the inventory to fill with the result items
        //Create items and store it in the inventory:
        int i = 0;
        foreach(ItemAmount itemAmount in cr.results) {
            for(int n=0; n<itemAmount.amount; n++) {
                inventory.AddItem(itemAmount.item.GetCopy());
            }
            i++;
        }
    }

    private bool CheckAllMaterials(CraftingRecipe cr) {
        foreach(ComponentSlot slot in componentSlots) {
            if(slot==null || slot.item == null || slot.amount == 0) continue;
            //Check if the component is in the recipe
            if(CheckMaterialInRecipe(slot.item, cr) == false) {
                return false; //At least one material is not in the recipe
            }
        } 
        return true; //all components and amounts are in the slots list
    }

    private bool CheckMaterialInRecipe(Item item, CraftingRecipe cr) {
        foreach(ItemAmount itemAmount in cr.materials) {
            if(itemAmount.item == null || itemAmount.amount == 0) continue;
            if(itemAmount.item == item) return true; // the material is in the recipe
        }
        return false;
    }

    private bool CheckAllComponents(CraftingRecipe cr) {
        foreach(ItemAmount itemAmount in cr.materials) {
            if(itemAmount.item != null && itemAmount.amount != 0) {
                //Check if this material is in any slot
                if(CheckComponentInAnySlot(itemAmount.item, itemAmount.amount) == false) {
                    return false; //At least one component is missing
                }
            }
        } 
        return true; //all components and amounts are in the slots list
    }

    private bool CheckComponentInAnySlot(Item item, int amount) {
        foreach(ComponentSlot slot in componentSlots) {
            if(slot==null) continue;
            if(slot.item == null || slot.amount==0) continue;
            if (slot.item.ID == item.ID && slot.amount >= amount) return true;
        }
        return false;
    }

    //Check if all the component slots are empty
    private bool ComponentSlotsEmpty() {
        foreach(ComponentSlot slot in componentSlots) {
            if(slot.item != null) {
                return false;
            }
        }
        return true;
    }

/*
    //Check if all the result slots are empty
    private bool ResultSlotsEmpty() {
        foreach(ResultSlot slot in resultSlots) {
            if(slot.item != null) {
                return false;
            }
        }
        return true;
    }
*/
}
