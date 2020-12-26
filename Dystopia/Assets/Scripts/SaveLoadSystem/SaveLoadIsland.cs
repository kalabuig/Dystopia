using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveLoadBuilding))]
public class SaveLoadIsland : MonoBehaviour
{
    [SerializeField] public GameObject pfGround;

    private SaveLoadBuilding saveLoadBuilding;

    //private const string fileName = "islands";

    private void Awake() {
        saveLoadBuilding = GetComponent<SaveLoadBuilding>();
    }

/*
    public void Save(GameObject islandToSave) {
        if(islandToSave!=null) {
            SerializableIsland serIsland = new SerializableIsland(islandToSave);
            SaveLoadSystem.Save<SerializableIsland>(serIsland, fileName);
        }
    }
*/

    public GameObject Load(SerializableIsland serLoadedIsland, Transform parent = null) {
        GameObject emptyObject = new GameObject(); //dummy object
        //Get Data
        //SerializableIsland serLoadedIsland = SaveLoadSystem.Load<SerializableIsland>(fileName);
        //Island
        Vector3 posIsland = new Vector3(serLoadedIsland.position.x, serLoadedIsland.position.y, serLoadedIsland.position.z);
        GameObject newIsland = GameObject.Instantiate(emptyObject, posIsland, Quaternion. identity);
        newIsland.name = "Island";
        if(parent != null) newIsland.transform.parent = parent; //Set parent
        newIsland.transform.SetPositionAndRotation(posIsland, Quaternion.identity); //Set position
        //Instantiate Floor
        LoadGround(serLoadedIsland.ground, newIsland);
        //Buildings Folder
        GameObject buildingsFolder = GameObject.Instantiate(emptyObject, posIsland, Quaternion.identity);
        buildingsFolder.name = "Buildings";
        buildingsFolder.transform.SetParent(newIsland.transform, true);
        //Instantiate Buildings
        foreach(SerializableBuilding sb in serLoadedIsland.buildings) {
            loadBuilding(sb, buildingsFolder.transform);
        }
        //Destroy dummy object
        Destroy(emptyObject);
        return newIsland;
    }

    private void LoadGround(SerializableTransform st, GameObject parent) {
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject newFloor = GameObject.Instantiate(pfGround, pos , rot);
        newFloor.transform.localScale = lsc;
        newFloor.name = "Ground";
        newFloor.transform.SetParent(parent.transform, true);
    }

    private void loadBuilding(SerializableBuilding sb, Transform parent) {
        if(saveLoadBuilding==null) saveLoadBuilding = GetComponent<SaveLoadBuilding>();
        saveLoadBuilding.Load(sb, parent);
    }
}
