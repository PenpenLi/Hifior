using System;
using System.Collections.Generic;
public enum EnumOccupyStatus
{
    None,
    Player,
    Enemy
}
public enum EnumCharacterCamp
{
    Player,
    Enemy,
    Ally,
    NPC
}
public enum EnumItemType
{
    武器,
    道具
}
public enum EnumEnemyActionAI
{
    /// <summary>
    /// 持有武器则进行攻击，持有治疗杖责进行治疗
    /// </summary>
    行动型,
    待机型,
    移动型,
    破坏村庄型,
    盗取宝箱型
}
public enum EnumEventTriggerCondition
{
    /// <summary>
    /// 每回合之间触发
    /// </summary>
    回合事件,
    /// <summary>
    /// 到达特定的地点进行触发,比如村庄宝箱
    /// </summary>
    位置事件,
    /// <summary>
    /// 到达指定坐标进行待机后触发
    /// </summary>
    范围事件,
    /// <summary>
    /// 敌人数量小于等于一定数量触发
    /// </summary>
    敌人少于事件,
    /// <summary>
    /// 某个人物死亡后触发,通常设在Boss和NPC身上
    /// </summary>
    死亡事件
}
public enum EnumEnemyCureSelfCondition
{
    不进行治疗,
    HP低于一半,
    HP低于四分之一,
    HP低于一半随机
}
/// <summary>
/// 胜利条件
/// </summary>
public enum EnumWinCondition
{
    全灭敌人,
    击败指定Boss,
    击败全部Boss,
    压制指定城池,
    压制所有城池,
    回合坚持,
    领主地点撤离
}
/// <summary>
/// 失败条件
/// </summary>
public enum EnumFailCondition
{
    //主角死亡，必有
    人物死亡,
    城池被夺,
    回合达到
}
/// <summary>
/// 天气
/// </summary>
public enum EnumWeather
{
    白天,
    夜晚,
    下雨,
    雾天,
    沙城暴,
    雪天
}
/// <summary>
/// 动画触发点
/// </summary>
public enum EnumBuffSkillTrigger
{
    自己移动时,
    自己被攻击时,
    主动攻击开始,
    被攻击时,
    自己死亡时,
    回合开始时,
    战斗开始时,
    战斗结束时,
    战斗胜利时,
    达到特定回合数,
    每间隔一定回合,
    受到伤害时,
    自身HP大于或小于某百分比时,
    /// <summary>
    /// 购物有会员卡在身上即添加会员Buff
    /// </summary>
    购物时
}
public enum EnumCombatBuffEffect
{
    /// <summary>
    /// 仅仅作为一个标识符,逻辑不涉及其他的因素,例如骑乘状态
    /// </summary>
    无,
    人物属性百分比改变,
    人物属性固定改变,
    攻击属性百分比改变,
    攻击属性固定改变,
    每回合掉百分比HP,
    无法行动,
    无法攻击,
    攻击所有单位,
    攻击己方单位
}

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
    必杀无效
}
public enum EnumCombatBuffDisapper
{
    回合计时,
    持续待打破
}
public enum EnumCharacterImportance
{
    Worker,
    Leader,
    SecondLeader
}
public enum EnumCareerLevel
{
    Low,
    Middle,
    High
}

public enum EnumCareerModelSize
{
    Small,
    Middle,
    Large
}
/// <summary>
/// 兵种
/// </summary>
public enum EnumCareerSeries
{
    战士系,
    骑乘系,
    重装系,
    飞行系,
    魔法系,
    暗黑系,
    光明系,
    龙系
}
public enum EnumWeaponType
{
    剑,
    枪,
    斧,
    弓弩,
    火枪,
    炎爆,
    冰冻,
    光明,
    暗黑,
    治疗,
}
public enum EnumWeaponLevel
{
    D,
    C,
    B,
    A,
    S,
    SSS,
    星
}
public enum EnumWeaponRangeType
{
    菱形菱形,
    十字形,
    正方形,
    中心菱形,
    扇形,
}
public enum EnumWeaponAttackEffectType
{
    无,
    HP吸收,
    防御力无视,
    防御力减半,
    HP强制为1,
    对方不可反击,
    对方不可必杀
}
public enum EnumPropsEffectType
{
    治疗,
    解毒,
    防御增加
}
public static class EnumTables
{
    /// <summary>
    /// 根据enumValue获取哪一位为选中状态
    /// </summary>
    /// <param name="enumValue"></param>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static List<int> GetIntListByValue(int enumValue, Type enumType)
    {
        List<int> l = new List<int>();
        if (enumValue == 0)
            return l;
        int enumCount = Enum.GetNames(enumType).Length;
        int bit = enumValue < 0 ? enumValue + (1 << enumCount) : enumValue;
        if (bit != 0)
        {
            for (int i = 0; i < enumCount; i++)
            {
                if ((bit & (1 << i)) == (1 << i))
                {
                    l.Add(i);
                }
            }
        }
        return l;
    }
    /// <summary>
    /// 根据int位获取符合枚举类型的String型Enum Name 的List
    /// </summary>
    /// <param name="enumValue"></param>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static List<string> GetStringListByValue(int enumValue, Type enumType)
    {
        List<string> l = new List<string>();
        if (enumValue == 0)
            return l;
        int enumCount = Enum.GetNames(enumType).Length;
        int bit = enumValue < 0 ? enumValue + (1 << enumCount) : enumValue;
        if (bit != 0)
        {
            for (int i = 0; i < enumCount; i++)
            {
                if ((bit & (1 << i)) == (1 << i))
                {
                    l.Add(Enum.GetName(enumType, i));
                }
            }
        }
        return l;
    }
    /// <summary>
    /// 如果位为0则代表存在这个位上的int值
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="bitPosition"></param>
    /// <returns></returns>
    public static bool MaskFieldIdentify(int mask, int bitPosition)
    {
        int bitInt = (1 << ((int)bitPosition));
        return (~mask & bitInt) != bitInt;
    }
    /// <summary>
    /// 将某一位设为0
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="bitPosition"></param>
    /// <returns></returns>
    public static int MaskFieldSetTrue(int mask, int bitPosition)
    {
        int bitInt = ~(1 << ((int)bitPosition));
        return mask & bitInt;
    }
    /// <summary>
    /// 将某一位设为1
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="bitPosition"></param>
    /// <returns></returns>
    public static int MaskFieldSetFalse(int mask, int bitPosition)
    {
        int bitInt = (1 << ((int)bitPosition));
        return mask | bitInt;
    }
    /// <summary>
    /// 获取从0开始递增为1的 长度为length的数组
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static int[] GetSequentialArray(int length)
    {
        int[] x = new int[length];
        for (int i = 0; i < length; i++)
        {
            x[i] = i;
        }
        return x;
    }
    public static int[] GetPowArray(int length)
    {
        int[] x = new int[length];
        for (int i = 0; i < length; i++)
        {
            x[i] = 1 << i;
        }
        return x;
    }
    public static bool[] GetTrueArray(int length)
    {
        bool[] b = new bool[length];
        for (int i = 0; i < length; i++)
        {
            b[i] = true;
        }
        return b;
    }
}
