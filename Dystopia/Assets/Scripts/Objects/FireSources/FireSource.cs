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

    [Serializable]
    public struct ContainerItem {
        public Item item;
        public int amount;

        public static ContainerItem Empty() {
            return new ContainerItem { item = null, amount = 0};
        }
    }

    [SerializeField] private string fireSourceName = "Fire Source";
    [Space]
    [Header("Item sources and results")]
    [SerializeField] List<ItemPair> itemAssets;

    private ContainerItem myItem; //Item in the fire source

    public string GetFireSourceName() {
        return fireSourceName;
    }

    public Sprite GetSprite() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr!=null) {
            return sr.sprite;
        }
        return null;
    }

    public ContainerItem ReplaceItem() {
        if(myItem.item==null) return ContainerItem.Empty();
        ItemPair itemPair = itemAssets.Find( x => x.sourceItem.ID == myItem.item.ID);
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

    public void EmptyInvenoty() {
        myItem = ContainerItem.Empty();
    }
}
