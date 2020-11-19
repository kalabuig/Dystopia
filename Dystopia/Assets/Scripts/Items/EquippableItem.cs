using UnityEngine;

public enum EquipmentType {
    Head,
    Body,
    Hands,
    Legs,
    Feet,
    Accessory,
    Bag,
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
    [Header("Modifiers")]
    public int warm; //How many increases your warm
    public int protection; //How many increases your protection to physical damage
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
