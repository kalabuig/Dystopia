using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableContainer
{
    public SerializableTransform transform;
    public int currentHealth;
    public string containerName;

    public SerializableContainer(GameObject container) {
        DoSerialization(container);
    }

    public void DoSerialization(GameObject container) {
        transform = new SerializableTransform(container.transform);
        Hittable hittableScript = container.GetComponent<Hittable>();
        currentHealth = hittableScript!=null ? hittableScript.currentHealth : 1; //Some containers aren't hittable
        containerName = container.GetComponent<Container>().GetContainerName();
        //TODO: Serialització dels objectes de l'inventari del contenidor.
    }
}
