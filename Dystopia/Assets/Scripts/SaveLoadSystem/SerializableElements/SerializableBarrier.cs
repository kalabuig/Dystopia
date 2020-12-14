using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableBarrier 
{
    public SerializableTransform transform;
    public int currentHealth;
    public int barrierType;

    public SerializableBarrier(GameObject barrier) {
        DoSerialization(barrier);
    }

    public void DoSerialization(GameObject barrier) {
        transform = new SerializableTransform(barrier.transform);
        currentHealth = barrier.GetComponent<Hittable>().currentHealth;
        barrierType = (int)barrier.GetComponent<Barrier>().barrierData.barrierType;
    }
}
