using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionPanel : MonoBehaviour
{
    [SerializeField] SkillAssets skillAssets;

    [SerializeField] SkillOption[] skillOptions;

    public void SetRandomSkills() {
        int numPassiveSkills = skillAssets.passiveSkills.Count;
        for(int i = 0; i<skillOptions.Length; i++) {
            int randNum = UnityEngine.Random.Range(0, numPassiveSkills);
            skillOptions[i]?.SetPassiveSkill(skillAssets.passiveSkills[randNum]);
        }
    }
}
