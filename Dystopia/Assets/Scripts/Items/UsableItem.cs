using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items / Usable Item")]
public class UsableItem : Item
{
    public bool IsConsumable; //if true, remove after use it
    [Space]
    [Header("Subproduct after use")]
    [SerializeField] private Item SubProduct; //Some items, when used, creates a subproduct (for example: Bottle full of water -- use --> Empty Bottle)
    [Space]
    [Header("Effects when used")]
    public List<UsableItemEffect> listOfEffects;
    [Space]
    [Header("Sound effect when using it")]
    public SoundManager.Sound soundToPlay;

    public virtual void Use(Character character) {
        foreach(UsableItemEffect effect in listOfEffects) {
            effect.ExecuteEffect(this, character); //execute each effect
        }
        if(soundToPlay!=SoundManager.Sound.None) {
            SoundManager.PlaySound(soundToPlay);
        }
    }

    public Item GetSubProduct() {
        return SubProduct;
    }

    

}
