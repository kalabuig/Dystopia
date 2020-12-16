using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableItem
{
    public string ID;
    public int amount;

    public SerializableItem(Item item, int itemAmount) {
        DoSerialization(item, itemAmount);
    }

    public void DoSerialization(Item item, int itemAmount) {
        ID = item.ID;
        amount = itemAmount;
    }
}
