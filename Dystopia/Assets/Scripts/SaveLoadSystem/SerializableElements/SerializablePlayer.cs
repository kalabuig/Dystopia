using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializablePlayer 
{
    public int level;
    public int experience;
    public SerializableTransform transform;
    public SerializableCharacter characterData;
    public List<SerializableSkill> skills;

    public SerializablePlayer(GameObject player) {
        skills = new List<SerializableSkill>();
        DoSerialization(player);
    }

    public void DoSerialization(GameObject player) {
        //Level and exeprience
        GameHandler gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
        if(gameHandler!=null) {
            LevelSystem levelSystem = gameHandler.levelSystem;
            if(levelSystem!=null) {
                level = levelSystem.GetLevelNumber();
                experience = levelSystem.GetExperienceAbsolute();
            }
        }
        //Transform
        transform = new SerializableTransform(player.transform);
        //Character Data
        Character characterComponent = player.GetComponent<Character>();
        if(characterComponent!=null) {
            characterData = new SerializableCharacter(characterComponent);
        }
        //Skills
        Skills skillsComponent = player.GetComponent<Skills>();
        if(skillsComponent!=null) {
            foreach(PassiveSkillData passiveSkillData in skillsComponent.GetSkills()) {
                SerializableSkill serSkill = new SerializableSkill(passiveSkillData);
                if(serSkill!=null) {
                    skills.Add(serSkill);
                }
            }
        }
    }
}
