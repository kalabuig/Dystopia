using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableBuilding
{
    public SerializableVector3 position;
    public SerializableTransform roof;
    public SerializableTransform floor;
    public List<SerializableTransform> walls;
    public List<SerializableTransform> cols;
    public List<SerializableBarrier> barriers;
    public List<SerializableContainer> containers;
    public List<SerializableWaterFiller> waterFillers;
    public List<SerializableFireSource> fireSources;
    public List<SerializableStreetLight> streetLights;
    
    public SerializableBuilding(GameObject building) {
        walls = new List<SerializableTransform>();
        cols = new List<SerializableTransform>();
        barriers = new List<SerializableBarrier>();
        containers = new List<SerializableContainer>();
        waterFillers = new List<SerializableWaterFiller>();
        fireSources = new List<SerializableFireSource>();
        streetLights = new List<SerializableStreetLight>();
        DoSerialization(building);
    }

    public void DoSerialization(GameObject building) {
        //Serialize position
        position = new SerializableVector3(building.transform.position);
        //Serialize children objects:
        foreach (Transform child in building.transform) {
            //Roof
            if(child.name.Substring(0,3) == "Roo") {
                roof = new SerializableTransform(child.transform);
            }
            //Floor
            if(child.name.Substring(0,3) == "Flo") {
                floor = new SerializableTransform(child.transform);
            }
            //Walls
            if (child.name=="Walls") {
                foreach(Transform subchild in child.transform) {
                    if(subchild.gameObject.layer == LayerMask.NameToLayer("Walls")) {
                        walls.Add(new SerializableTransform(subchild.transform));
                    }
                }
            }
            //Cols
            if (child.name=="Cols") {
                foreach(Transform subchild in child.transform) {
                    if(subchild.gameObject.layer == LayerMask.NameToLayer("Walls")) {
                        cols.Add(new SerializableTransform(subchild.transform));
                    }
                }
            }
            //Barriers
            if (child.name=="Barriers") {
                foreach(Transform subchild in child.transform) {
                    if(subchild.gameObject.layer == LayerMask.NameToLayer("Barriers")) {
                        barriers.Add(new SerializableBarrier(subchild.gameObject));
                    }
                }
            }
            //Containers
            if (child.name=="Containers") {
                foreach(Transform subchild in child.transform) {
                    if(subchild.gameObject.layer == LayerMask.NameToLayer("Containers")) {
                        containers.Add(new SerializableContainer(subchild.gameObject));
                    }
                }
            }
            //WaterFillers (Water Resources)
            if (child.name=="WaterResources") {
                foreach(Transform subchild in child.transform) {
                    if(subchild.gameObject.layer == LayerMask.NameToLayer("WaterFillers")) {
                        waterFillers.Add(new SerializableWaterFiller(subchild.gameObject));
                    }
                }
            }
            //FireSources
            if (child.name=="FireSources") {
                foreach(Transform subchild in child.transform) {
                    if(subchild.gameObject.layer == LayerMask.NameToLayer("FireSources")) {
                        fireSources.Add(new SerializableFireSource(subchild.gameObject));
                    }
                }
            }
            //StreetLights
            if (child.name=="StreetLights") {
                foreach(Transform subchild in child.transform) {
                    if(subchild.gameObject.layer == LayerMask.NameToLayer("StreetLights")) {
                        streetLights.Add(new SerializableStreetLight(subchild.gameObject));
                    }
                }
            }
        } 
    }
}
