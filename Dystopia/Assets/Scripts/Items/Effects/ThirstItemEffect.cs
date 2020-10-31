using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Item Effects / Thirst")]
public class ThirstItemEffect : UsableItemEffect
{
    public int thirstAmount;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        character.ModifyThirst(thirstAmount);
    }

    public override string GetTooltipDescription()
    {
        return string.Concat(
            thirstAmount > 0?"<color=#00FF00>+ ":"<color=#FF0000>- ",
            Math.Abs(thirstAmount).ToString(),
            " Thirst",
            "</color>"
        );
    }
}
