using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    protected Text amountText;
    [SerializeField] protected Image itemImage;

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    private Color alpha1 = Color.white;
    private Color alpha0 = new Color(1,1,1,0);

    protected Item _item;
    public Item item {
        get { return _item; }
        set {
            _item = value;
            if (_item == null) {
                itemImage.color = alpha0;
            } else {
                itemImage.sprite = _item.icon;
                itemImage.color = alpha1;
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
            amountText.enabled = ( _item != null ) && 
                ( _amount > 1 || _item.isMultiUsable == true ); //quantity visible if it is bigger than 1 or if it is multi usable
            if(amountText.enabled) {
                //if it is a multiusabel object (tools, bottles, food, etc)
                if(_item.isMultiUsable) {
                    amountText.text = Mathf.Floor(100f * _amount / _item.maxMultiUses).ToString() + "%";
                } else { //otherwise
                    amountText.text = _amount > 0 ? _amount.ToString() : string.Empty;
                }
            }
        }
    }

/*
    private void OnValidate() { //Only works in editor mode
        gameObject.name = "Slot";
    }
*/

    private void Awake() {
        amountText = GetComponentInChildren<Text>();
    }

    public virtual bool CanAddStack(Item otherItem, int amountToAdd = 1) {
        return item != null && item.ID == otherItem.ID && amount + amountToAdd < otherItem.MaximumStacks;
    }

    public virtual bool CanReceiveItem(Item item) {
        return true; //All slots in inventory can receive an item (no restrictions)
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Check if right click on the slot
        if(eventData != null && eventData.button == PointerEventData.InputButton.Right) {
            if(OnRightClickEvent != null)
                OnRightClickEvent(this); //Throw event OnRightClickEvent
        }
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(OnBeginDragEvent != null)
            OnBeginDragEvent(this); //Throw event OnBeginDragEvent
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(OnEndDragEvent != null)
            OnEndDragEvent(this); //Throw event OnEndDragEvent
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(OnDragEvent != null)
            OnDragEvent(this); //Throw event OnDragEvent
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(OnDropEvent != null)
            OnDropEvent(this); //Throw event OnDropEvent
    }
}
