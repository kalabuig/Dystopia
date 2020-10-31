using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Item Effects / Heal")]
public class HealItemEffect : UsableItemEffect
{
    public int healthAmount;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        character.ModifyHealth(healthAmount);
    }

    public override string GetTooltipDescription()
    {
        return string.Concat(
            healthAmount > 0?"<color=#00FF00>+ ":"<color=#FF0000>- ",
            Math.Abs(healthAmount).ToString(),
            " Health",
            "</color>"
        );
    }
}
