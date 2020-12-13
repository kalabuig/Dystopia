using UnityEngine;

public enum EquipmentType {
    Head,
    Body,
    Hands,
    Legs,
    Feet,
    Accessory,
    Back,
    Weapon,
}

public enum AttackRange {
    Short,
    Medium,
    Large,
}

[CreateAssetMenu(menuName = "Items / Equippable Item")]
public class EquippableItem : Item
{
    [Space]
    [Header("Color to show")]
    public Color color = new Color(1,1,1,1); //Color to show in the player body when equipped
    [Space]
    [Header("Modifiers")]
    public int warm; //How many increases your warm
    public int protection; //How many increases your protection to physical damage
    public int moveSpeed; //How fast increase / decrease your movement
    public int criticalChance; //Increase or decrease what chance of criticla hit do you have
    public float craftSpeed; //seconds increased or decreased when crafting
    public float investigationSpeed; //seconds increased or decreased when doing and investigation
    public float scavengingSpeed; //seconds increased or decreased when scavenging
    public float fillWaterSpeed; //seconds increased or decreased when filling water
    public float useFireSpeed; //seconds increased or decreased when using fire
    [Space]
    [Header("Equipment Type")]
    public EquipmentType equipmentType;
    [Header("Only Weapons")]
    public int damage;
    public AttackRange attackRange;

    public override Item GetCopy() {
        return Instantiate(this); //Equippable items are not stackable, so we create a new instance
    }

    public override void Destroy() {
        Destroy(this);
    }
}
