﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class Container : MonoBehaviour
{
    [Serializable]
    public struct ContainerItem {
        public Item item;
        public int amount;

        public static ContainerItem Empty() {
            return new ContainerItem { item = null, amount = 0};
        }
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

    [Space]
    [Header("Hurt chance at scavenge fail")]
    [Range(0,100)]
    [SerializeField] private int _hurtChance = 5; //% chance to be hurted when the scavenging fails
    public int hurtChance { get => _hurtChance; }
    [SerializeField] private int _hurtAmount = 1; //amount of damage to do when hurted
    public int hurtAmount { get => _hurtAmount; }

    private List<ContainerItem> items; //Items in the container
    public int remainingScavengings;

    private void Awake() {
        items = new List<ContainerItem>();
        StartingSetting();
    }

    public string GetContainerName() {
        return containerName;
    }

    public Sprite GetSprite() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr!=null) {
            return sr.sprite;
        }
        return null;
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

    public void EmptyInventory() {
        items = null;
        items = new List<ContainerItem>();
    }
}
