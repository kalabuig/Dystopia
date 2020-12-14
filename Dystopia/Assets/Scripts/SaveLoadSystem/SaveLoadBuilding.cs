using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadBuilding : MonoBehaviour
{
    [SerializeField] private GameObject pfRoof;
    [SerializeField] private GameObject pfFloor;
    [SerializeField] private GameObject pfWall;
    [SerializeField] private GameObject pfCol;
    [SerializeField] private GameObject pfBarrier;
    [SerializeField] private GameObject[] pfContainers;

    private const string fileName = "buildings";

    public void Save(GameObject buildingToSave) {
        if(buildingToSave!=null) {
            SerializableBuilding serBuilding = new SerializableBuilding();
            serBuilding.DoSerialization(buildingToSave);
            SaveLoadSystem.Save<SerializableBuilding>(serBuilding, fileName);
        }
    }

    public void Load() {
        GameObject emptyObject = new GameObject(); //dummy object
        //Get Data
        SerializableBuilding serLoadedBuilding = SaveLoadSystem.Load<SerializableBuilding>(fileName);
        //Building
        Vector3 posBuilding = new Vector3(serLoadedBuilding.position.x, serLoadedBuilding.position.y, serLoadedBuilding.position.z);
        GameObject newBuilding = GameObject.Instantiate(emptyObject, posBuilding, Quaternion. identity);
        newBuilding.name = "Building";
        //Instantiate Roof
        LoadRoof(serLoadedBuilding.roof, newBuilding);
        //Instantiate Floor
        LoadFloor(serLoadedBuilding.floor, newBuilding);
        //Walls Folder
        GameObject wallsFolder = GameObject.Instantiate(emptyObject, posBuilding, Quaternion.identity);
        wallsFolder.name = "Walls";
        wallsFolder.transform.SetParent(newBuilding.transform, true);
        //Instantiate Walls
        foreach(SerializableTransform st in serLoadedBuilding.walls) {
            loadWall(st, wallsFolder);
        }
        //Cols Folder
        GameObject colsFolder = GameObject.Instantiate(emptyObject, posBuilding, Quaternion.identity);
        colsFolder.name = "Cols";
        colsFolder.transform.SetParent(newBuilding.transform, true);
        //Instantiate Cols
        foreach(SerializableTransform st in serLoadedBuilding.cols) {
            loadCol(st, colsFolder);
        }
        //Barriers Folder
        GameObject barriersFolder = GameObject.Instantiate(emptyObject, posBuilding, Quaternion.identity);
        barriersFolder.name = "Barriers";
        barriersFolder.transform.SetParent(newBuilding.transform, true);
        //Instantiate Barriers
        foreach(SerializableBarrier sb in serLoadedBuilding.barriers) {
            loadBarrier(sb, barriersFolder);
        }
        //Container Folder
        GameObject containersFolder = GameObject.Instantiate(emptyObject, posBuilding, Quaternion.identity);
        containersFolder.name = "Containers";
        containersFolder.transform.SetParent(newBuilding.transform, true);
        //Instantiate Boxes
        foreach(SerializableContainer sc in serLoadedBuilding.containers) {
            loadContainer(sc, containersFolder);
        }
        //Destroy dummy object
        Destroy(emptyObject);
    }

    private void LoadRoof(SerializableTransform st, GameObject parent) {
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject newRoof = GameObject.Instantiate(pfRoof, pos , rot);
        newRoof.transform.localScale = lsc;
        newRoof.name = "Roof";
        newRoof.transform.SetParent(parent.transform, true);
    }

    private void LoadFloor(SerializableTransform st, GameObject parent) {
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject newFloor = GameObject.Instantiate(pfFloor, pos , rot);
        newFloor.transform.localScale = lsc;
        newFloor.name = "Floor";
        newFloor.transform.SetParent(parent.transform, true);
    }

    private void loadWall(SerializableTransform st, GameObject parent) {
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject newWall = GameObject.Instantiate(pfWall, pos , rot);
        newWall.transform.localScale = lsc;
        newWall.name = "Wall";
        newWall.transform.SetParent(parent.transform, true);
    }

    private void loadCol(SerializableTransform st, GameObject parent) {
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject newCol = GameObject.Instantiate(pfCol, pos , rot);
        newCol.GetComponent<RandomExistance>().enabled = false; //Disable random existance script
        newCol.transform.localScale = lsc;
        newCol.name = "Col";
        newCol.transform.SetParent(parent.transform, true);
    }

    private void loadBarrier(SerializableBarrier sb, GameObject parent) {
        //Transform
        SerializableTransform st = sb.transform;
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject newBarrier = GameObject.Instantiate(pfBarrier, pos , rot);
        newBarrier.GetComponent<RandomExistance>().enabled = false; //Disable random existance script
        newBarrier.transform.localScale = lsc;
        newBarrier.name = "Barrier";
        newBarrier.transform.SetParent(parent.transform, true);
        //Barrier Data (by type)
        newBarrier.GetComponent<Barrier>().SetBarrierDataByType((Barrier.BarrierType)sb.barrierType);
        //Current Health
        GetComponent<Hittable>()?.SetHealth(sb.currentHealth, GetComponent<Hittable>().maxHealth);
    }

    private void loadContainer(SerializableContainer sc, GameObject parent) {
        //Transform
        SerializableTransform st = sc.transform;
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject newContainer = GameObject.Instantiate(SelectContainerByName(sc.containerName), pos , rot);
        newContainer.transform.localScale = lsc;
        newContainer.name = sc.containerName;
        newContainer.transform.SetParent(parent.transform, true);
        //Current Health
        GetComponent<Hittable>()?.SetHealth(sc.currentHealth, GetComponent<Hittable>().maxHealth);
        //Fill its inventory with objects
        //TODO: utilizar la part d'objectes inicials de l'inventari del contenidor per a carregar els objectes serialitzats i grabats
    }

    private GameObject SelectContainerByName(string containerName) {
        foreach(GameObject go in pfContainers) {
            if(go.GetComponent<Container>().GetContainerName() == containerName) {
                return go;
            }
        }
        return null;
    }
}
