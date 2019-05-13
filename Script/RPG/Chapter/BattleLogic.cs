using System.Collections.Generic;
using UnityEngine;
public struct BattleAttackEstimateInfo
{
    public int Hit;
    public int Damage;
    public int Times;
}
[System.Serializable]
public struct LevelUPInfo
{
    [System.Serializable]
    public struct AbilityData
    {
        public int[] original, add;
        public AbilityData(int[] ori, int[] _add)
        {
            original = ori;
            add = _add;
        }
    }
    public int startExp;
    public int endExp;
    public int startLevel;
    public int endLevel;
    public List<AbilityData> abilityData;
}
/// <summary>
/// 战斗出击的信息
/// </summary>
public class BattleAttackInfo
{
    public CharacterLogic attacker;
    public CharacterLogic defender;
    public BattleAttackInfo(CharacterLogic att, CharacterLogic def) { attacker = att; defender = def; }
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
    public void Process()
    {
        if (BattleLogic.IsHurtSelf(attacker, defender))
        {
            damageToAttack = BattleLogic.GetDamage(attacker, attacker);
            damageToDefender = 0;
            suckFromDefender = 0;
            hit = true;
        }
        {
            damageToAttack = 0;
            damageToDefender = BattleLogic.GetDamage(attacker, defender);
            suckFromDefender = BattleLogic.GetSuckedHP(attacker, damageToDefender);
            hit = BattleLogic.RandomYes(BattleLogic.GetHit(attacker, defender));
        }
    }
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
        var hit = attacker.GetHit() - defender.GetAvoid();
        hit = Mathf.Clamp(hit, 0, 100);
        return hit;
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
    public static int GetAttackCount(CharacterLogic attacker, CharacterLogic defender)
    {
        return 1;
    }
    public static int GetAttackDamage(CharacterLogic attacker, CharacterLogic defender)
    {
        int dmg = attacker.GetAttack() - defender.GetPhysicalDefense();
        return Mathf.Max(0, dmg);
    }
    public static int GetGrowValue(int grow)
    {
        return RandomYes(grow) ? 1 : 0;
    }
    public static CharacterAttribute GetGrow(CharacterAttributeGrow grow)
    {
        CharacterAttribute r = new CharacterAttribute();
        r.HP = GetGrowValue(grow.HP);
        r.PhysicalPower = GetGrowValue(grow.PhysicalPower);
        r.MagicalPower = GetGrowValue(grow.MagicalPower);
        r.Skill = GetGrowValue(grow.Skill);
        r.Speed = GetGrowValue(grow.Speed);
        r.Intel = GetGrowValue(grow.Intel);
        r.PhysicalDefense = GetGrowValue(grow.PhysicalDefense);
        r.MagicalDefense = GetGrowValue(grow.MagicalDefense);
        r.BodySize = GetGrowValue(grow.BodySize);
        r.Movement = GetGrowValue(grow.Movement);
        return r;
    }
    public static LevelUPInfo GetAttackExp(CharacterLogic logic, int defenderCareerRank, int defenderLevel, bool isEnemyDead, int damage, bool isCritical)
    {
        int attackerLevel = logic.GetLevel();
        int attackerExp = logic.GetExp();
        int careerRankDiff = 0;
        LevelUPInfo r = new LevelUPInfo();
        int levelDiff = attackerLevel - defenderLevel;
        int exp = careerRankDiff * 20 + levelDiff + 10;
        exp = Mathf.Max(exp, 2);
        if (isEnemyDead)
        {
            exp *= 3;
        }
        else
        {
            if (isCritical)
            {
                exp *= 2;
            }
        }
        exp = Mathf.Min(100, exp);
        r.startExp = attackerExp;
        int finalExp = r.startExp + exp;
        r.endExp = finalExp % 100;
        r.startLevel = attackerLevel;
        r.endLevel = attackerLevel + finalExp / 100;
        if (r.endLevel > r.startExp)
        {
            CharacterAttribute add = GetGrow(logic.GetAttributeGrow());

            r.abilityData = new List<LevelUPInfo.AbilityData>();
            LevelUPInfo.AbilityData d = new LevelUPInfo.AbilityData(logic.GetAttribute().Array, add.Array);
            r.abilityData.Add(d);
        }
        return r;
    }

    public static bool IsDead(BattleAttackInfo info, CharacterLogic logic)
    {
        return (info.hit && logic.GetCurrentHP() < info.damageToDefender);
    }
    public static List<BattleAttackInfo> GetAttackInfo(CharacterLogic player, CharacterLogic enemy)
    {
        List<BattleAttackInfo> r = new List<BattleAttackInfo>();
        var atkA = player.Info.Attribute;
        var defA = enemy.Info.Attribute;
        BattleAttackInfo i = new BattleAttackInfo(player, enemy);
        i.Process();
        r.Add(i);
        if (IsDead(i, enemy) == false && IsCounterAttack(player, enemy))
        {
            BattleAttackInfo j = new BattleAttackInfo(enemy, player);
            j.Process();
            r.Add(j);
        }
        return r;
    }
}
