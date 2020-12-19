using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableRecipe
{
    public List<SerializableItem> materials;
    public List<SerializableItem> results;

    public SerializableRecipe(CraftingRecipe craftingRecipe) {
        materials = new List<SerializableItem>();
        results = new List<SerializableItem>();
        DoSerialization(craftingRecipe);
    }

    public void DoSerialization(CraftingRecipe craftingRecipe) {
        if(craftingRecipe!=null) {
            //Materials
            foreach(ItemAmount itemAmount in craftingRecipe.materials) {
                if(itemAmount.item!=null && itemAmount.amount>0) {
                    materials.Add(new SerializableItem(itemAmount.item, itemAmount.amount));
                }
            }
            //Results
            foreach(ItemAmount itemAmount in craftingRecipe.results) {
                if(itemAmount.item!=null && itemAmount.amount>0) {
                    results.Add(new SerializableItem(itemAmount.item, itemAmount.amount));
                }
            }
        }
    }
}
