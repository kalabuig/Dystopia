using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableWaterFiller
{
    public SerializableTransform transform;
    //Hittable attributes
    public int currentHealth;
    //Water Resource attributes:
    public int waterType;
    public string waterResourceName;
    public SerializableItem item;

    public SerializableWaterFiller(GameObject waterFiller) {
        DoSerialization(waterFiller);
    }

    public void DoSerialization(GameObject waterFiller) {
        //Transform
        transform = new SerializableTransform(waterFiller.transform);
        //Current Health
        Hittable hittableScript = waterFiller.GetComponent<Hittable>();
        currentHealth = hittableScript!=null ? hittableScript.currentHealth : 1; //Some waterFillers aren't hittable
        //Container attributes:
        WaterResource waterResourceComponent = waterFiller.GetComponent<WaterResource>();
        if(waterResourceComponent!=null) {
            //Water Type
            waterType = (int)waterResourceComponent.GetWaterType();
            //Water Resorce name
            waterResourceName = waterResourceComponent.GetSimpleWaterResourceName();
            //Items in the container:
            WaterResource.ContainerItem containerItem = waterResourceComponent.GetItem();
            if(containerItem.item!=null && containerItem.amount>0) {
                item = new SerializableItem(containerItem.item, containerItem.amount);
            }
        }
    }
}
