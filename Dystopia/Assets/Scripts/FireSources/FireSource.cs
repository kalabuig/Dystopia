using System.Collections.Generic;
using UnityEngine;
using System;

public class FireSource : MonoBehaviour
{
    [Serializable]
    public class ItemPair {
        public Item sourceItem;
        public Item resultItem;
    }

    [SerializeField] private string fireSourceName = "Fire Source";
    [Space]
    [Header("Item sources and results")]
    [SerializeField] List<ItemPair> itemAssets;

    private Item item; //Item in the fire source

    public string GetFireSourceName() {
        return fireSourceName;
    }

    public Item ReplaceItem() {
        if(item==null) return null;
        ItemPair itemPair = itemAssets.Find( x => x.sourceItem.ID == item.ID);
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
