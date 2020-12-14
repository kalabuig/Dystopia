using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableVector4
{
    public float w;
    public float x;
    public float y;
    public float z;

    public SerializableVector4(Vector4 v) {
        w = v.w;
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public SerializableVector4(float paramW, float paramX, float paramY, float paramZ) {
        w = paramW;
        x = paramX;
        y = paramY;
        z = paramZ;
    }
}
