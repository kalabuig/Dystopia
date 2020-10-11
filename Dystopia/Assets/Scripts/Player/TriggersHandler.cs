using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersHandler : MonoBehaviour
{
    private GameHandler gameHandler;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        //if(itemWorld != null) { //Touching an item
//            gameHandler.GetInventory().AddItem(itemWorld.GetItem());
//            itemWorld.DestroySelf();
//        }
    }

}
