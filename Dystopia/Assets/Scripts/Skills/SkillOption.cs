using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillOption : MonoBehaviour
{
    public struct PassiveSkillData {
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

    [SerializeField] Image skillSprite;
    [SerializeField] TextMeshProUGUI skillTitle;
    [SerializeField] TextMeshProUGUI skillDescription;

    private PassiveSkillData _passiveSkillData;
    public PassiveSkillData passiveSkillData { get => _passiveSkillData; }

    public void SetPassiveSkill(PassiveSkill newPassiveSkill) {
        _passiveSkillData.skillSprite = newPassiveSkill.skillSprite;
        _passiveSkillData.skillName = newPassiveSkill.skillName;
        _passiveSkillData.skillDescription = newPassiveSkill.skillDescription;
        _passiveSkillData.skillLevel = newPassiveSkill.skillLevel;
        _passiveSkillData.valueAtThisLevel = newPassiveSkill.valuesPerLevel[(int)newPassiveSkill.skillLevel];
        _passiveSkillData.skillModifierType = newPassiveSkill.skillModifierType;
        //CopyStatsModifiers(newPassiveSkill);
        //CopyCharacterModifiers(newPassiveSkill);
        //CopySpecialModifiers(newPassiveSkill);


        RefreshSkillOptionUI();
        Debug.Log("Skill " + _passiveSkillData.skillName + " defined");
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

    public void RefreshSkillOptionUI() {
        skillSprite.sprite = _passiveSkillData.skillSprite;
        skillTitle.text = _passiveSkillData.skillName;
        skillDescription.text = _passiveSkillData.skillDescription;
    }
}
