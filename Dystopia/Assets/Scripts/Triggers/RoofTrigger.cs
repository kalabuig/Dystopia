using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofTrigger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
 
    // Inside the building
    void OnTriggerEnter2D(Collider2D other) {
        //if the player is inside the building
        if (other.gameObject.name == "Player" && spriteRenderer!=null) {
            spriteRenderer.enabled = false; // Hide roof
        }
    }
 
    // Outside the building
    void OnTriggerExit2D(Collider2D other) {
        if(spriteRenderer!=null) {
            spriteRenderer.enabled = true; // Show roof
        }
    }
}
