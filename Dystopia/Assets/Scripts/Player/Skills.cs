using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skills : MonoBehaviour
{
    private List<PassiveSkillData> skills;

    private void Awake() {
        skills = new List<PassiveSkillData>();
    }

    //Check if the skill is in the list, if it is there return true and set index position value to the out parameter
    public bool IsTheSkillThere(PassiveSkillData newSkill, out int skillIndex) {
        for(int i = 0; i<skills.Count; i++) {
            if(skills[i]!=null && skills[i].skillName == newSkill.skillName) { //match (skill found)
                skillIndex = i; 
                return true; 
            }
        }
        //Not found
        skillIndex = -1;
        return false;
    }

    //Add skill to the list
    public void AddSkill(PassiveSkillData newSkill) {
        int skillIndex = -1;
        if(IsTheSkillThere(newSkill, out skillIndex) == false) { //It is a new skill
            PassiveSkillData skill = new PassiveSkillData();
            skill.skillSprite = newSkill.skillSprite;
            skill.skillName = newSkill.skillName;
            skill.skillDescription = newSkill.skillDescription;
            skill.skillLevel = newSkill.skillLevel;
            skill.valueAtThisLevel = newSkill.valueAtThisLevel;
            skill.skillModifierType = newSkill.skillModifierType;
            CopyStatsModifiers(skill, newSkill);
            CopyCharacterModifiers(skill, newSkill);
            CopySpecialModifiers(skill, newSkill);
            skills.Add(skill); //add skill
        } else { //The player has this skill
            if(skillIndex>=0 && skillIndex<skills.Count) {
                if(skills[skillIndex].skillLevel < newSkill.skillLevel) {
                    UpgradeSkillLevel(skillIndex, newSkill.skillLevel, newSkill.valueAtThisLevel); //Upgrade the skill level
                }
            }
        }
    }

    //Get the list of skills
    public List<PassiveSkillData> GetSkills() {
        return skills;
    }

    //Upgrade Skill Level
    private void UpgradeSkillLevel(int skillIndex, SkillLevel newLevel, int newValue) {
        if(skillIndex>=0 && skillIndex<skills.Count) {
            skills[skillIndex].skillLevel = newLevel;
            skills[skillIndex].valueAtThisLevel = newValue;
        }
    }

    public PassiveSkillData GetSkillData(int skillIndex) {
        if(skillIndex>=0 && skillIndex<skills.Count) {
            return skills[skillIndex];
        } else {
            return null;
        }
    }

    private void CopyStatsModifiers(PassiveSkillData skill, PassiveSkillData newSkill) {
        int counter = newSkill.statModifiers.Length;
        skill.statModifiers = new StatsModifiers.Modifier[counter];
        for(int i = 0; i<counter; i++) {
            skill.statModifiers[i] = new StatsModifiers.Modifier();
            skill.statModifiers[i] = newSkill.statModifiers[i];
        }
    }

    private void CopyCharacterModifiers(PassiveSkillData skill, PassiveSkillData newSkill) {
        int counter = newSkill.characterModifiers.Length;
        skill.characterModifiers = new CharacterModifier[counter];
        for(int i = 0; i<counter; i++) {
            skill.characterModifiers[i] = new CharacterModifier();
            skill.characterModifiers[i] = newSkill.characterModifiers[i];
        }
    }

    private void CopySpecialModifiers(PassiveSkillData skill, PassiveSkillData newSkill) {
        int counter = newSkill.specialModifiers.Length;
        skill.specialModifiers    = new SpecialModifier[counter];
        for(int i = 0; i<counter; i++) {
            skill.specialModifiers[i] = new SpecialModifier();
            skill.specialModifiers[i] = newSkill.specialModifiers[i];
        }
    }
}

//Skill Data Structure
[Serializable]
public class PassiveSkillData {
    public Sprite skillSprite;
    public string skillName;
    public string skillDescription;
    public SkillLevel skillLevel;
    public int valueAtThisLevel;
    public SkillModifierType skillModifierType;
    public StatsModifiers.Modifier[] statModifiers;
    public CharacterModifier[] characterModifiers;
    public SpecialModifier[] specialModifiers;
}
