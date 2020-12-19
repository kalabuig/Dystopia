using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField] SkillDisplay pfSkillDisplay; //prefab of skill display
    [SerializeField] RectTransform skillsUIParent; //content object of the scroll view

    public List<SkillDisplay> skillsUIs; //skills rows list

    public Skills playerSkills;  //Skills (of the player) script reference
    public List<PassiveSkillData> skills; //list of player skills to show

    private void Start() {
        playerSkills = GameObject.Find("Player")?.GetComponent<Skills>();
        StartSkillsRowsList();
    }

    public void RefreshSkillsList() {
        if(playerSkills!=null) {
            playerSkills = GameObject.Find("Player")?.GetComponent<Skills>();
            skills = playerSkills?.GetSkills();
        }
        UpdateSkillsRowsList();
    }

    private void StartSkillsRowsList() {
        skillsUIParent.GetComponentsInChildren<SkillDisplay>(includeInactive: true, result: skillsUIs);
        RefreshSkillsList();
        UpdateSkillsRowsList();
    }

    public void UpdateSkillsRowsList() {
        if(skills==null) return;
        //foreach skill to show
        for(int i = 0; i < skills.Count; i++) {
            if(skillsUIs.Count == i) { //if we are at the end of the list of UI objects
                skillsUIs.Add(Instantiate(pfSkillDisplay, skillsUIParent, false)); //add a new row
            } 
            skillsUIs[i].SetPassiveSkill(skills[i]);
            skillsUIs[i].RefreshSkillOptionUI();
        }
    }

    private void OnEnable() {
        RefreshSkillsList();
    }
}
