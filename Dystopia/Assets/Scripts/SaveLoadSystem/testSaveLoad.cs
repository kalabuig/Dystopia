using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSaveLoad : MonoBehaviour
{
    [SerializeField] private GameObject building;

    SaveLoadBuilding saveLoadBuilding;

    private void Awake() {
        saveLoadBuilding = GetComponent<SaveLoadBuilding>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            SaveTest();
        }
        if(Input.GetKeyDown(KeyCode.Z)) {
            LoadTest();
        }
    }

    private void SaveTest() {
        if(building!=null && saveLoadBuilding!=null) {
            saveLoadBuilding.Save(building);
        }
    }

    private void LoadTest() {
        if(saveLoadBuilding!=null) {
            saveLoadBuilding.Load();
        }
    }


}
