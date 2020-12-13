using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField] Guy guy;
    [SerializeField] EquipmentPanel equipmentPanel;

    private GameHandler gameHandler;

    private List<EquippableItem> equippedItems;

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
        if(equipmentPanel!=null && guy!=null) {
            equipmentPanel.OnItemChanged += OnItemChangedFunc;
        }
    }

    private void OnItemChangedFunc() {
        GetEquippedItems();
        RefreshColors();
    }

    private void RefreshColors() {
        //Update the color of each body part
        bool backFound = false;
        bool bodyFound = false;
        bool handsFound = false;
        bool headFound = false;
        foreach(EquippableItem item in equippedItems) {
            //Back
            if(item.equipmentType == EquipmentType.Back) {
                guy.SetColor(Guy.BodyPart.Back, item.color);
                backFound = true;
            }
            //Body
            if(item.equipmentType == EquipmentType.Body) {
                guy.SetColor(Guy.BodyPart.Body, item.color);
                bodyFound = true;
            }
            //Hands
            if(item.equipmentType == EquipmentType.Hands) {
                guy.SetColor(Guy.BodyPart.Hands, item.color);
                handsFound = true;
            }
            //Head
            if(item.equipmentType == EquipmentType.Head) {
                guy.SetColor(Guy.BodyPart.Head, item.color);
                headFound = true;
            }
            //Set default colors for not found body parts
            if(backFound==false) {
                guy.SetTransparent(Guy.BodyPart.Back);
            }
            if(bodyFound==false) {
                guy.SetColor(Guy.BodyPart.Body, guy.defaultBodyColor);
            }
            if(handsFound==false) {
                 guy.SetColor(Guy.BodyPart.Hands, guy.defaultLeftHandColor);
            }
            if(headFound==false) {
                guy.SetColor(Guy.BodyPart.Head, guy.defaultHeadColor);
            }
        }
    }

    public void GetEquippedItems() {
        equippedItems = gameHandler.GetEquippedItems();
    }

    private void OnDestroy() {
        equipmentPanel.OnItemChanged -= OnItemChangedFunc;
    }
}
