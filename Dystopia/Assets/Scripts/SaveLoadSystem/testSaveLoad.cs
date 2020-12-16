using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSaveLoad : MonoBehaviour
{
    [SerializeField] private GameObject building;
    [SerializeField] private GameObject island;

    SaveLoadBuilding saveLoadBuilding;
    SaveLoadIsland saveLoadIsland;

    private void Awake() {
        saveLoadBuilding = GetComponent<SaveLoadBuilding>();
        saveLoadIsland = GetComponent<SaveLoadIsland>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            //SaveBuildingTest();
            SaveIslandTest();
        }
        if(Input.GetKeyDown(KeyCode.Z)) {
            //LoadBuildingTest();
            LoadIslandTest();
        }
    }

    private void SaveBuildingTest() {
        if(building!=null && saveLoadBuilding!=null) {
            //saveLoadBuilding.Save(building);
        }
    }

    private void LoadBuildingTest() {
        if(saveLoadBuilding!=null) {
            //saveLoadBuilding.Load();
        }
    }

    private void SaveIslandTest() {
        if(island!=null && saveLoadIsland!=null) {
            saveLoadIsland.Save(island);
        }
    }

    private void LoadIslandTest() {
        if(saveLoadIsland!=null) {
            saveLoadIsland.Load();
        }
    }

}
