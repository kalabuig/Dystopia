using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CraftingBookPanel : MonoBehaviour
{
    public event Action<RecipeSlot> OnPointerEnterEvent;
    public event Action OnPointerExitEvent;

    [SerializeField] CraftingRecipeUI pfRecipeUI; //prefab of the recipe row
    [SerializeField] RectTransform recipeUIParent; //content object of the scroll view
    
    public List<CraftingRecipeUI> craftingRecipeUIs; //crafting recipe rows list
    
    private Inventory inventory; //player inventory

    public List<CraftingRecipe> knownCraftingRecipes; //list of known crafting recipes to show

    private void Start() {
        inventory = GameObject.Find("InventoryPanel")?.GetComponent<Inventory>();
        StartCraftingRecipeRowsList();
    }

    private void StartCraftingRecipeRowsList() {
        recipeUIParent.GetComponentsInChildren<CraftingRecipeUI>(includeInactive: true, result: craftingRecipeUIs);
        UpdateCraftingRecipeRowsList();
    }

    public void UpdateCraftingRecipeRowsList() { //Update the rows of recipes to show
        //foreach recipe to show
        for(int i = 0; i < knownCraftingRecipes.Count; i++) {
            if(craftingRecipeUIs.Count == i) { //if we are at the end of the list
                craftingRecipeUIs.Add(Instantiate(pfRecipeUI, recipeUIParent, false)); //add a new row
            } 
            craftingRecipeUIs[i].inventory = inventory; //assign inventory to the row
            craftingRecipeUIs[i].craftingRecipe = knownCraftingRecipes[i]; //assign recipe to the row
        }
        //if thereare more rows that recipes, set them to null (maybe this is not necessary to do)
        for(int i = knownCraftingRecipes.Count; i < craftingRecipeUIs.Count; i++) {
            craftingRecipeUIs[i].craftingRecipe = null; // null --> deactivate it
        }
    }
}
