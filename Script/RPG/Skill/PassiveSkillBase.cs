using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumPassiveSkillEffect
{
    人物属性百分比改变,
    人物属性固定改变,
    攻击属性百分比改变,
    攻击属性固定改变,
    获得双倍经验值,
    回复百分比HP,
    攻击不会被反击,
    攻击吸取HP,
    总是先制攻击,
    不论攻击多少次耐久总是减一,
    必杀无效,
    神行者,
    None,
}
public enum EnumPassiveSkillType
{
    None,
    必杀无效,
    神行者,
}
public struct PassiveSkillState
{
    public static readonly PassiveSkillState None;
    public EnumPassiveSkillType Type;
    public EnumPassiveSkillEffect Effect;
    public Dictionary<string, int> ParamsMap;
    private static PassiveSkillState Create(EnumPassiveSkillType type, EnumPassiveSkillEffect effect, Dictionary<string, int> param)
    {
        PassiveSkillState r = new PassiveSkillState();
        r.Type = type;
        r.Effect = effect;
        r.ParamsMap = param;
        return r;
    }
    public bool Valid() { return Type != EnumPassiveSkillType.None; }
    public static Dictionary<EnumPassiveSkillType, PassiveSkillState> PassiveSkills = new Dictionary<EnumPassiveSkillType, PassiveSkillState>();
    static PassiveSkillState()
    {
        None = Create(EnumPassiveSkillType.None, EnumPassiveSkillEffect.None, null);
        PassiveSkills.Add(EnumPassiveSkillType.神行者, Create(EnumPassiveSkillType.神行者, EnumPassiveSkillEffect.神行者, null));
    }
   
}

public abstract class PassiveSkillBase
{
    protected CharacterLogic logic;
    public abstract string GetName();
    public abstract string Description();
    /// <summary>
    /// 回合触发
    /// </summary>
    public virtual void OnStartTurn(int turn) { }
    /// <summary>
    /// 当自己的回合所有玩家行动结束的时候
    /// </summary>
    public virtual void OnEndTurn(int turn) { }
    /// <summary>
    /// 当角色死亡的时候
    /// </summary>
    public virtual void OnDead() { }
    /// <summary>
    /// 当战斗胜利的时候
    /// </summary>
    public virtual void OnWinBattle() { }
    /// <summary>
    /// 当战斗收到伤害进行结算的时候
    /// </summary>
    public virtual void OnUnderDamaged(int damage) { }
    /// <summary>
    /// 当遭受攻击的时候
    /// </summary>
    public virtual void OnUnderAttack() { }
    public virtual void OnUnderHeal() { }
    public virtual void OnUnderSteal() { }
    /// <summary>
    /// 在攻击之前
    /// </summary>
    public virtual void OnStartAttack() { }
    public virtual void OnEndAttack() { }
    public virtual void OnStartMove() { }
    public virtual void OnEndMove() { }
}
