using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableMap
{
    public SerializableVector3 position;
    public List<SerializableIsland> islands;

    public SerializableMap(GameObject map) {
        islands = new List<SerializableIsland>();
        DoSerialization(map);
    }

    public void DoSerialization(GameObject map) {
        //Serialize position
        position = new SerializableVector3(map.transform.position);
        //Serialize children objects:
        foreach (Transform child in map.transform) {
            //Islands
            islands.Add(new SerializableIsland(child.gameObject));
            //if (child.name=="Islands") {
                //foreach(Transform subchild in child.transform) {
                    //if(subchild.gameObject.layer == LayerMask.NameToLayer("Islands")) {
                    //islands.Add(new SerializableIsland(subchild.gameObject));
                    //}
                //}
            //}
        }
    }
}
