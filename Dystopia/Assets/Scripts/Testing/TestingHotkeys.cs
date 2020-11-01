using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingHotkeys : MonoBehaviour
{
    private Character character;

    private void Awake() {
        character = GetComponent<Character>();
    }

    private void Update() {
        //TESTING HOTKEYS
        if(Input.GetKeyDown(KeyCode.Y)){ //increase health
            character.ModifyHealth(20);
        } else if(Input.GetKeyDown(KeyCode.U)) { //decrease health
            character.ModifyHealth(-20);
        } else if(Input.GetKeyDown(KeyCode.H)) { //increase hungry
            character.ModifyHungry(20);
        } else if(Input.GetKeyDown(KeyCode.J)) { //decrease hungry
            character.ModifyHungry(-20);
        } else if(Input.GetKeyDown(KeyCode.N)) { //increase thirst
            character.ModifyThirst(20);
        } else if(Input.GetKeyDown(KeyCode.M)) { //decrease thirst
            character.ModifyThirst(-20);
        }
    }
}
