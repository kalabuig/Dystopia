using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class WorldInventory : Inventory
{
    [Header("Panel Title")]
    [SerializeField] protected TextMeshProUGUI title;
    [Space]
    [Header("Progress Bar")]
    [SerializeField] Slider progressSlider;
    [SerializeField] Text progressText;
    [Space]

    //Management of the time for do an action
    protected int actionTick;
    protected int actionTickMax;
    protected bool isDoingAction;

    public void loadItems(Container c) {
        if(c==null) return;
        //Reset the slots
        foreach(ItemSlot slot in itemSlots) {
            slot.item = null;
            slot.amount = 0;
        }
        List<Container.ContainerItem> itemsToLoad = c.GetItems(); //Get the items to load in the panel
         int i = 0;
        //Populate slots with items
        if(itemsToLoad!=null && itemsToLoad.Count>0) {
            for(; i<itemsToLoad.Count && i < itemSlots.Length; i++) {
                if(itemsToLoad[i].item!=null && itemsToLoad[i].amount>0) {
                    itemSlots[i].item = itemsToLoad[i].item.GetCopy(); //Instantiate a new item based on the scriptobject
                    itemSlots[i].amount = itemsToLoad[i].amount; //Setting the amount
                }
            }
        }
        //Populate slots with empty
        for(; i < itemSlots.Length; i++) {
            itemSlots[i].item = null;
            itemSlots[i].amount = 0; //Setting the amount to 0
        }
    }

    public void storeItems(Container c) {
        if(c==null) return;
        //Create the structure to store the items in the structure
        Container.ContainerItem[] itemsToStore = new Container.ContainerItem[itemSlots.Length];
        for(int i = 0; i < itemSlots.Length; i++) {
            itemsToStore[i] = new Container.ContainerItem { item = itemSlots[i].item, amount = itemSlots[i].amount};
        }
        c.SetItems(itemsToStore);
    }

    public void DoAction(int ticksToFinishAction) {
        actionTickMax = ticksToFinishAction;
        actionTick = 0;
        isDoingAction = true;
        ResetProgress();
        UnSuscribe(); 
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;  //Suscribe to time tick system
    }

    public void StopAction() {
        ResetProgress();
        UnSuscribe(); //unsubscribe from the tick system
    }

    private void ResetProgress() {
        progressText.text = "0%";
        progressSlider.value = 0;
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e) {
        if(isDoingAction) {
            actionTick += 1;
            if(actionTick>=actionTickMax) { //Scavenging finished
                isDoingAction = false;
                UnSuscribe(); //unsubscribe from the tick system
                DoSomething();
            }
            progressText.text = (actionTick * 100f / actionTickMax).ToString() + "%";
            progressSlider.value = (actionTick * 1f / actionTickMax);
        }
    }

    protected virtual void DoSomething() {
        //Override here the actions to do when the action timer finished
    }

    private void UnSuscribe() {
         TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
    }

    private void OnDestroy() {
        UnSuscribe();
    }
}
