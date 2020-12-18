using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableIsland
{
    public SerializableVector3 position;
    public SerializableTransform ground;
    public List<SerializableBuilding> buildings;

    public SerializableIsland(GameObject island) {
        buildings = new List<SerializableBuilding>();
        DoSerialization(island);
    }

    public void DoSerialization(GameObject island) {
        //Serialize position
        position = new SerializableVector3(island.transform.position);
        //Serialize children objects:
        foreach (Transform child in island.transform) {
            //Floor
            if(child.name.Substring(0,3) == "Gro") {
                ground = new SerializableTransform(child.transform);
            }
            //Buildings
            if (child.name=="Buildings") {
                foreach(Transform subchild in child.transform) {
                    //if(subchild.gameObject.layer == LayerMask.NameToLayer("Buildings")) {
                    buildings.Add(new SerializableBuilding(subchild.gameObject));
                    //}
                }
            }
        }
    }
}
