using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSaveLoad : MonoBehaviour
{
    private GameObject map;

    private SaveLoadBuilding saveLoadBuilding;
    private SaveLoadIsland saveLoadIsland;
    private SaveLoadMap saveLoadMap;

    private void Awake() {
        saveLoadBuilding = GetComponent<SaveLoadBuilding>();
        saveLoadIsland = GetComponent<SaveLoadIsland>();
        saveLoadMap = GetComponent<SaveLoadMap>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            SaveMapTest();
        }
        if(Input.GetKeyDown(KeyCode.Z)) {
            LoadMapTest();
        }
    }

    private void SaveMapTest() {
        map = GameObject.Find("Map"); //Searching the map gameobject
        if(map!=null && saveLoadMap!=null) {
            saveLoadMap.Save(map);
        }
    }

    private void LoadMapTest() {
        if(saveLoadMap!=null) {
            saveLoadMap.Load();
        }
    }

}
