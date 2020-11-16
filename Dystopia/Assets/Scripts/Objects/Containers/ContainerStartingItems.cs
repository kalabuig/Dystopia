using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerStartingItems : MonoBehaviour
{
    [Header("Starting Items")]
    [Range(0, 10)]
    [SerializeField] int minAmount = 0;
    [Range(0, 10)]
    [SerializeField] int maxAmount = 0;

    private Container container;

    private void Awake() {
        container = GetComponent<Container>();
    }

    private void Start() {
        CreateStartingItems();
    }

    private void CreateStartingItems() {
        if(minAmount>maxAmount) return;
        if(minAmount==0 && maxAmount==0) return;
        //ItemAssets itemAssets = container.GetItemAssets();
        //if(itemAssets!=null && container!=null) {
        if(container!=null) {
            int numOfItems = UnityEngine.Random.Range(minAmount, maxAmount+1); //Return a number between minAmount (inclusive) and maxAmount+1 (exclusive)
            for(int i = 0; i<numOfItems; i++) { //Create numOfItems amount of random items
                Item randomItem = container.GetRandomItem(); //itemAssets.GetRandomItem();
                if(randomItem!=null) {
                    container.AddItem(new Container.ContainerItem { item = randomItem, amount = 1}); //Add item to the container inventory
                }
            }
            
        }
    }
}
