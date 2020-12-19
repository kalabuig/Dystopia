using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPlayer : MonoBehaviour
{
    private GameObject player;
    private Character characterComponent;
    private Skills skillsComponent;
    private GameHandler gameHandler;

    private const string fileName = "player";

    private void Awake() {
        player = GameObject.Find("Player");
        if(player!=null) {
            characterComponent = player.GetComponent<Character>();
            skillsComponent = player.GetComponent<Skills>();
        }
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    public void Save(GameObject playerToSave) {
        if(playerToSave!=null) {
            SerializablePlayer serPlayer = new SerializablePlayer(playerToSave);
            SaveLoadSystem.Save<SerializablePlayer>(serPlayer, fileName);
        }
    }

    public void Load() {
        if(player==null) GameObject.Find("Player");
        if(characterComponent==null) characterComponent = player.GetComponent<Character>();
        if(skillsComponent==null) skillsComponent = player.GetComponent<Skills>();
        //Get Data
        SerializablePlayer serLoadedPlayer = SaveLoadSystem.Load<SerializablePlayer>(fileName);
        //Experience and Level
        if(gameHandler!=null) {
            LevelSystem levelSystem = gameHandler.levelSystem;
            if(levelSystem!=null) {
                levelSystem.LoadLevelExp(serLoadedPlayer.level, serLoadedPlayer.experience);
            }
        }
        //Transform
        SerializableTransform st = serLoadedPlayer.transform;
        Vector3 pos = new Vector3(st.position.x, st.position.y, st.position.z);
        Quaternion rot = new Quaternion(st.rotation.x, st.rotation.y, st.rotation.z, st.rotation.w);
        Vector3 lsc = new Vector3(st.localScale.x, st.localScale.y, st.localScale.z);
        player.transform.position = pos;
        player.transform.rotation = rot;
        player.transform.localScale = lsc;
        //Character Data
        if(serLoadedPlayer!=null && serLoadedPlayer.characterData!=null && characterComponent!=null) {
            characterComponent.LoadSerializedData(serLoadedPlayer.characterData);
        }
        //Skills
        if(serLoadedPlayer!=null && serLoadedPlayer.skills!=null && skillsComponent!=null) {
            skillsComponent.EmptySkills();
            foreach(SerializableSkill serSkill in serLoadedPlayer.skills) {
                PassiveSkill passiveSkill = GetSkillByName(serSkill.skillName);
                if(passiveSkill!=null) {
                    PassiveSkillData passiveSkillData = PassiveSkillToPassiveSkillData(passiveSkill, serSkill.skillLevel);
                    skillsComponent.AddSkill(passiveSkillData);
                }
            }
            
        }
    }

    private PassiveSkill GetSkillByName(string skillName) {
        //Search in the GlobalItemAssets the item (objectscript) by its ID.
        PassiveSkill passiveSkill = SkillAssets.Instance.GetPassiveSkillsList()?.Find( x => x.skillName == skillName );
        if(passiveSkill==null) Debug.Log("Skill not found: " + skillName);
        return passiveSkill;
    }

    private PassiveSkillData PassiveSkillToPassiveSkillData(PassiveSkill passiveSkill, int skillLevel) {
        SkillOption skillOptionDummy = new SkillOption();
        skillOptionDummy.SetPassiveSkill(passiveSkill, false); //Set data without refreshint UI (we don't have UI in this dummy SkillOption object)
        skillOptionDummy.UpdateSkillLevel((SkillLevel)skillLevel, skillLevel);
        return skillOptionDummy.passiveSkillData;
    }
}
