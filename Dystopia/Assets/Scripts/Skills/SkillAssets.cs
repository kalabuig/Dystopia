using System.Collections.Generic;
using UnityEngine;

public class SkillAssets : GenericSingletonClass<SkillAssets>
{
    //Here the list of possible passive skills for the player
    [SerializeField] private List<PassiveSkill> _passiveSkills;
    public List<PassiveSkill> passiveSkills { get => _passiveSkills; }

    public List<PassiveSkill> GetPassiveSkillsList(){
        return _passiveSkills;
    }
}
