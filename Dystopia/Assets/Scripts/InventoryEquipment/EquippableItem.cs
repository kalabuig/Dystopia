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

[CreateAssetMenu]
public class EquippableItem : Item
{
    public int warm; //How many increases your warm
    public int protection; //How many increases your protection to physical damage
    public int damage;
    [Space]
    public EquipmentType equipmentType;
}
