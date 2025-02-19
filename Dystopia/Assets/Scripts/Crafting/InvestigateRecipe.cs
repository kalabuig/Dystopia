﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigateRecipe : MonoBehaviour
{
    [SerializeField] private CraftingBookPanel craftingBookPanel;

    [SerializeField] private Image progressImage;

    private GameHandler gameHandler;

    //Management of the time for do an action
    protected int actionTick;
    protected int actionTickMax;
    protected bool isDoingAction;

    private Character _character; //To get speeds
    public Character character { get => _character;}

    private StatsModifiers _statsModifiers; //To get speed modifiers
    public StatsModifiers statsModifiers { get => _statsModifiers;}

    private ComponentSlot[] componentSlots;
    public ComponentSlot[] ComponentSlots { get => componentSlots;}
    
    private List<CraftingRecipe> allCraftingRecipes;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
        componentSlots = GetComponent<CraftingPanel>()?.componentSlots;
        allCraftingRecipes = GameObject.Find("RecipeAssets")?.GetComponent<RecipeAssets>()?.GetCraftingRecipesList();
        GameObject player = GameObject.Find("Player");
        _character = player?.GetComponent<Character>();
        _statsModifiers = player?.GetComponent<StatsModifiers>();
        isDoingAction = false;
    }

    public void TryInvestigation() {
        if(ComponentSlotsEmpty() == true) return;
        float investigationTime = character.investigationSpeed - statsModifiers.GetFloatStatMod(StatsModifiers.Modifier.investigationSpeed);
        DoAction(investigationTime);
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e) {
        if(isDoingAction) {
            actionTick += 1;
            if(actionTick>=actionTickMax) { //Scavenging finished
                isDoingAction = false;
                UnSuscribe(); //unsubscribe from the tick system
                actionTick = 0;
                ResetProgress();
                character.ModifyVigor(-1); //-1 in vigor when investigation is finished (succesfully or not)
                TryRecipe();
            }
            //show progress
            progressImage.fillAmount = actionTick * 1f / actionTickMax;
        }
    }

    private void ResetProgress() {
        progressImage.fillAmount = 0;
    }

    public void DoAction(float secondsToFinishAction) {
        if(isDoingAction==false) {
            actionTickMax = (int)(secondsToFinishAction * TimeTickSystem.GetTicksPerSecond());
            actionTick = 0;
            isDoingAction = true;
            UnSuscribe(); 
            ResetProgress();
            TimeTickSystem.OnTick += TimeTickSystem_OnTick;  //Suscribe to time tick system
        }
    }

    private void TryRecipe() {
        //If there are no components don't do anything
        if(ComponentSlotsEmpty() == true) return;
        //Check all crafting recipes
        foreach(CraftingRecipe cr in allCraftingRecipes) {
            //Check if all the materials of the recipe are in the component slots
            if(CheckAllComponents(cr)) { 
                //We have a candidate, Check if all the component slots have a component that is part of the recipe
                if(CheckAllMaterials(cr)) {
                    if(AddRecipeToKnownRecipes(cr)) { //Add this recipe to the known recipes
                        gameHandler.levelSystem.AddExperience(20);
                        gameHandler.ShowMessage("Recipe discovered: " + cr.name, MessagePanel.MessageIcon.Celebration, SoundManager.Sound.ItemFound);
                    } else {
                        gameHandler.ShowMessage("Recipe already known: " + cr.name, MessagePanel.MessageIcon.IDontKnow, SoundManager.Sound.ItemNotFound);
                    }
                    return;
                }
            }
        }
        //TODO: Message no recipe discovered
        //Debug.Log("No recipe found");
        gameHandler.ShowMessage("No recipe found", MessagePanel.MessageIcon.Fail, SoundManager.Sound.ItemNotFound);
    }

    private bool RecipeInKnownRecipesList(CraftingRecipe cr) {
        foreach(CraftingRecipe recipe in craftingBookPanel.knownCraftingRecipes) {
            if(cr.name == recipe.name) return true;
        }
        return false;
    }

    private bool AddRecipeToKnownRecipes(CraftingRecipe cr) {
        if(cr != null && RecipeInKnownRecipesList(cr) == false) { //check if recipe is not in the list
            //Add recipe
            craftingBookPanel.knownCraftingRecipes.Add(cr);
            //TODO Send Message
            Debug.Log("Recipe " + cr.name + " added to known recipes.");
            //Refresh UI
            craftingBookPanel.UpdateCraftingRecipeRowsList();
            return true;
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
            if(itemAmount.item.ID == item.ID) return true; // the material is in the recipe
        }
        return false;
    }

    private void UnSuscribe() {
         TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
    }

    private void OnDestroy() {
        UnSuscribe();
    }

}
