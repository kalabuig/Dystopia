using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Item Effects / Vigor")]
public class VigorItemEffect : UsableItemEffect
{
    public int vigorAmount;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        character.ModifyVigor(vigorAmount);
    }

    public override string GetTooltipDescription()
    {
        return string.Concat(
            vigorAmount > 0?"<color=#00FF00>+ ":"<color=#FF0000>- ",
            Math.Abs(vigorAmount).ToString(),
            " Vigor",
            "</color>"
        );
    }
}

