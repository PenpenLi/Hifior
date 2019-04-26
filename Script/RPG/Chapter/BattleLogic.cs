using System.Collections.Generic;
using UnityEngine;
public struct BattleAttackEstimateInfo
{
    public int Hit;
    public int Damage;
    public int Times;
}
/// <summary>
/// 战斗出击的信息
/// </summary>
public struct BattleAttackInfo
{
    public CharacterLogic attacker;
    public CharacterLogic defender;
    public bool hit;
    /// <summary>
    /// 攻击方吸收的血量
    /// </summary>
    public int suckFromDefender;
    public int damageToDefender;
    /// <summary>
    /// 有些技能或者武器可能会反噬自己血量
    /// </summary>
    public int damageToAttack;
    public override string ToString()
    {
        return "攻击方:" + attacker.GetName() + "    防御方:" + defender.GetName() + "\n" + "是否命中:" + hit + "  伤害:" + damageToDefender + "  反噬伤害:" + damageToAttack + "  吸收血量:" + suckFromDefender;
    }
}
public static class BattleLogic
{
    public static int RandomInt(int start, int end)
    {
        return Random.Range(start, end);
    }
    public static float RandomFloat(float start, float end)
    {
        return Random.Range(start, end);
    }
    public static float RandomFloat01()
    {
        return Random.Range(0f, 1f);
    }
    public static bool RandomYes(int value)
    {
        return value > RandomInt(1, 100);
    }
    public static int Clamp1_99(int v)
    {
        return Mathf.Clamp(v, 1, 99);
    }
    /// <summary>
    /// 有些技能可以反击
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    public static bool IsCounterAttack(CharacterLogic attacker, CharacterLogic defender)
    {
        return attacker.GetSpeed() < defender.GetSpeed();
    }
    /// <summary>
    /// 考虑武器能力特效，克制关系，职业特效，技能修正等
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    public static int GetDamage(CharacterLogic attacker, CharacterLogic defender)
    {
        return attacker.GetAttack() - defender.GetPhysicalDefense();
    }
    /// <summary>
    /// 技能或者武器特效等
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    public static int GetHit(CharacterLogic attacker, CharacterLogic defender)
    {
        return attacker.GetHit() - defender.GetAvoid();
    }
    /// <summary>
    /// 武器或者技能触发多次连续攻击
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    public static int GetAttackTimes(CharacterLogic attacker, CharacterLogic defender)
    {
        return 1;
    }
    public static bool IsHurtSelf(CharacterLogic attacker, CharacterLogic defender)
    {
        return false;
    }
    public static int GetSuckedHP(CharacterLogic attacker, int damage)
    {
        return 0;
    }
    public static List<BattleAttackEstimateInfo> GetEstimateAttackInfo(CharacterLogic attacker, CharacterLogic defender)
    {
        List<BattleAttackEstimateInfo> r = new List<BattleAttackEstimateInfo>();
        BattleAttackEstimateInfo i = new BattleAttackEstimateInfo();
        i.Damage = GetDamage(attacker, defender);
        i.Hit = GetHit(attacker, defender);
        i.Times = GetAttackTimes(attacker, defender);
        r.Add(i);
        if (IsCounterAttack(attacker, defender))
        {
            BattleAttackEstimateInfo j = new BattleAttackEstimateInfo();
            j.Damage = GetDamage(defender, attacker);
            j.Hit = GetHit(defender, attacker);
            j.Times = GetAttackTimes(defender, attacker);
            r.Add(j);
        }
        return r;
    }
    public static List<BattleAttackInfo> GetAttackInfo(CharacterLogic attacker, CharacterLogic defender)
    {
        List<BattleAttackInfo> r = new List<BattleAttackInfo>();
        var atkA = attacker.Info.Attribute;
        var defA = defender.Info.Attribute;
        BattleAttackInfo i = new BattleAttackInfo();
        i.attacker = attacker;
        i.defender = defender;
        if (IsHurtSelf(i.attacker, i.defender))
        {
            i.damageToAttack = GetDamage(i.attacker, i.attacker);
            i.damageToDefender = 0;
            i.suckFromDefender = 0;
            i.hit = true;
        }
        else
        {
            i.damageToAttack = 0;
            i.damageToDefender = GetDamage(i.attacker, i.defender);
            i.suckFromDefender = GetSuckedHP(i.attacker, i.damageToAttack);
            i.hit = RandomYes(GetHit(i.attacker, i.defender)); 
        }
        r.Add(i);
        if (IsCounterAttack(attacker, defender))
        {
            BattleAttackInfo j = new BattleAttackInfo();
            j.attacker = defender;
            j.defender = attacker;
            if (IsHurtSelf(j.attacker, j.defender))
            {
                j.damageToAttack = 0;
                j.suckFromDefender = 0;
                j.damageToDefender = GetDamage(j.attacker, j.attacker);
                j.hit = true;
            }
            else
            {
                j.damageToAttack = GetDamage(j.attacker, j.defender);
                j.damageToDefender = 0;
                j.suckFromDefender = GetSuckedHP(j.attacker, j.damageToAttack);
                j.hit = RandomYes(GetHit(j.attacker, j.defender));
            }
            r.Add(j);
        }
        return r;
    }
}
