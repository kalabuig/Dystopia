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
    private Item item; //Item in the water resource

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

    public Item ReplaceItem() {
        if(item==null) return null;
        ItemPair itemPair = itemAssets.FindAll(x => x.waterType == waterType).Find( x => x.sourceItem.ID == item.ID);
        if(itemPair!=null && itemPair.resultItem != null) {
            item = null;
            item = itemPair.resultItem;
            return item;
        } else {
            return null;
        }
    }

    public Item GetItem() {
        return item;
    }

    public void SetItem(Item newItem) {
        item = null;
        item = newItem.GetCopy();
    }
}
