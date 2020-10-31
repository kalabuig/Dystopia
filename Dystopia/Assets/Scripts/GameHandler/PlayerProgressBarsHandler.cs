using UnityEngine;
using System;

public class PlayerProgressBarsHandler : MonoBehaviour
{
    [SerializeField] ProgressBar healthBar;
    [SerializeField] ProgressBar hungryBar;
    [SerializeField] ProgressBar thirstBar;
    [SerializeField] ProgressBar vigorBar;

    //Player data
    private Character character;
    
    private void Awake() {
        //Player transform
        GameObject player = GameObject.Find("Player");
        if(player!=null) character = player.GetComponent<Character>();
    }

    private void Start() {
        SuscribeToCharacterEvents();
        RefreshBars();
    }

    private void RefreshBars() {
        if(character!=null) {
            healthBar.Current = character.health;
            hungryBar.Current = character.hungry;
            thirstBar.Current = character.thirst;
            vigorBar.Current = character.vigor;
        }
    }

    private void SuscribeToCharacterEvents() {
        character.OnHealthChange += Character_OnHealthChange;  //Suscribe to OnHealthChange event
        character.OnHungryChange += Character_OnHungryChange;  //Suscribe to OnHungryChange event
        character.OnThirstChange += Character_OnThirstChange;  //Suscribe to OnThirstChange event
        character.OnVigorChange += Character_OnVigorChange;  //Suscribe to OnVigorChange event
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
