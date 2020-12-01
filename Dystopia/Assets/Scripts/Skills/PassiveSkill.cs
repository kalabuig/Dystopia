using UnityEngine;


public enum SkillLevel {
    Basic,
    Good,
    Great,
    Expert,
    GreatExpert,
    Professional,
}

public enum SkillModifierType {
    None,
    StatAbsolute,
    StatPercentage,
}

public enum CharacterModifier {
    healthRate,
    thirstRate,
    hungryRate,
    vigorRate,
    maxHealth,
    maxHungry,
    maxThirst,
    maxVigor,
}

public enum SpecialModifier {
    None,
    Healing,
}

[CreateAssetMenu(menuName = "Passive Skill")]
public class PassiveSkill : ScriptableObject
{
    [Header("Skill Description")]
    [SerializeField] Sprite _skillSprite;
    public Sprite skillSprite { get => _skillSprite;}
    [SerializeField] string _skillName;
    public string skillName { get => _skillName;}
    [SerializeField] string _skillDescription;
    public string skillDescription { get => _skillDescription;}
    [Space]
    [Header("Skill Especifications")]
    public SkillLevel skillLevel = SkillLevel.Basic;
    public int[] valuesPerLevel; //Amount to use for each passive level
    public SkillModifierType skillModifierType = SkillModifierType.None;
    [Space]
    [Header("Stat Modifiers")]
    public StatsModifiers.Modifier[] statModifiers;
    [Space]
    [Header("Player Modifiers")]
    public CharacterModifier[] characterModifiers;
    [Space]
    [Header("Special Modifiers")]
    public SpecialModifier[] specialModifiers; 

    public static string EnumToString(SkillLevel sl) {
        return System.Enum.GetName(typeof(SkillLevel), sl);
    }

    public static string EnumToString(SkillModifierType smt) {
        return System.Enum.GetName(typeof(SkillModifierType), smt);
    }

    public static string EnumToString(CharacterModifier cm) {
        return System.Enum.GetName(typeof(SkillModifierType), cm);
    }

        public static string EnumToString(SpecialModifier sm) {
        return System.Enum.GetName(typeof(SkillModifierType), sm);
    }

    public static object GetPropValue(object src, string propName) {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }

    public static object GetFieldValue(object src, string propName) {
        return src.GetType().GetField(propName).GetValue(src);
    }

}
