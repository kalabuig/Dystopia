using UnityEngine;
using System;

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

    [SerializeField] private ContainerItem[] items; //Items in the container

    public ContainerItem[] GetItems() {
        return items;
    }

    public void SetItems(ContainerItem[] newItems) {
        items = null; //empty the container items array
        items = new ContainerItem[newItems.Length];
        for(int i = 0; i<newItems.Length; i++) {
            //copying each element to the container items array
            items[i] = new ContainerItem { item = newItems[i].item, amount = newItems[i].amount};
        }
    }

}
