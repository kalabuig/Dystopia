using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableTransform 
{
    public SerializableVector3 position;
    public SerializableVector4 rotation;
    public SerializableVector3 localScale;

    public SerializableTransform(Transform transform) {
        //Position
        position.x = transform.position.x;
        position.y = transform.position.y;
        position.z = transform.position.z;
        //Rotation
        rotation.w = transform.rotation.w;
        rotation.x = transform.rotation.x;
        rotation.y = transform.rotation.y;
        rotation.z = transform.rotation.z;
        //Local Scale
        localScale.x = transform.localScale.x;
        localScale.y = transform.localScale.y;
        localScale.z = transform.localScale.z;
    }

    public SerializableTransform(Vector3 paramPosition, Vector4 paramRotation, Vector3 paramLocalScale) {
        //Position
        position.x = paramPosition.x;
        position.y = paramPosition.y;
        position.z = paramPosition.z;
        //Rotation
        rotation.w = paramRotation.w;
        rotation.x = paramRotation.x;
        rotation.y = paramRotation.y;
        rotation.z = paramRotation.z;
        //Local Scale
        localScale.x = paramLocalScale.x;
        localScale.y = paramLocalScale.y;
        localScale.z = paramLocalScale.z;
    }
}
