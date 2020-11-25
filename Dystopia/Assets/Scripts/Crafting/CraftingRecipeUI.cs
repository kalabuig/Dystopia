using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CraftingRecipeUI : MonoBehaviour
{
    public Action<RecipeSlot> OnPointerEnterEvent;
    public Action<RecipeSlot> OnPointerExitEvent;

    //[SerializeField] RectTransform craftButtonTransform;
    [SerializeField] RecipeSlot[] recipeSlots; //slots of the recipe row

    [Space]
    [Header("Progress bar")]
    [SerializeField] private Image progressImage;

    [Space]
    [Header("Autopopulated fields")]
    public Inventory inventory; //player inventory (crafted items destination)

    //Management of the time for do an action
    protected int actionTick;
    protected int actionTickMax;
    protected bool isDoingAction;

    private Character _character; //To get speeds
    public Character character { get => _character;}

    private StatsModifiers _statsModifiers; //To get speed modifiers
    public StatsModifiers statsModifiers { get => _statsModifiers;}

    private CraftingRecipe _craftingRecipe;
    public CraftingRecipe craftingRecipe {
        get { return _craftingRecipe; }
        set { SetCraftingRecipe(value); }
    }

    private void Awake() {
        recipeSlots = GetComponentsInChildren<RecipeSlot>(includeInactive: true);
        GameObject player = GameObject.Find("Player");
        _character = player?.GetComponent<Character>();
        _statsModifiers = player?.GetComponent<StatsModifiers>();
        isDoingAction = false;
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
                    float craftingTime = character.craftSpeed - statsModifiers.GetFloatStatMod(StatsModifiers.Modifier.craftSpeed);
                    DoAction(craftingTime);
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

    private void ResetProgress() {
        progressImage.fillAmount = 0;
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e) {
        if(isDoingAction) {
            actionTick += 1;
            if(actionTick>=actionTickMax) { //Scavenging finished
                isDoingAction = false;
                UnSuscribe(); //unsubscribe from the tick system
                actionTick = 0;
                ResetProgress();
                craftingRecipe.Craft(inventory);
            }
            //show progress
            progressImage.fillAmount = actionTick * 1f / actionTickMax;
        }
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

    private void UnSuscribe() {
         TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
    }

    private void OnDestroy() {
        UnSuscribe();
    }

}
