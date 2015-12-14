using System;
using System.Collections.Generic;

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
public enum EnumWeaponRangeType
{
    中心菱形,
    正方形,
    十字形,
    菱形菱形
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
public static class EnumTables
{
    /// <summary>
    /// 根据int位获取符合枚举类型的枚举值
    /// </summary>
    /// <param name="enumValue"></param>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static List<int> GetIntListByValue(int enumValue, Type enumType)
    {
        List<int> l = new List<int>();
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
}