using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> panels;
    [SerializeField] private Color tabIdle;
    [SerializeField] private Color tabHover;
    [SerializeField] private Color tabActive;
    private TabButton selectedTab; 

    public void Subscribe(TabButton button) {
        if(tabButtons == null) {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
        //if there is no selected tab yet, set this as the selected one
        if(selectedTab==null) {
            OnTabSelected(button);
        }
    }

    public void OnTabEnter(TabButton button) {
        ResetTabs();
        if(selectedTab == null || button != selectedTab) {
            button.SetColor(tabHover);
        }
    }

    public void OnTabExit(TabButton button) {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button) {
        selectedTab = button;
        ResetTabs();
        button.SetColor(tabActive);
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i<panels.Count; i++) {
            panels[i].SetActive(i==index);
        }
    }

    public void ResetTabs() {
        foreach(TabButton button in tabButtons) {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.SetColor(tabIdle);
        }
    }
}
