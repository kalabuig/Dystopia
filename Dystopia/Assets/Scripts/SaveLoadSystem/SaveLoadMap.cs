using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveLoadIsland), typeof(SaveLoadBuilding))]
public class SaveLoadMap : MonoBehaviour
{
    [SerializeField] private GameObject pfMap;
    private SaveLoadIsland saveLoadIsland;

    private const string fileName = "map";
    
    private void Awake() {
        saveLoadIsland = GetComponent<SaveLoadIsland>();
    }

    public void Save(GameObject mapToSave) {
        if(mapToSave!=null) {
            SerializableMap serMap = new SerializableMap(mapToSave);
            SaveLoadSystem.Save<SerializableMap>(serMap, fileName);
        }
    }

    public void Load() {
        //Get Data
        SerializableMap serLoadedMap = SaveLoadSystem.Load<SerializableMap>(fileName);
        //Map
        Vector3 posMap = new Vector3(serLoadedMap.position.x, serLoadedMap.position.y, serLoadedMap.position.z);
        GameObject newMap = GameObject.Instantiate(pfMap, posMap, Quaternion. identity);
        newMap.name = "Map";
        //Instantiate Islands
        foreach(SerializableIsland si in serLoadedMap.islands) {
            //loadIsland(si, islandsFolder.transform);
            GameObject newIsland = loadIsland(si, newMap.transform);
            if(newIsland!=null) {
                newMap.GetComponent<MapWithoutSectorsHandler>().AddIslandToDictionary(newIsland);
            }
        }
        //Activate Refresh
        newMap.GetComponent<MapWithoutSectorsHandler>().ActivateRefresh();
    }

    private GameObject loadIsland(SerializableIsland si, Transform parent) {
        if(saveLoadIsland==null) saveLoadIsland = GetComponent<SaveLoadIsland>();
        return saveLoadIsland.Load(si, parent);
    }
}
