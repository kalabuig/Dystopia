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

[CreateAssetMenu(menuName = "Items / Equippable Item")]
public class EquippableItem : Item
{
    public int warm; //How many increases your warm
    public int protection; //How many increases your protection to physical damage
    public int damage;
    [Space]
    public EquipmentType equipmentType;

    public override Item GetCopy() {
        return Instantiate(this); //Equippable items are not stackable, so we create a new instance
    }

    public override void Destroy() {
        Destroy(this);
    }
}
