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
            ShowObjects(false); // Hide roof
            DayNightCycle.Instance.SetInsideBuilding(true);
        }
    }
 
    // Outside the building
    void OnTriggerExit2D(Collider2D other) {
        if(spriteRenderer!=null) {
            ShowObjects(true); // Show roof
            DayNightCycle.Instance.SetInsideBuilding(false);
        }
    }

    private void ShowObjects(bool b) {
        spriteRenderer.enabled = b; // Show/Hide roof
        if(WeatherHandler.Instance.weather == WeatherHandler.WeatherType.Fog) {
            WeatherHandler.Instance.ActivateFog(b);
        }
    }
}
