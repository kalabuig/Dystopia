using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionPanel : MonoBehaviour
{
    [SerializeField] SkillAssets skillAssets;
    [SerializeField] SkillOption[] skillOptions;

    private GameHandler gameHandler;
    private Skills playerSkills;

    private void Start() {
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        playerSkills = gameHandler.playerTransform.gameObject.GetComponent<Skills>();
    }

    public void SetRandomSkills() {
        foreach(SkillOption s in skillOptions) { //Cleaning values
            if(s.passiveSkillData!=null) {
                s.passiveSkillData.skillName = "-1";
            }
        }
        int numPassiveSkills = skillAssets.passiveSkills.Count;
        for(int i = 0; i<skillOptions.Length; i++) { //filling with random skills
            int randNum = UnityEngine.Random.Range(0, numPassiveSkills);
            if(SkillOptionInPanel(skillAssets.passiveSkills[randNum])) {
                i--; //try again
                continue;
            } else {
                skillOptions[i]?.SetPassiveSkill(skillAssets.passiveSkills[randNum]);
                UpdateSkillLevel(skillOptions[i], randNum);
            }
        }
    }

    //Check if the skill is already on the panel
    private bool SkillOptionInPanel(PassiveSkill newSkill) {
        foreach(SkillOption skillOption in skillOptions) {
            if(skillOption.passiveSkillData!=null && skillOption.passiveSkillData.skillName == newSkill.skillName) {
                return true;
            }
        }
        return false;
    }

    //Update the level if the player it has this skill
    private void UpdateSkillLevel(SkillOption so, int skillAssetsIndex) {
        int numOfLevelsAvailable = Enum.GetNames(typeof(SkillLevel)).Length;
        if(playerSkills.IsTheSkillThere(so.passiveSkillData, out int index)) { //The player has the skill
            PassiveSkillData psd = playerSkills.GetSkillData(index);
            int playerSkillLevel = (int)psd.skillLevel;
            if(playerSkillLevel<numOfLevelsAvailable-1) {
                playerSkillLevel++;
            } else {
                playerSkillLevel = numOfLevelsAvailable - 1; //Max level
            }
            so.passiveSkillData.skillLevel = (SkillLevel)playerSkillLevel;
            so.UpdateSkillLevel(so.passiveSkillData.skillLevel, skillAssets.passiveSkills[skillAssetsIndex].valuesPerLevel[(int)so.passiveSkillData.skillLevel]); //skill level + value of the level
            so.RefreshSkillOptionUI();
        }
    }

    public void SkillSelected(int index) {
        if(playerSkills!=null && index>=0 && index<skillOptions.Length) {
            playerSkills.AddSkill(skillOptions[index].passiveSkillData);
            gameHandler.ResumeGame();
            gameHandler.CloseSkillSelectionPanel();
        }
    }
}
