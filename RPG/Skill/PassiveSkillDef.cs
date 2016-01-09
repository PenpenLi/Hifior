using UnityEngine;
using System.Collections;

public class PassiveSkillDef : ExtendScriptableObject
{
    public PropertyIDNameDesc CommonProperty;
    public Sprite Icon;
    public EnumBuffSkillTrigger EventTrigger;
    public EnumPassiveSkillEffect Effect;
    public CharacterAttribute AttributeChange;
}