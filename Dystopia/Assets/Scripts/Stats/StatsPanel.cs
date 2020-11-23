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

    private void Awake() {
        character = GameObject.Find("Player")?.GetComponent<Character>();
    }

    private void Start() {
        statDisplays = GetComponentsInChildren<StatDisplay>();
        SetStats(); 
        RefreshUIStats();
    }

    public void SetStats() {
        stats = new List<StatData>();
        if(character==null) return;
        stats.Add(new StatData { statName = "Moving Speed", statValue = (int)character.moveSpeed });
        stats.Add(new StatData { statName = @"Critical Chance (%)", statValue = character.criticalChance });
        stats.Add(new StatData { statName = "Scavenging Time", statValue = (int)character.scavengingSpeed });
        stats.Add(new StatData { statName = "Investigation Time", statValue = (int)character.investigationSpeed });
        stats.Add(new StatData { statName = "Crafting Time", statValue = (int)character.craftSpeed });
        stats.Add(new StatData { statName = "Thirst Rate", statValue = (int)character.thirstRate });
        stats.Add(new StatData { statName = "Hungry Rate", statValue = (int)character.hungryRate });
        stats.Add(new StatData { statName = "Vigor Loss Rate", statValue = (int)character.vigorRate });
    }

    public void RefreshUIStats() {
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
