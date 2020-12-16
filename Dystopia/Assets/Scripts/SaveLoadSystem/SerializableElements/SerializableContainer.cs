using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableContainer
{
    public SerializableTransform transform;
    public int currentHealth;
    //Container attributes:
    public string containerName;
    public List<SerializableItem> items;
    public int remainingScavengings;

    public SerializableContainer(GameObject container) {
        items = new List<SerializableItem>();
        DoSerialization(container);
    }

    public void DoSerialization(GameObject container) {
        //Transform
        transform = new SerializableTransform(container.transform);
        //Current Health
        Hittable hittableScript = container.GetComponent<Hittable>();
        currentHealth = hittableScript!=null ? hittableScript.currentHealth : 1; //Some containers aren't hittable
        //Container attributes:
        Container containerComponent = container.GetComponent<Container>();
        if(containerComponent!=null) {
            //Container name
            containerName = containerComponent.GetContainerName();
            //Items in the container:
            foreach(Container.ContainerItem containerItem in containerComponent.GetItems()) {
                if(containerItem.item!=null && containerItem.amount>0) {
                    items.Add(new SerializableItem(containerItem.item, containerItem.amount));
                }
            }
            //Remaining scavengings
            remainingScavengings = containerComponent.remainingScavengings;
        }
    }
}
