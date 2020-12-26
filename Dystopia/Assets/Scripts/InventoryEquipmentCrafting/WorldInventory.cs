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
    [SerializeField] ProgressPanel progressPanel;
    [SerializeField] Slider progressSlider;
    [SerializeField] Text progressText;
    [Space]

    private Character _character; //To get speeds
    public Character character { get => _character;}

    private StatsModifiers _statsModifiers; //To get speed modifiers
    public StatsModifiers statsModifiers { get => _statsModifiers;}

    private GameHandler _gameHandler; //To get Level System
    public GameHandler gameHandler { get => _gameHandler;}

    //Management of the time for do an action
    protected int actionTick;
    protected int actionTickMax;
    protected bool isDoingAction;

    private void Awake() {
        _gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
        GameObject player = GameObject.Find("Player");
        _character = player?.GetComponent<Character>();
        _statsModifiers = player?.GetComponent<StatsModifiers>();
    }

    public void DoAction(float secondsToFinishAction) {
        actionTickMax = (int)(secondsToFinishAction * TimeTickSystem.GetTicksPerSecond());
        actionTick = 0;
        isDoingAction = true;
        ResetProgress();
        UnSuscribe(); 
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;  //Suscribe to time tick system
        if(progressPanel!=null) {
            progressPanel.gameObject.SetActive(true);
            progressPanel.ShowPanel(title.text, GameHandler.GetSprite(GameHandler.GetSelectedContainer()));
        }
    }

    public void StopAction() {
        //ResetProgress();
        UnSuscribe(); //unsubscribe from the tick system
        if(progressPanel!=null) {
            progressPanel.HidePanel();
        }
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
                if(progressPanel!=null) {
                    progressPanel.HidePanel();
                }
                character.ModifyVigor(-1); //-1 in vigor when action is finished (scavenging, fill with water, use fire...)
                DoSomething();
            }
            progressText.text = (actionTick * 100f / actionTickMax).ToString("F0") + "%";
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
