using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveLoadMap), typeof(SaveLoadPlayer), typeof(SaveLoadInventories))]
public class SaveLoadGame : MonoBehaviour
{
    private SaveLoadMap saveLoadMap;
    private SaveLoadPlayer saveLoadPlayer;
    private SaveLoadInventories saveLoadInventories;

    private void Start() {
        saveLoadMap = GetComponent<SaveLoadMap>();
        saveLoadPlayer = GetComponent<SaveLoadPlayer>();
        saveLoadInventories = GetComponent<SaveLoadInventories>();
    }

    public void Save() {
        //MAP
        GameObject map = GameObject.Find("Map");
        if(map!=null) {
            saveLoadMap.Save(map); //save map
        } else {
            Debug.LogError("Map object not found.");
        }
        //PLAYER
        GameObject player = GameObject.Find("Player");
        if(player!=null) {
            saveLoadPlayer.Save(player); //save player
        } else {
            Debug.LogError("Player object not found.");
        }
        //PLAYER INVENTORIES
        saveLoadInventories.Save();
    }

    public void Load() {
        //MAP
        GameObject map = GameObject.Find("Map");
        if(map!=null) Destroy(map); //remove actual map
        if(saveLoadMap==null) saveLoadMap = GetComponent<SaveLoadMap>();
        saveLoadMap.Load(); //load saved map
        //PLAYER
        //TODO: Load player stats, skills, inventory and equipped items
        GameObject player = GameObject.Find("Player");
        if(player!=null) {
            if(saveLoadPlayer==null) saveLoadPlayer = GetComponent<SaveLoadPlayer>();
            saveLoadPlayer.Load();
        } else {
            Debug.LogError("Player gameobject not found !!");
        }
        //PLAYER INVENTORIES
        if(saveLoadInventories==null) saveLoadInventories = GetComponent<SaveLoadInventories>();
        saveLoadInventories.Load();
    }
}
