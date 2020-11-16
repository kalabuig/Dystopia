
using UnityEngine;

public class WOTooltipCalls : MonoBehaviour
{
    private GameHandler gameHandler;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    private void OnMouseEnter() {
        gameHandler.worldObjectTooltip.ShowTooltip(this.gameObject);
    }

    void OnMouseExit()
    {
        gameHandler.worldObjectTooltip.HideTooltip();
    }
}
