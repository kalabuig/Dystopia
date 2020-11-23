
using UnityEngine;

public class WOTooltipCalls : MonoBehaviour
{
    private GameHandler gameHandler;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    private void OnMouseEnter() {
        if(GameHandler.IsMouseOverUI() == false) {
            gameHandler.worldObjectTooltip.ShowTooltip(this.gameObject);
        } else {
            gameHandler.worldObjectTooltip.HideTooltip();
        }
    }

    void OnMouseExit()
    {
        gameHandler.worldObjectTooltip.HideTooltip();
    }
}
