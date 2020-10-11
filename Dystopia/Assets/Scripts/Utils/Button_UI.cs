using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_UI : MonoBehaviour, IPointerClickHandler
{
    public Action MouseLeftClickFunc = null;
    public Action MouseRightClickFunc = null;
    public Action MouseMiddleClickFunc = null;
    public Action<PointerEventData> OnPointerClickFunc;

    public virtual void OnPointerClick(PointerEventData eventData) {
            if (OnPointerClickFunc != null) OnPointerClickFunc(eventData);
            if (eventData.button == PointerEventData.InputButton.Left)
                if (MouseLeftClickFunc != null) 
                    MouseLeftClickFunc();
            if (eventData.button == PointerEventData.InputButton.Right)
                if (MouseRightClickFunc != null) 
                    MouseRightClickFunc();
            if (eventData.button == PointerEventData.InputButton.Middle)
                if (MouseMiddleClickFunc != null) 
                    MouseMiddleClickFunc();
        }
}
