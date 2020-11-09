using UnityEngine;
using System;
using System.Collections.Generic;

public class Container : MonoBehaviour
{
    [Serializable]
    public struct ContainerItem {
        public Item item;
        public int amount;
    }

    [SerializeField] private string containerName = "Container";

    [Space]
    [Header("Item Assets of the container")]
    [SerializeField] ItemAssets itemAssets; //The list of items that this container can spam (starting items and scavenging items)
    
    [Space]
    [Header("Scavenging")]
    [Range(0,10)]
    [SerializeField] int minAmountToScavenge = 0;
    [Range(0,10)]
    [SerializeField] int maxAmountToScavenge = 5;

    private List<ContainerItem> items; //Items in the container
    private int remainingScavengings;

    private void Awake() {
        StartingSetting();
    }

    public string GetContainerName() {
        return containerName;
    }

    private void StartingSetting() {
        //Set the remaining scavengings (randomly generated)
        if(minAmountToScavenge>maxAmountToScavenge) {
            remainingScavengings = minAmountToScavenge;
            return;
        } 
        if(minAmountToScavenge==0 && maxAmountToScavenge==0) {
            remainingScavengings = 0;
            return;
        };
        remainingScavengings = UnityEngine.Random.Range(minAmountToScavenge, maxAmountToScavenge+1); //Return a number between minAmountToScavenge (inclusive) and maxAmountToScavenge+1 (exclusive)
    }

    public Item GetRandomItem() {
        if(remainingScavengings>0) {
            remainingScavengings--;
            return itemAssets.GetRandomItem();
        } else {
            return null;
        }
    }

    //public ItemAssets GetItemAssets() {
    //    return itemAssets;
    //}

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
