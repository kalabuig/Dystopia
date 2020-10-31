using UnityEngine;
using System;

public class ContainerTrigger : MonoBehaviour
{
    private GameHandler gameHandler;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    // if player enters the trigger area
    void OnTriggerEnter2D(Collider2D other) {
        //if player is inside the trigger area
        if (gameHandler!=null && other.gameObject.name == "Player") {
            gameHandler.SetContainer(this.gameObject);
        }
    }
 
    // If player exits the trigger area
    void OnTriggerExit2D(Collider2D other) {
        //Unselect the container
        if (gameHandler!=null && other.gameObject.name == "Player") {
            gameHandler.UnSetContainer(this.gameObject);
        }
    }
}
