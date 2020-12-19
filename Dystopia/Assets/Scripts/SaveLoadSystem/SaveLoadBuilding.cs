using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadBuilding : MonoBehaviour
{
    [SerializeField] public GameObject pfRoof;
    [SerializeField] public GameObject pfFloor;
    [SerializeField] public GameObject pfWall;
    [SerializeField] public GameObject pfCol;
    [SerializeField] public GameObject pfBarrier;
    [SerializeField] public GameObject[] pfContainers;
    [SerializeField] public GameObject[] pfWaterFillers;
    [SerializeField] public GameObject[] pfFireSources;

/*
    private const string fileName = "buildings";

    public void Save(GameObject buildingToSave) {
        if(buildingToSave!=null) {
            SerializableBuilding serBuilding = new SerializableBuilding(buildingToSave);
            SaveLoadSystem.Save<SerializableBuilding>(serBuilding, fileName);
        }
    }
*/

    public void Load(SerializableBuilding serLoadedBuilding, Transform parent = null) {
        GameObject emptyObject = new GameObject(); //dummy object
        //Get Data
        //SerializableBuilding serLoadedBuilding = SaveLoadSystem.Load<SerializableBuilding>(fileName);
        //Building
        Vector3 posBuilding = new Vector3(serLoadedBuilding.position.x, serLoadedBuilding.position.y, serLoadedBuilding.position.z);
        GameObject newBuilding = GameObject.Instantiate(emptyObject, posBuilding, Quaternion.identity);
        newBuilding.name = "Building";
        if(parent != null) newBuilding.transform.parent = parent; //Set parent
        newBuilding.transform.SetPositionAndRotation(posBuilding, Quaternion.identity); //Set position
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
        //Water Resources Folder
        GameObject waterResourcesFolder = GameObject.Instantiate(emptyObject, posBuilding, Quaternion.identity);
        waterResourcesFolder.name = "WaterResources";
        waterResourcesFolder.transform.SetParent(newBuilding.transform, true);
        //Instantiate WaterFillers
        foreach(SerializableWaterFiller sw in serLoadedBuilding.waterFillers) {
            loadWaterFiller(sw, waterResourcesFolder);
        }
        //Fire Sources Folder
        GameObject fireSourcesFolder = GameObject.Instantiate(emptyObject, posBuilding, Quaternion.identity);
        fireSourcesFolder.name = "FireSources";
        fireSourcesFolder.transform.SetParent(newBuilding.transform, true);
        //Instantiate FireSources
        foreach(SerializableFireSource sf in serLoadedBuilding.fireSources) {
            loadFireSource(sf, fireSourcesFolder);
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
        newBarrier.GetComponent<Hittable>()?.SetHealth(sb.currentHealth, newBarrier.GetComponent<Hittable>().maxHealth);
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
        newContainer.GetComponent<Hittable>()?.SetHealth(sc.currentHealth, newContainer.GetComponent<Hittable>().maxHealth);
        //ContainerStartingItems
        newContainer.GetComponent<ContainerStartingItems>().createdFromSavedGame = true;
        //Container:
        Container container = newContainer.GetComponent<Container>();
        if(container!=null) {
            //Remaining scavengings
            container.remainingScavengings = sc.remainingScavengings;
            //Empty the inventory (because of the fact that some random objects are created when the new instance is called)
            container.EmptyInventory();
            //Fill inventory with objects
            List<Container.ContainerItem> containerItemList = GetContainerItemList(sc); 
            container.SetItems(containerItemList.ToArray());
        } else Debug.LogError("Container component not found.");
    }

    private GameObject SelectContainerByName(string containerName) {
        foreach(GameObject go in pfContainers) {
            if(go.GetComponent<Container>().GetContainerName() == containerName) {
                return go;
            }
        }
        return null;
    }

    private List<Container.ContainerItem> GetContainerItemList(SerializableContainer sc) {
        List<Container.ContainerItem> containerItemList = new List<Container.ContainerItem>();
        foreach(SerializableItem si in sc.items) {
            Item item = GetItemByID(si.ID);
            if(item!=null) {
                containerItemList.Add(new Container.ContainerItem() { item = item.GetCopy(), amount = si.amount });
            }
        }
        return containerItemList;
    }

    private Item GetItemByID(string ID) {
        //Search in the GlobalItemAssets the item (objectscript) by its ID.
        Item item = GlobalItemAssets.Instance.itemList?.Find( x => x.ID == ID );
        if(item==null) Debug.Log("Item: " + ID);
        return item;
    }

    private void loadWaterFiller(SerializableWaterFiller sw, GameObject parent) {
        //Transform
        SerializableTransform st = sw.transform;
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject waterResource = GameObject.Instantiate(SelectWaterResourceByName(sw.waterResourceName), pos , rot);
        waterResource.transform.localScale = lsc;
        waterResource.name = sw.waterResourceName;
        waterResource.transform.SetParent(parent.transform, true);
        //Current Health
        waterResource.GetComponent<Hittable>()?.SetHealth(sw.currentHealth, waterResource.GetComponent<Hittable>().maxHealth);
        //Water Resource:
        WaterResource waterResourceComponent = waterResource.GetComponent<WaterResource>();
        if(waterResourceComponent!=null) {
            //Empty the inventory (because of the fact that some random objects are created when the new instance is called)
            waterResourceComponent.EmptyInvenoty();
            //Fill inventory with the object
            if(sw.item!=null) {
                Item item = GetItemByID(sw.item.ID);
                if(item!=null) {
                    waterResourceComponent.SetItem( new WaterResource.ContainerItem() { item = item.GetCopy(), amount = sw.item.amount });
                }
            }
        }
    }

    private GameObject SelectWaterResourceByName(string waterFillerName) {
        foreach(GameObject go in pfWaterFillers) {
            if(go.GetComponent<WaterResource>().GetSimpleWaterResourceName() == waterFillerName) {
                return go;
            }
        }
        return null;
    }

    private void loadFireSource(SerializableFireSource sf, GameObject parent) {
        //Transform
        SerializableTransform st = sf.transform;
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        GameObject fireSource = GameObject.Instantiate(SelectFireSourceByName(sf.fireSourceName), pos , rot);
        fireSource.transform.localScale = lsc;
        fireSource.name = sf.fireSourceName;
        fireSource.transform.SetParent(parent.transform, true);
        //Current Health
        fireSource.GetComponent<Hittable>()?.SetHealth(sf.currentHealth, fireSource.GetComponent<Hittable>().maxHealth);
        //Fire Source:
        FireSource fireSourceComponent = fireSource.GetComponent<FireSource>();
        if(fireSourceComponent!=null) {
            //Empty the inventory (because of the fact that some random objects are created when the new instance is called)
            fireSourceComponent.EmptyInvenoty();
            //Fill inventory with the object
            if(sf.item!=null) {
                Item item = GetItemByID(sf.item.ID);
                if(item!=null) {
                    fireSourceComponent.SetItem( new FireSource.ContainerItem() { item = item.GetCopy(), amount = sf.item.amount });
                }
            }
        }
    }

    private GameObject SelectFireSourceByName(string fireSourceName) {
        foreach(GameObject go in pfFireSources) {
            if(go.GetComponent<FireSource>().GetFireSourceName() == fireSourceName) {
                return go;
            }
        }
        return null;
    }
}
