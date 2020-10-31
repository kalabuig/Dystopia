using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Item Effects / Hungry")]
public class HungryItemEffect : UsableItemEffect
{
    public int hungryAmount;

    public override void ExecuteEffect(UsableItem parentItem, Character character)
    {
        character.ModifyHungry(hungryAmount);
    }

    public override string GetTooltipDescription()
    {
        return string.Concat(
            hungryAmount > 0?"<color=#00FF00>+ ":"<color=#FF0000>- ",
            Math.Abs(hungryAmount).ToString(),
            " Hungry",
            "</color>"
        );
    }
}

