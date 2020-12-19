using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class RecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Text amountText;
    protected Image itemImage;

    public event Action<RecipeSlot> OnPointerEnterEvent;
    public event Action<RecipeSlot> OnPointerExitEvent;

    private Color alpha1 = Color.white;
    private Color alpha0 = new Color(1,1,1,0);

    protected Item _item;
    public Item item {
        get { return _item; }
        set {
            _item = value;
            if (_item == null) {
                if(itemImage!=null) {
                    itemImage.color = alpha0;
                }
            } else {
                if(itemImage!=null) {
                    itemImage.sprite = _item.icon;
                    itemImage.color = alpha1;
                }
            }
        }
    }

    private int _amount;
    public int amount {
        get { return _amount; }
        set {
            _amount = value;
            if(_amount < 0) _amount = 0;
            if(_amount == 0) item = null;
            if(amountText!=null) {
                amountText.enabled = _item != null && _amount > 1;
                if(amountText.enabled) {
                    amountText.text = _amount.ToString();
                }
            }
        }
    }

    private void Awake() {
        amountText = GetComponentInChildren<Text>();
        itemImage = GetComponent<Image>();
    }

    public virtual bool CanAddStack(Item otherItem, int amountToAdd = 1) {
        return item != null && item.ID == otherItem.ID && amount + amountToAdd < otherItem.MaximumStacks;
    }

    public virtual bool CanReceiveItem(Item item) {
        return false; //All slots in recipes can not receive any item
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //itemTooltip.ShotTooltip(item);
        if(OnPointerEnterEvent != null)
            OnPointerEnterEvent(this); //Throw event OnPointerEnterEvent
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //itemTooltip.HideTooltip();
        if(OnPointerExitEvent != null)
            OnPointerExitEvent(this); //Throw event OnPointerExitEvent
    }

}
