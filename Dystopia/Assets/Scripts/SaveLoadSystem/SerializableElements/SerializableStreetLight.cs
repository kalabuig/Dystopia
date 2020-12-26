using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableStreetLight
{
    public SerializableTransform transform;
    //StreetLight attributes:
    public string lightName;
    public bool bucle;

    public SerializableStreetLight(GameObject streetLight) {
        DoSerialization(streetLight);
    }

    public void DoSerialization(GameObject streetLight) {
        //Transform
        transform = new SerializableTransform(streetLight.transform);
        //StreetLight attributes:
        LightEffects lightEffectsComponent = streetLight.GetComponent<LightEffects>();
        if(lightEffectsComponent!=null) {
            //Light name
            lightName = lightEffectsComponent.GetName();
            //Effects bucle (true or false)
            bucle = lightEffectsComponent.bucle;
        }
    }
}
