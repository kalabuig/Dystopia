using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingHotkeys : MonoBehaviour
{
    private Character character;
    private WeatherHandler weatherHandler;
    private Camera cam;

    private void Awake() {
        character = GetComponent<Character>();
        cam = Camera.main;
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
        //Weather Hotkeys
        if(Input.GetKeyDown(KeyCode.Alpha1)){ //normal weather
            WeatherHandler.Instance.SetWeather(WeatherHandler.WeatherType.None);
        } else if(Input.GetKeyDown(KeyCode.Alpha2)){ //fog
            WeatherHandler.Instance.SetWeather(WeatherHandler.WeatherType.Fog);
        } else if(Input.GetKeyDown(KeyCode.Alpha3)){ //rain
            WeatherHandler.Instance.SetWeather(WeatherHandler.WeatherType.Rain);
        }
        //Popup mouse button
        //if(Input.GetMouseButtonDown(0)) {
        //    Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); 
        //    mousePos.z = 0;
        //    bool isCriticalHit = UnityEngine.Random.Range(0,100) < 30; //30% chance of crit
        //    DamagePopup.Create(mousePos, 100, isCriticalHit);
        //}
    }
}
