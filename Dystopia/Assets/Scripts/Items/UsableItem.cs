using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items / Usable Item")]
public class UsableItem : Item
{
    public bool IsConsumable; //if true, remove after use it

    public List<UsableItemEffect> listOfEffects;

    public virtual void Use(Character character) {
        foreach(UsableItemEffect effect in listOfEffects) {
            effect.ExecuteEffect(this, character); //execute each effect
        }
    }
}
