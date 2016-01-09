using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 人物在战场上的战斗阶段状态,与被动技能的不同点在于其状态可以进行开关,而技能会一直都存在,持有某个物体或者使用某种药会增加Buff
/// </summary>
public class CombatBuffDef : ExtendScriptableObject
{
    public PropertyIDNameDesc CommonProperty;
    public Sprite Icon;

    public EnumCombatBuffDisapper DisapperCondition;
    public int DisapperRound = 5;
    public bool DisapperOnExitBattle = true;

    public EnumBuffSkillTrigger EventTrigger;
    public EnumCombatBuffEffect Effect;
    public CharacterAttribute AttributeChange;
    /// <summary>
    /// 参数
    /// </summary>
    public List<int> Params;

}