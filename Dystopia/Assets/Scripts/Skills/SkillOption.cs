using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillOption : MonoBehaviour
{
    [SerializeField] Image skillSprite;
    [SerializeField] TextMeshProUGUI skillTitle;
    [SerializeField] TextMeshProUGUI skillDescription;

    private PassiveSkillData _passiveSkillData;
    public PassiveSkillData passiveSkillData { get => _passiveSkillData; }

    public void SetPassiveSkill(PassiveSkill newPassiveSkill) {
        _passiveSkillData = new PassiveSkillData();
        _passiveSkillData.skillSprite = newPassiveSkill.skillSprite;
        _passiveSkillData.skillName = newPassiveSkill.skillName;
        _passiveSkillData.skillDescription = newPassiveSkill.skillDescription;
        _passiveSkillData.skillLevel = newPassiveSkill.skillLevel;
        _passiveSkillData.valueAtThisLevel = newPassiveSkill.valuesPerLevel[(int)newPassiveSkill.skillLevel];
        _passiveSkillData.skillModifierType = newPassiveSkill.skillModifierType;
        CopyStatsModifiers(newPassiveSkill);
        CopyCharacterModifiers(newPassiveSkill);
        CopySpecialModifiers(newPassiveSkill);
        RefreshSkillOptionUI();
    }

    private void CopyStatsModifiers(PassiveSkill newPassiveSkill) {
        int counter = newPassiveSkill.statModifiers.Length;
        _passiveSkillData.statModifiers = new StatsModifiers.Modifier[counter];
        for(int i = 0; i<counter; i++) {
            _passiveSkillData.statModifiers[i] = new StatsModifiers.Modifier();
            _passiveSkillData.statModifiers[i] = newPassiveSkill.statModifiers[i];
        }
    }

    private void CopyCharacterModifiers(PassiveSkill newPassiveSkill) {
        int counter = newPassiveSkill.characterModifiers.Length;
        _passiveSkillData.characterModifiers = new CharacterModifier[counter];
        for(int i = 0; i<counter; i++) {
            _passiveSkillData.characterModifiers[i] = new CharacterModifier();
            _passiveSkillData.characterModifiers[i] = newPassiveSkill.characterModifiers[i];
        }
    }

    private void CopySpecialModifiers(PassiveSkill newPassiveSkill) {
        int counter = newPassiveSkill.specialModifiers.Length;
        _passiveSkillData.specialModifiers    = new SpecialModifier[counter];
        for(int i = 0; i<counter; i++) {
            _passiveSkillData.specialModifiers[i] = new SpecialModifier();
            _passiveSkillData.specialModifiers[i] = newPassiveSkill.specialModifiers[i];
        }
    }

    public void UpdateSkillLevel(SkillLevel newSkillLevel, int newValue) {
        _passiveSkillData.skillLevel = newSkillLevel;
        _passiveSkillData.valueAtThisLevel = newValue;
    }

    public void RefreshSkillOptionUI() {
        skillSprite.sprite = _passiveSkillData.skillSprite;
        skillTitle.text = GetTitleColor(_passiveSkillData.skillLevel) + _passiveSkillData.skillName + " - " + PassiveSkill.EnumToString(_passiveSkillData.skillLevel) + "</color>";
        skillDescription.text = _passiveSkillData.skillDescription;
        string percentilText = _passiveSkillData.skillModifierType == SkillModifierType.StatPercentage ? "% " : " ";
        string positiveText = _passiveSkillData.valueAtThisLevel >= 0 ? "+" : "";
        //Stats Modifiers
        if(_passiveSkillData.statModifiers.Length>0) {
            skillDescription.text += "\n";
            foreach(StatsModifiers.Modifier statModifier in _passiveSkillData.statModifiers) {
                skillDescription.text += "\n" + positiveText +  _passiveSkillData.valueAtThisLevel.ToString() + percentilText + statModifier.ToString();
            }
        }
        //Character Modifiers
        if(_passiveSkillData.characterModifiers.Length>0) {
            skillDescription.text += "\n";
            foreach(CharacterModifier characterModifier in _passiveSkillData.characterModifiers) {
                    skillDescription.text += "\n" + positiveText +  _passiveSkillData.valueAtThisLevel.ToString() + percentilText + characterModifier.ToString();
            }
        }
        //Special Modifiers
        if(_passiveSkillData.specialModifiers.Length>0) {
            skillDescription.text += "\n";
            foreach(SpecialModifier specialModifier in _passiveSkillData.specialModifiers) {
                skillDescription.text += "\n" + positiveText +  _passiveSkillData.valueAtThisLevel.ToString() + percentilText + specialModifier.ToString();
            }
        }
    }

    private string GetTitleColor(SkillLevel skillLevel) {
        switch(skillLevel) {
            default:
            case SkillLevel.Basic:
                return "<color=#000000>"; //black
            case SkillLevel.Good:
                return "<color=#60A917>"; //green
            case SkillLevel.Great:
                return "<color=#E3C800>"; //Yellow
            case SkillLevel.Expert:
                return "<color=#FA6800>"; //Orange
            case SkillLevel.GreatExpert:
                return "<color=#E51400>"; //Red
            case SkillLevel.Professional:
                return "<color=#AA00FF>"; //Purple
        }
    }
}
