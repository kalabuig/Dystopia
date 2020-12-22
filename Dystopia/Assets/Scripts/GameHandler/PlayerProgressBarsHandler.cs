using UnityEngine;
using System;

public class PlayerProgressBarsHandler : MonoBehaviour
{
    [SerializeField] ProgressBar healthBar;
    [SerializeField] ProgressBar hungryBar;
    [SerializeField] ProgressBar thirstBar;
    [SerializeField] ProgressBar vigorBar;
    [SerializeField] ProgressBar batteryBar;

    //Player data
    private Character character;
    private Lantern lantern;
    
    private int maxBatteryEnergy = 1;
    private int currentBatteryEnergy = 0;

    private void Awake() {
        //Player transform
        GameObject player = GameObject.Find("Player");
        if(player!=null) {
            character = player.GetComponent<Character>();
            lantern = player.GetComponent<Lantern>();
        } 
    }

    private void Start() {
        SuscribeToCharacterEvents();
        SuscribeToLanternEvents();
        RefreshBars();
    }

    private void RefreshBars() {
        if(character!=null) {
            healthBar.Current = character.health;
            hungryBar.Current = character.hungry;
            thirstBar.Current = character.thirst;
            vigorBar.Current = character.vigor;
        }
        if(lantern!=null) {
            batteryBar.minValue = 0;
            batteryBar.maxValue = maxBatteryEnergy;
            batteryBar.Current = currentBatteryEnergy;
        }
    }

    private void SuscribeToCharacterEvents() {
        character.OnHealthChange += Character_OnHealthChange;  //Suscribe to OnHealthChange event
        character.OnHungryChange += Character_OnHungryChange;  //Suscribe to OnHungryChange event
        character.OnThirstChange += Character_OnThirstChange;  //Suscribe to OnThirstChange event
        character.OnVigorChange += Character_OnVigorChange;  //Suscribe to OnVigorChange event
    }

    private void SuscribeToLanternEvents() {
        lantern.OnBatteryLevelChange += Lantern_OnBatteryLevelChange; //Suscribe to OnBatteryLevelChange event
    }

    private void Lantern_OnBatteryLevelChange(object sender, Lantern.BatteryEventArgs e)
    {
        maxBatteryEnergy = e.maxValue;
        currentBatteryEnergy = e.currentValue;
        RefreshBars();
    }

    private void Character_OnHealthChange(object sender, Character.AmountEventArgs e) {
        RefreshBars();
    }

    private void Character_OnHungryChange(object sender, Character.AmountEventArgs e) {
        RefreshBars();
    }
    
    private void Character_OnThirstChange(object sender, Character.AmountEventArgs e) {
        RefreshBars();
    }
    
    private void Character_OnVigorChange(object sender, Character.AmountEventArgs e) {
        RefreshBars();
    }
}
