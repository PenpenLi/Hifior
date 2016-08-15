using UnityEngine;

public class MiscCoefficientSetting : ExtendScriptableObject
{
    /// <summary>
    /// 金钱持有上限
    /// </summary>
    [GUIContent("金钱上限")]
    [IntSlider(10000,999999)]
    public int MoneyUpperLimit = 99999;
    /// <summary>
    /// 被动技能持有上限
    /// </summary>
    [GUIContent("被动技能持有上限")]
    [IntSlider(3, 10)]
    public int PassiveSkillHoldUpperLimit = 5;
    /// <summary>
    /// 主动技能持有上限
    /// </summary>
    [GUIContent("主动技能持有上限")]
    [IntSlider(5, 30)]
    public int InitiativeSkillHoldUpperLimit = 5;
    /// <summary>
    /// 武器持有上限
    /// </summary>
    [GUIContent("武器持有上限")]
    [IntSlider(5, 10)]
    public int WeaponHoldUpperLimit = 5;
    /// <summary>
    /// 运输队物品持有上限
    /// </summary>
    [GUIContent("运输队物品持有上限")]
    [IntSlider(50, 200)]
    public int FerryHoldUpperLimit = 5;
    /// <summary>
    /// 低阶职业转职临界点
    /// </summary>
    [GUIContent("低阶职业转职临界点")]
    [IntSlider(10, 30)]
    public int CareerTransferLevel = 20;
    /// <summary>
    /// 职业等级上限
    /// </summary>
    [GUIContent("最大等级上限")]
    [IntSlider(10, 30)]
    public int LevelUpperLimit = 40;
    /// <summary>
    /// 攻击第二次的速度差
    /// </summary>
    [GUIContent("攻击第二次的速度差")]
    [IntSlider(1, 10)]
    public int RepeatAttackSpeedGap = 5;
    /// <summary>
    /// 自动恢复的系数
    /// </summary>
    [GUIContent("自动恢复的系数")]
    [IntSlider(0, 50)]
    public int SelfRecoveryCoefficient = 5;
    /// <summary>
    /// Boss击败额外经验值
    /// </summary>
    [GUIContent("Boss击败额外经验值")]
    [IntSlider(30, 100)]
    public int BossAdditionExp = 100;
    /// <summary>
    /// 普通小兵击败额外经验值
    /// </summary>
    [GUIContent("普通小兵击败额外经验值")]
    [IntSlider(10, 50)]
    public int KillAdditionExp = 30;
}
