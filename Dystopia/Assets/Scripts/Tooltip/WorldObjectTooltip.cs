using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WorldObjectTooltip : MonoBehaviour
{
    [SerializeField] Image worldObjectImage;
    [SerializeField] TextMeshProUGUI worldObjectNameText;
    [SerializeField] TextMeshProUGUI worldObjectText;

    public void ShowTooltip(GameObject go) {
        if(go==null) return;
        worldObjectImage.sprite = go.GetComponent<SpriteRenderer>()?.sprite;
        worldObjectNameText.text = GetName(go);
        worldObjectText.text = "";
        gameObject.SetActive(true);
    }

    public void HideTooltip() {
        gameObject.SetActive(false);
    }

    public string GetName(GameObject go) {
        string nameToReturn = "";
        nameToReturn = go.GetComponent<Container>()?.GetContainerName();
        if(nameToReturn!=null) return nameToReturn;
        nameToReturn = go.GetComponent<WaterResource>()?.GetWaterResourceName();
        Debug.Log(nameToReturn);
        if(nameToReturn!=null) return nameToReturn;
        nameToReturn = go.GetComponent<FireSource>()?.GetFireSourceName();
        Debug.Log(nameToReturn);
        if(nameToReturn!=null) return nameToReturn;
        return "unknown";
    }
}
