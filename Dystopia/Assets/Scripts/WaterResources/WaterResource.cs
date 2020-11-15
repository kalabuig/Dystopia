using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterResource : MonoBehaviour
{
    [Serializable]
    public class ItemPair {
        public WaterType waterType;
        public Item sourceItem;
        public Item resultItem;
    }

    [Serializable]
    public struct ContainerItem {
        public Item item;
        public int amount;

        public static ContainerItem Empty() {
            return new ContainerItem { item = null, amount = 0};
        }
    }

    public enum WaterType {
        Pure, //0
        Dirty, //1
        Toxic //2
    }

    [SerializeField] private string waterResourceName = "Water Resource";
    [Space]
    [Header("Item sources and results")]
    [SerializeField] List<ItemPair> itemAssets;

    private WaterType waterType;
    private ContainerItem myItem; //Item in the water resource

    private void Awake() {
        SetRandomWaterType();
    }

    private void SetRandomWaterType() {
        int randNum = UnityEngine.Random.Range(0, 3);
        waterType = (WaterType)randNum;
    }

    public WaterType GetWaterType() {
        return waterType;
    }

    public string GetWaterResourceName() {
        return waterResourceName + " of " + waterType.ToString() + " Water";
    }

    public ContainerItem ReplaceItem() {
        if(myItem.item==null) return ContainerItem.Empty();
        ItemPair itemPair = itemAssets.FindAll(x => x.waterType == waterType).Find( x => x.sourceItem.ID == myItem.item.ID);
        if(itemPair!=null && itemPair.resultItem != null) {
            myItem.item = null;
            myItem.item = itemPair.resultItem;
            return myItem;
        } else {
            return ContainerItem.Empty();
        }
    }

    public ContainerItem GetItem() {
        return myItem;
    }

    public void SetItem(ContainerItem newItem) {
        myItem = new ContainerItem { item = newItem.item, amount = newItem.amount};
    }
}
