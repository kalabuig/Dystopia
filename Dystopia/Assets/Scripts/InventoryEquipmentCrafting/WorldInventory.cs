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
