using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    private struct StatData {
        public string statName;
        public int statValue;
    }

    [SerializeField] StatDisplay[] statDisplays;
    
    private List<StatData> stats;
    private Character character;
    private StatsModifiers statsModifiers;
    private Skills skills;
    private PlayerAttack playerAttack;

    private void Awake() {
        GameObject player = GameObject.Find("Player");
        character = player?.GetComponent<Character>(); //Get Character script from the player
        statsModifiers = player?.GetComponent<StatsModifiers>(); //Get StatsModifiers script from the player
        skills = player?.GetComponent<Skills>(); //Get Skills script from the player
        playerAttack = player?.GetComponent<PlayerAttack>(); //Get PlayerAttack script from the player
    }

    private void Start() {
        statDisplays = GetComponentsInChildren<StatDisplay>();
        RefreshStats();
    }

    public void RefreshStats() {
        RefreshStatsData(); 
        RefreshUIStats();
    }

    private void RefreshStatsData() {
        stats = new List<StatData>();
        if(character==null) return;
        statsModifiers.GetEquippedItems();
        stats.Add(new StatData { statName = "Moving Speed", statValue = 
            System.Int32.Parse( (character.moveSpeed + statsModifiers.GetIntStatMod(StatsModifiers.Modifier.moveSpeed) + skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.moveSpeed) ).ToString() )});
        stats.Add(new StatData { statName = @"Damage", statValue = playerAttack.GetDamageAmount() });
        stats.Add(new StatData { statName = @"Defense", statValue = 
            character.defense + statsModifiers.GetIntStatMod(StatsModifiers.Modifier.protection) + + skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.protection) });
        stats.Add(new StatData { statName = @"Critical Chance (%)", statValue = 
            character.criticalChance + statsModifiers.GetIntStatMod(StatsModifiers.Modifier.criticalChance) + skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.criticalChance) });
        stats.Add(new StatData { statName = "Scavenging Time", statValue 
            = (int)(character.scavengingSpeed - statsModifiers.GetFloatStatMod(StatsModifiers.Modifier.scavengingSpeed) - skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.scavengingSpeed)) });
        stats.Add(new StatData { statName = "Investigation Time", statValue =
             (int)(character.investigationSpeed - statsModifiers.GetFloatStatMod(StatsModifiers.Modifier.investigationSpeed) - skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.investigationSpeed)) });
        stats.Add(new StatData { statName = "Crafting Time", statValue = 
            (int)(character.craftSpeed - statsModifiers.GetFloatStatMod(StatsModifiers.Modifier.craftSpeed) - skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.craftSpeed)) });
        //stats.Add(new StatData { statName = "Thirst Rate", statValue = (int)character.thirstRate });
        //stats.Add(new StatData { statName = "Hungry Rate", statValue = (int)character.hungryRate });
        //stats.Add(new StatData { statName = "Vigor Loss Rate", statValue = (int)character.vigorRate });
    }

    private void RefreshUIStats() {
        int i = 0;
        //Show stats in displays
        for(; i < statDisplays.Length && i < stats.Count; i++) {
            statDisplays[i].NameText.text = stats[i].statName;
            statDisplays[i].ValueText.text = stats[i].statValue.ToString();
            statDisplays[i].enabled = true;
        }
        //Hide empty displays
        for(; i< statDisplays.Length; i++) {
            statDisplays[i].NameText.text = "";
            statDisplays[i].ValueText.text = "";
            statDisplays[i].enabled = false;
        }
    }
}
