using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillLogic
{
    /// <summary>
    /// 改变逻辑值
    /// </summary>
    /// <param name="att"></param>
    void ChangeValue(CharacterAttribute att);
    /// <summary>
    /// 回退结果
    /// </summary>
    /// <param name="att"></param>
    void RevertValue(CharacterAttribute att);
}

public abstract class SkillBase
{
    protected CharacterLogic logic;
    public abstract string GetName();
    public abstract string Description();
}
public abstract class ActionSkillBase : SkillBase
{

}
public abstract class BufferBase
{
    public abstract EnumBufferType Type();
    public abstract EnumBufferTrigger TriggerCondition();
    public CombatBufferDef Def;
}
