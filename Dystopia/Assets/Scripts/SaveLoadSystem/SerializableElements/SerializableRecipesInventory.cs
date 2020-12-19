using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableRecipesInventory
{
    public List<SerializableRecipe> recipes;

    public SerializableRecipesInventory(GameObject recipesInventory) {
        recipes = new List<SerializableRecipe>();
        DoSerialization(recipesInventory);
    }

    public void DoSerialization(GameObject recipesInventory) {
        CraftingBookPanel craftingBookPanelComponent = recipesInventory.GetComponent<CraftingBookPanel>();
        if(craftingBookPanelComponent!=null) {
            //Recipes in the recipes inventory:
           List<CraftingRecipe> craftingRecipes = craftingBookPanelComponent.knownCraftingRecipes;
            if(craftingRecipes!=null && craftingRecipes.Count>0) {
                foreach(CraftingRecipe recipe in craftingRecipes) {
                    if(recipe!=null && recipe.materials!=null && recipe.results!=null) {
                        SerializableRecipe serRecipe = new SerializableRecipe(recipe);
                        if(serRecipe!=null) {
                            recipes.Add(serRecipe);
                        }
                    }
                }
            }
        }
    }
}
