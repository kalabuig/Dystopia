using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableFireSource
{
    public SerializableTransform transform;
    //Hittable attributes
    public int currentHealth;
    //Fire Source attributes:
    public string fireSourceName;
    public SerializableItem item;

    public SerializableFireSource(GameObject fireSource) {
        DoSerialization(fireSource);
    }

    public void DoSerialization(GameObject fireSource) {
        //Transform
        transform = new SerializableTransform(fireSource.transform);
        //Current Health
        Hittable hittableScript = fireSource.GetComponent<Hittable>();
        currentHealth = hittableScript!=null ? hittableScript.currentHealth : 1; //Some fireSource aren't hittable
        //FireSource attributes:
        FireSource fireSourceComponent = fireSource.GetComponent<FireSource>();
        if(fireSourceComponent!=null) {
            //Fire Source name
            fireSourceName = fireSourceComponent.GetFireSourceName();
            //Items in the container:
            FireSource.ContainerItem containerItem = fireSourceComponent.GetItem();
            if(containerItem.item!=null && containerItem.amount>0) {
                item = new SerializableItem(containerItem.item, containerItem.amount);
            }
        }
    }
}
