
using UnityEngine;

public class WOTooltipCalls : MonoBehaviour
{
    private GameHandler gameHandler;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    private void OnMouseEnter() {
        if(gameHandler!=null && gameHandler.worldObjectTooltip!=null) {
            if(GameHandler.IsMouseOverUI() == false) {
                gameHandler.worldObjectTooltip.ShowTooltip(this.gameObject);
            } else {
                gameHandler.worldObjectTooltip.HideTooltip();
            }
        }
    }

    void OnMouseExit(){
        if(gameHandler!=null && gameHandler.worldObjectTooltip!=null) {
            gameHandler.worldObjectTooltip.HideTooltip();
        }
    }

    private void OnDisable() {
        if(gameHandler!=null && gameHandler.worldObjectTooltip!=null) {
            gameHandler.worldObjectTooltip.HideTooltip();
        }
    }

    private void OnDestroy() {
        if(gameHandler!=null && gameHandler.worldObjectTooltip!=null) {
            gameHandler.worldObjectTooltip.HideTooltip();
        }
    }
}
