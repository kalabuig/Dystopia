using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CraftingRecipeUI : MonoBehaviour
{
    public Action<RecipeSlot> OnPointerEnterEvent;
    public Action<RecipeSlot> OnPointerExitEvent;

    //[SerializeField] RectTransform craftButtonTransform;
    [SerializeField] RecipeSlot[] recipeSlots; //slots of the recipe row

    [Space]
    [Header("Autopopulated fields")]
    public Inventory inventory; //player inventory (crafted items destination)

    private CraftingRecipe _craftingRecipe;
    public CraftingRecipe craftingRecipe {
        get { return _craftingRecipe; }
        set { SetCraftingRecipe(value); }
    }

    private void Awake() {
        recipeSlots = GetComponentsInChildren<RecipeSlot>(includeInactive: true);
    }

    private void Start() {
        foreach(RecipeSlot recipeSlot in recipeSlots) {
            //Tooltip
            recipeSlot.OnPointerEnterEvent += OnPointerEnterEvent; //Suscribe
            recipeSlot.OnPointerExitEvent += OnPointerExitEvent; //Suscribe
        }
    }

    public void OnCraftButtonClick() {
        if(craftingRecipe != null && inventory != null) { //if we have a recipe and an inventory
            if(craftingRecipe.CanCraft(inventory)) { //if there are enough materials in inventory
                if(inventory.IsFull() == false) { //if inventory is not full
                    craftingRecipe.Craft(inventory);
                } else { //Inventory is full
                    //Show message (TODO)
                    Debug.Log("Inventory Full");
                }
            } else { //The are not enough materials in inventory
                //Show message (TODO)
                Debug.Log("There are not enough materials in inventory");
            }
        }
    }

    private void SetCraftingRecipe(CraftingRecipe newCraftingRecipe) {
        _craftingRecipe = newCraftingRecipe;
        if(_craftingRecipe != null) {//if we have a recipe
            int slotIndex = 0; //first material slot (0-3)
            slotIndex = SetSlots(_craftingRecipe.materials, slotIndex); //Set slots with materials
            //Hide the not needed material slots
            for(int i = slotIndex; i < 4; i++) {
                //recipeSlots[i].transform.gameObject.SetActive(false);
                recipeSlots[i].item = null; //if set it to null, then image alpha is 0
                recipeSlots[i].amount = 0;
            }
            slotIndex = 4; //first result slot (4-5)
            slotIndex = SetSlots(_craftingRecipe.results, slotIndex); //Set slots with results
            //Hide the not needed result slots
            for(int i = slotIndex; i < recipeSlots.Length; i++) {
                //recipeSlots[i].transform.gameObject.SetActive(false);
                recipeSlots[i].item = null; //if set it to null, then image alpha is 0
                recipeSlots[i].amount = 0;
            }
            gameObject.SetActive(true); //Activate the row
        } else { //if we don't have a recipe
            gameObject.SetActive(false); //Deactivate the row
        }
    }

    private int SetSlots(IList<ItemAmount> itemAmountList, int slotIndex) {
        for(int i = 0; i < itemAmountList.Count; i++, slotIndex++) { //loop the list of items and slots at the same time
            ItemAmount itemAmount = itemAmountList[i]; //Get the item in this list position
            RecipeSlot recipeSlot = recipeSlots[slotIndex]; //Get the slot in this list position
            //Insert item in the slot:
            recipeSlot.item = itemAmount.item;
            recipeSlot.amount = itemAmount.amount;
            recipeSlot.transform.gameObject.SetActive(true);
        }
        return slotIndex;
    }
}
