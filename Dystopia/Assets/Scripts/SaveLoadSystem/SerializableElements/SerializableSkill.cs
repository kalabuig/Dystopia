using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableSkill
{
    public string skillName;
    public int skillLevel;

    public SerializableSkill(PassiveSkillData passiveSkillData) {
        DoSerialization(passiveSkillData);
    }

    public void DoSerialization(PassiveSkillData passiveSkillData) {
        skillName = passiveSkillData.skillName;
        skillLevel = (int)passiveSkillData.skillLevel;
    }
}
