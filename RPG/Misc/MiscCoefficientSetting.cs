using UnityEngine;
using System.Collections;

public class MiscCoefficientSetting : ExtendScriptableObject
{
    /// <summary>
    /// 金钱持有上限
    /// </summary>
    public int MoneyUpperLimit = 99999;
    /// <summary>
    /// 被动技能持有上限
    /// </summary>
    public int PassiveSkillHoldUpperLimit = 5;
    /// <summary>
    /// 主动技能持有上限
    /// </summary>
    public int InitiativeSkillHoldUpperLimit = 5;
    /// <summary>
    /// 武器持有上限
    /// </summary>
    public int WeaponHoldUpperLimit = 5;
    /// <summary>
    /// 运输队物品持有上限
    /// </summary>
    public int FerryHoldUpperLimit = 5;
    /// <summary>
    /// 低阶职业转职临界点
    /// </summary>
    public int CareerTransferLevel = 20;
    /// <summary>
    /// 职业等级上限
    /// </summary>
    public int LevelUpperLimit = 40;
    /// <summary>
    /// 攻击第二次的速度差
    /// </summary>
    public int RepeatAttackSpeedGap = 5;
    /// <summary>
    /// 自动恢复的系数
    /// </summary>
    public int SelfRecoveryCoefficient = 5;
    /// <summary>
    /// Boss击败额外经验值
    /// </summary>
    public int BossAdditionExp = 100;
    /// <summary>
    /// 普通小兵击败额外经验值
    /// </summary>
    public int KillAdditionExp = 30;
}
