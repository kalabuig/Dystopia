using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 v) {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public SerializableVector3(float paramX, float paramY, float paramZ) {
        x = paramX;
        y = paramY;
        z = paramZ;
    }
}
