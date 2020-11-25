using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsModifiers : MonoBehaviour
{
    public enum Modifier {
        damage,
        criticalChance,
        moveSpeed,
        protection,
        craftSpeed,
        investigationSpeed,
        scavengingSpeed,
        fillWaterSpeed,
        useFireSpeed,
        warm,
    }

     private GameHandler gameHandler;
     private List<EquippableItem> equippedItems;

    public static string EnumToString(Modifier m) {
        return System.Enum.GetName(typeof(Modifier), m);
    }

    public static object GetPropValue(object src, string propName) {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }

    public static object GetFieldValue(object src, string propName) {
        return src.GetType().GetField(propName).GetValue(src);
    }

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    private void Start() {
        GetEquippedItems();
    }

/*
    private void Update() {
        //TEST
        if(Input.GetKeyDown(KeyCode.L)) {
            GetEquippedItems();
            int aux = 0;
            GetEquipmentAttribute(EnumToString(Modifier.warm), out aux);
            Debug.Log("warm: " + aux);
            float aux2 = 0f;
            GetEquipmentAttribute(EnumToString(Modifier.scavengingSpeed), out aux2);
            Debug.Log("protection: " + aux2);
        }
    }
*/

    public void GetEquippedItems() {
        equippedItems = gameHandler.GetEquippedItems();
    }

    //// Equippment Modifiers ////

    public int GetIntStatMod(StatsModifiers.Modifier modifier) {
        int v = 0;
        GetEquipmentAttribute(StatsModifiers.EnumToString(modifier), out v);
        return v;
    }

    public float GetFloatStatMod(StatsModifiers.Modifier modifier) {
        float v = 0f;
        GetEquipmentAttribute(StatsModifiers.EnumToString(modifier), out v);
        return v;
    }

    private void GetEquipmentAttribute(string attributeName, out int result) {
        result = 0; 
        if(equippedItems==null) return;
        if(attributeName=="") return;
        foreach(EquippableItem item in equippedItems) {
            result += (int) GetFieldValue(item, attributeName);
        }
    }

    private void GetEquipmentAttribute(string attributeName, out float result) {
        result = 0f; 
        if(equippedItems==null) return;
        if(attributeName=="") return;
        foreach(EquippableItem item in equippedItems) {
            float aux = 0f;
            if(System.Single.TryParse(GetFieldValue(item, attributeName).ToString(), out aux)) {
                result += aux;
            }
        }
    }

    //// Passive Modifiers ////
    

}
