using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WeaponDef : ExtendScriptableObject
{
    public PropertyIDNameDesc CommonProperty;

    public EnumWeaponType WeaponType;
    public EnumWeaponLevel WeaponLevel;
    public Sprite Icon;
    public int SinglePrice;
    public int UseNumber;
    public SelectRangeType RangeType;
    public int Weight;
    public int Power;
    public int Hit;
    public int Crit;
    /// <summary>
    /// 人物专用
    /// </summary>
    public List<int> DedicatedCharacter;

    /// <summary>
    /// 职业专用
    /// </summary>
    public List<int> DedicatedJob;

    /// <summary>
    /// 对那些系的职业有特效
    /// </summary>
    public List<int> CareerEffect;
    public int SuperEffect;
    public EnumWeaponAttackEffectType AttackEffect;
    public bool ImportantWeapon;
    public bool NoExchange;

    //添加的额外属性
    public CharacterAttribute AdditionalAttribute;
    //成长率提高
    public CharacterAttributeGrow AdditionalAttributeGrow;

    public const int SWORD = 1;
    public const int SPEAR = 2;
    public const int AXE = 3;
    public const int ANCHOR = 4;
    public const int NATURE = 5;
    public const int LIGHT = 6;
    public const int DARK = 7;
    public const int SPECIAL = 8;

    public const int CONSUMEITEM = 11;
    public const int PASSIVEITEM = 12;

    private enum Effent1
    {
        二次攻击 = 1,
        带毒 = 2,
        吸血 = 4,
        间接攻击 = 8,
        沉默 = 16,
        睡眠 = 32,
        狂暴 = 64,
        耐久无限 = 128,
        魔抗减半 = 256,
        防御减半 = 512,
        hp直接减半 = 1024, 不可必杀 = 2048,
        移动后不可使用 = 4096,
        伤害减半 = 8192,
        反向克制 = 16384,
        全克制 = 32768
    }
    private enum Effent2
    {
        天马特效 = 1,
        重甲特效 = 2,
        骑兵特效 = 4,
        魔物特效 = 8,
        法师特效 = 16,
        龙特效 = 32
    }

    public int GetID()
    {
        return CommonProperty.ID;
    }
    public string GetWeaponTypeName()
    {
        return WeaponType.ToString();
    }
    public string GetWeaponLevelName()
    {
        return WeaponLevel.ToString();
    }
    public CharacterAttribute GetAdditionalAbility()
    {
        return AdditionalAttribute;
    }

    public bool IsPhysicalWeapon()
    {
        if (WeaponType <= EnumWeaponType.火枪)
            return true;
        else
            return false;
    }
    public bool IsMagicalWeapon()
    {
        if (WeaponType >= EnumWeaponType.炎爆 && WeaponType <= EnumWeaponType.暗黑)
            return true;
        else
            return false;
    }
    public bool IsWeaponType()//是否是武器类型 1-8是武器
    {
        if (WeaponType < EnumWeaponType.治疗)
            return true;
        else
            return false;
    }

    public bool IsSpecialType(int id)
    {
        if ((int)WeaponType == 10)
            return true;
        else
            return false;
    }

    public bool RefrainCareer(int CareerID)
    {
        return CareerEffect.Contains(CareerID);
    }

    public EnumWeaponAttackEffectType GetAttackEffect()
    {
        return AttackEffect;
    }

    public string GetDescription()
    {
        return CommonProperty.Description;
    }
    
    public int GetPrice()
    {
        return SinglePrice * UseNumber;
    }

    public bool DedicatedUse(int CharacterID, int CareerID)
    {
        bool bCharacter = false;
        bool bCareer = false;
        if (DedicatedCharacter.Count == 0 || DedicatedCharacter.Contains(CharacterID))
        {
            bCharacter = true;
        }

        if (DedicatedJob.Count != 0 || DedicatedJob.Contains(CareerID))
        {
            bCareer = true;
        }
        return bCharacter && bCareer;
    }

    public int GetUsageTime()
    {
        return UseNumber;
    }

    public bool IsHaveSuperEffect(int SuperEffectType)
    {
        return EnumTables.MaskFieldIdentify(SuperEffect, SuperEffectType);
    }
    /*
        #region Effect1判断
        public bool isDoubleAttack(int id)
        {
            if ((effect1[id] & (int)Effent1.二次攻击) != 0)
                return true;
            return false;
        }
        public bool isPoison(int id)
        {
            if ((effect1[id] & (int)Effent1.带毒) != 0)
                return true;
            return false;
        }
        public bool isSuckBlood(int id)
        {
            if ((effect1[id] & (int)Effent1.吸血) != 0)
                return true;
            return false;
        }
        public bool isIndirectAttack(int id)
        {
            if ((effect1[id] & (int)Effent1.间接攻击) != 0)
                return true;
            return false;
        }

        public bool isSilent(int id)
        {
            if ((effect1[id] & (int)Effent1.沉默) != 0)
                return true;
            return false;
        }

        public bool isSleep(int id)
        {
            if ((effect1[id] & (int)Effent1.睡眠) != 0)
                return true;
            return false;
        }
        public bool isCrazy(int id)
        {
            if ((effect1[id] & (int)Effent1.狂暴) != 0)
                return true;
            return false;
        }
        public bool isUnlimitedUse(int id)
        {
            if ((effect1[id] & (int)Effent1.耐久无限) != 0)
                return true;
            return false;
        }
        public bool isHalfMdef(int id)
        {
            if ((effect1[id] & (int)Effent1.魔抗减半) != 0)
                return true;
            return false;
        }
        public bool isHalfDef(int id)
        {
            if ((effect1[id] & (int)Effent1.防御减半) != 0)
                return true;
            return false;
        }
        public bool isHalfHP(int id)
        {
            if ((effect1[id] & (int)Effent1.hp直接减半) != 0)
                return true;
            return false;
        }
        public bool isAvoidCritical(int id)
        {
            if ((effect1[id] & (int)Effent1.不可必杀) != 0)
                return true;
            return false;
        }
        public bool isUnableUseAfterMove(int id)
        {
            if ((effect1[id] & (int)Effent1.移动后不可使用) != 0)
                return true;
            return false;
        }
        public bool isHalfDamage(int id)
        {
            if ((effect1[id] & (int)Effent1.伤害减半) != 0)
                return true;
            return false;
        }
        public bool isReverseRestrain(int id)
        {
            if ((effect1[id] & (int)Effent1.反向克制) != 0)
                return true;
            return false;
        }

        public bool isAllRestrain(int id)
        {
            if ((effect1[id] & (int)Effent1.全克制) != 0)
                return true;
            return false;
        }
        #endregion
        #region Effect2判断
        public bool isHeavyinfantryRestrain(int id)
        {
            if ((effect2[id] & (int)Effent2.重甲特效) != 0)
                return true;
            return false;
        }
        public bool isAirForceRestrain(int id)
        {
            if ((effect2[id] & (int)Effent2.天马特效) != 0)
                return true;
            return false;
        }
        public bool isCavalryRestrain(int id)
        {
            if ((effect2[id] & (int)Effent2.骑兵特效) != 0)
                return true;
            return false;
        }
        public bool isMasterRestrain(int id)
        {
            if ((effect2[id] & (int)Effent2.法师特效) != 0)
                return true;
            return false;
        }
        public bool isDragonRestrain(int id)
        {
            if ((effect2[id] & (int)Effent2.龙特效) != 0)
                return true;
            return false;
        }
        public bool isDemonRestrain(int id)
        {
            if ((effect2[id] & (int)Effent2.魔物特效) != 0)
                return true;
            return false;
        }
        #endregion
        public override void setupData(string[][] paramArrayOfString)
        {
            if (paramArrayOfString == null) return;
            int i = paramArrayOfString.Length;
            this.key = new int[i];
            this.name = new string[i];
            this.price = new int[i];
            this.type = new int[i];
            this.power = new int[i];
            this.hit = new int[i];
            this.weight = new int[i];
            this.critical = new int[i];
            this.rangeType = new int[i];
            this.rangeMin = new int[i];
            this.rangeMax = new int[i];
            this.requireType = new int[i];
            this.requireValue = new int[i];
            this.usage = new int[i];
            this.effect1 = new int[i];
            this.effect2 = new int[i];
            this.help = new string[i];
            this.extra_str = new int[i];
            this.extra_wis = new int[i];
            this.extra_dex = new int[i];
            this.extra_agi = new int[i];
            this.extra_def = new int[i];
            this.extra_res = new int[i];
            this.extra_luk = new int[i];
            for (int j = 0; j < i; j++)
            {
                this.key[this.count] = Convert.ToInt32(paramArrayOfString[j][0]);
                this.name[this.count] = paramArrayOfString[j][1];
                this.price[this.count] = Convert.ToInt32(paramArrayOfString[j][2]);
                this.type[this.count] = Convert.ToInt32(paramArrayOfString[j][3]);
                this.power[this.count] = Convert.ToInt32(paramArrayOfString[j][4]);
                this.hit[this.count] = Convert.ToInt32(paramArrayOfString[j][5]);
                this.weight[this.count] = Convert.ToInt32(paramArrayOfString[j][6]);//重量
                this.critical[this.count] = Convert.ToInt32(paramArrayOfString[j][7]);
                this.rangeType[this.count] = Convert.ToInt32(paramArrayOfString[j][8]);
                this.rangeMin[this.count] = Convert.ToInt32(paramArrayOfString[j][9]);
                this.rangeMax[this.count] = Convert.ToInt32(paramArrayOfString[j][10]);
                this.requireType[this.count] = Convert.ToInt32(paramArrayOfString[j][11]);
                this.requireValue[this.count] = Convert.ToInt32(paramArrayOfString[j][12]);
                this.usage[this.count] = Convert.ToInt32(paramArrayOfString[j][13]);
                this.effect1[this.count] = Convert.ToInt32(paramArrayOfString[j][14]);
                this.effect2[this.count] = Convert.ToInt32(paramArrayOfString[j][15]);
                this.help[this.count] = paramArrayOfString[j][16];

                this.extra_str[this.count] = Convert.ToInt32(paramArrayOfString[j][17]);
                this.extra_wis[this.count] = Convert.ToInt32(paramArrayOfString[j][18]);
                this.extra_dex[this.count] = Convert.ToInt32(paramArrayOfString[j][19]);
                this.extra_agi[this.count] = Convert.ToInt32(paramArrayOfString[j][20]);
                this.extra_def[this.count] = Convert.ToInt32(paramArrayOfString[j][21]);
                this.extra_res[this.count] = Convert.ToInt32(paramArrayOfString[j][22]);
                this.extra_luk[this.count] = Convert.ToInt32(paramArrayOfString[j][23]);
                this.count = (int)(1 + this.count);
            }
        }
        public override void clear()
        {
            this.count = 0;
            int i = this.key.Length;
            for (int j = 0; j < i; j++)
            {
                this.key[j] = 0;
                this.name[j] = null;
                this.price[j] = 0;
                this.type[j] = 0;
                this.power[j] = 0;
                this.hit[j] = 0;
                this.weight[j] = 0;
                this.critical[j] = 0;
                this.rangeType[i] = 0;
                this.rangeMin[j] = 0;
                this.rangeMax[j] = 0;
                this.requireType[j] = 0;
                this.requireValue[j] = 0;
                this.usage[j] = 0;
                this.effect1[j] = 0;
                this.effect2[j] = 0;
                this.help[j] = null;
                this.extra_str[j] = 0;//力量
                this.extra_wis[j] = 0;//魔力
                this.extra_dex[j] = 0;//技术
                this.extra_agi[j] = 0;//速度
                this.extra_def[j] = 0;//守备
                this.extra_res[j] = 0;//魔抗
                this.extra_luk[j] = 0;//幸运
            }
        }
        */
}
