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
        //Container
        nameToReturn = go.GetComponent<Container>()?.GetContainerName();
        if(nameToReturn!=null && nameToReturn!="") return nameToReturn;
        //Water Resource
        nameToReturn = go.GetComponent<WaterResource>()?.GetWaterResourceName();
        if(nameToReturn!=null && nameToReturn!="") return nameToReturn;
        //Fire Source
        nameToReturn = go.GetComponent<FireSource>()?.GetFireSourceName();
        if(nameToReturn!=null && nameToReturn!="") return nameToReturn;
        //Barrier
        nameToReturn = go.GetComponent<Barrier>()?.GetBarrierName();
        if(nameToReturn!=null && nameToReturn!="") return nameToReturn;
        //Enemy
        nameToReturn = go.GetComponent<Enemy>()?.GetEnemyName();
        if(nameToReturn!=null && nameToReturn!="") return nameToReturn;
        return "unknown";
    }
}
