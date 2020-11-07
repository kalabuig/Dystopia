using UnityEngine;
using System;
using System.Collections.Generic;

public class Container : MonoBehaviour
{
    [Serializable]
    public struct ContainerItem {
        public Item item;
        public int amount;
        // It looks like properties can't be serialized in the editor
        /*
        private Item _item;
        public Item item {
            get { return _item; }
            set {
                _item = value;
            }
        }
        private int _amount;
        public int amount {
            get { return _amount; }
            set {
                _amount = value;
                if(_amount < 0) _amount = 0;
                if(_amount == 0) item = null;
            }
        }
        */
    }

    [SerializeField] ItemAssets itemAssets; //The list of items that this container can spam (starting items and scavenging items)
    [SerializeField] private List<ContainerItem> items; //Items in the container

    public ItemAssets GetItemAssets() {
        return itemAssets;
    }

    public List<ContainerItem> GetItems() {
        return items;
    }

    public void SetItems(ContainerItem[] newItems) {
        items = null; //empty the container items array
        items = new List<ContainerItem>();
        for(int i = 0; i<newItems.Length; i++) {
            //adding each element to the container items array
            items.Add(new ContainerItem { item = newItems[i].item, amount = newItems[i].amount});
        }
    }

    public void AddItem(ContainerItem newItem) {
        if(items==null) {
            items = new List<ContainerItem>();
        }
        items.Add(new ContainerItem { item = newItem.item, amount = newItem.amount});
    }

}
