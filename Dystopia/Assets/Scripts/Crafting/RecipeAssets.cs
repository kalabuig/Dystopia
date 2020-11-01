using System.Collections.Generic;
using UnityEngine;

public class RecipeAssets : GenericSingletonClass<RecipeAssets>
{
    //Here the list of recipes to craft obejcts in the game
    [SerializeField] private List<CraftingRecipe> craftingRecipes;

    public List<CraftingRecipe> GetCraftingRecipesList(){
        return craftingRecipes;
    }
}
