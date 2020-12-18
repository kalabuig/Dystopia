using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableCharacter
{
    //Character Script Data
    public int health;
    public int hungry;
    public int thirst;
    public int vigor;
    public int maxHealth; 
    public int maxHungry; 
    public int maxThirst; 
    public int maxVigor; 
    public int attack; 
    public int defense;

    public int criticalChance;
    public float moveSpeed;
    public float craftSpeed;
    public float investigationSpeed;
    public float scavengingSpeed;
    public float fillWaterSpeed;
    public float useFireSpeed;

    public float healthRate;
    public float thirstRate;
    public float hungryRate;
    public float vigorRate; 

    public SerializableCharacter(Character character) {
        DoSerialization(character);
    }

    public void DoSerialization(Character character) {
        if(character!=null) {
            health = character.health;
            hungry = character.hungry;
            thirst = character.thirst;
            vigor = character.vigor;
            maxHealth = character.maxHealth;
            maxHungry = character.maxHungry;
            maxThirst = character.maxThirst;
            maxVigor = character.maxVigor;
            attack = character.attack;
            defense = character.defense;

            criticalChance = character.criticalChance;
            moveSpeed = character.moveSpeed;
            craftSpeed = character.craftSpeed;
            investigationSpeed = character.investigationSpeed;
            scavengingSpeed = character.scavengingSpeed;
            fillWaterSpeed = character.fillWaterSpeed;
            useFireSpeed = character.useFireSpeed;

            healthRate = character.healthRate;
            thirstRate = character.thirstRate;
            hungryRate = character.hungryRate;
            vigorRate = character.vigorRate;
        }
    }

}
