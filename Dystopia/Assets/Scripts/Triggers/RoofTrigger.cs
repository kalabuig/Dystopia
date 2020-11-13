using UnityEngine;

public class RoofTrigger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject fog;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fog = GameObject.Find("Fog");
    }
 
    // Inside the building
    void OnTriggerEnter2D(Collider2D other) {
        //if the player is inside the building
        if (other.gameObject.name == "Player" && spriteRenderer!=null) {
            ShowObjects(false); // Hide roof
        }
    }
 
    // Outside the building
    void OnTriggerExit2D(Collider2D other) {
        if(spriteRenderer!=null) {
            ShowObjects(true); // Show roof
        }
    }

    private void ShowObjects(bool b) {
        spriteRenderer.enabled = b; // Show/Hide roof
        fog.SetActive(b); // Show/Hide fog
    }
}
