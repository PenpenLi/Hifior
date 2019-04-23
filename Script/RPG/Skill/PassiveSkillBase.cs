using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnumPassiveSkillType
{

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
    public virtual void OnUnderAttack(int damage) { }
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
