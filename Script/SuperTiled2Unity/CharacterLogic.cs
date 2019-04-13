using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic
{
    public CharacterLogic(CharacterDef def)
    {
        Info = new CharacterInfo(def);
        Item = new ItemGroup(GetAttribute());
    }
    public CharacterLogic() { }
    /// <summary>
    /// 包含需要被序列化记录的数据
    /// </summary>
    public CharacterInfo Info { private set; get; }

    public CharacterDef characterDef;
    public CareerDef careerDef;
    public ItemGroup Item { private set; get; }

    /// <summary>
    /// 是否可以操控行动，行动完毕或者被石化，冻住等则为False
    /// </summary>
    protected bool bEnableAction = true;

    /// <summary>
    /// 是否可以被玩家选择并进行行动
    /// </summary>
    public bool Controllable
    {
        get
        {
            return bEnableAction;
        }
    }
    /// <summary>
    /// 是否在移动中
    /// </summary>
    protected bool bRunning = false;
    /// <summary>
    /// 是否在攻击过程中
    /// </summary>
    protected bool bAttacking = false;
    protected int damageCount = 0;//收到伤害和造成伤害的次数
    [SerializeField]
    protected Vector2Int tileCoords;

    protected Vector2Int oldTileCoords;

    #region get
    public CharacterAttribute GetAttribute() { return Info.Attribute; }

    public int GetMovement()
    {
        return 6;
    }
    public Vector2Int GetTileCoord()
    {
        return tileCoords;
    }
    public Vector2Int GetOldTileCoord()
    {
        return oldTileCoords;
    }
    public EnumCharacterImportance Importance
    {
        get
        {
            return characterDef.CharacterImportance;
        }
    }
    public int GetDefaultCareer()
    {
        return characterDef.Career;
    }
    public CharacterAttribute GetDefaultAttribute()
    {
        return characterDef.DefaultAttribute;
    }

    public int GetLevel()
    {
        return Info.Level;
    }
    public int GetCareer()
    {
        return Info.Career;
    }
    public string GetCareerName()
    {
        return null;
    }
    public int GetMaxHP()
    {
        return Info.MaxHP;
    }
    public int GetCurrentHP()
    {
        return Info.Exp;
    }
    public int GetExp()
    {
        return Info.Exp;
    }

    public object GetPhysicalPower()
    {
        return Info.Attribute.PhysicalPower;
    }

    public object GetSkill()
    {
        return Info.Attribute.Skill;
    }

    public object GetSpeed()
    {
        return Info.Attribute.Speed;
    }

    public object GetLuck()
    {
        return Info.Attribute.Luck;
    }

    public object GetMagicalDefense()
    {
        return Info.Attribute.MagicalDefense;
    }

    public object GetPhysicalDefense()
    {
        return Info.Attribute.PhysicalDefense;
    }

    public object GetMagicalPower()
    {
        return Info.Attribute.MagicalPower;
    }
    #endregion
    #region set
    public void SetAttribute(CharacterAttribute InAttribute)
    {
        Info.Attribute = (CharacterAttribute)InAttribute.Clone();
        Info.Attribute.HP = InAttribute.HP;
    }
    public void SetTileCoord(Vector2Int tilePos)
    {
        oldTileCoords = tileCoords;
        tileCoords = tilePos;
    }
    public void SetCareer(int career)
    {
        Info.Career = career;
    }
    public void SetLevel(int level)
    {
        Info.Level = level;
    }
    #endregion


    #region 人物战斗获取函数

    public int GetAttack()//攻击力等于自身的伤害加武器伤害
    {
        WeaponItem equipItem = Item.GetEquipWeapon();
        if (equipItem == null)
            return 0;
        WeaponDef itemDef = ResourceManager.GetWeaponDef(equipItem.ID);
        var att = GetAttribute();
        int itemType = (int)itemDef.WeaponType;
        int power = itemDef.Power;
        if (itemType > 0 && itemType <= 4)
            return att.PhysicalPower + power;
        if (itemType > 4 && itemType <= 8)
            return att.MagicalPower + power;
        if (itemType > 8)
            return att.PhysicalPower + att.MagicalPower + power;
        return 0;
    }

    public int GetHit()
    {
        WeaponItem equipItem = Item.GetEquipWeapon();
        var att = GetAttribute();
        return ResourceManager.GetWeaponDef(equipItem.ID).Hit + att.Skill;//武器命中+技术
    }
    public int GetCritical()
    {
        WeaponItem equipItem = Item.GetEquipWeapon();
        var att = GetAttribute();
        return ResourceManager.GetWeaponDef(equipItem.ID).Crit + (att.Skill + att.Luck / 2) / 2;
    }
    public int GetAvoid()
    { //自身速度+自身幸运+支援效果+地形效果
        var att = GetAttribute();
        return att.Speed + att.Luck;//getMapAvoid()
    }
    public int GetCriticalAvoid()
    {
        var att = GetAttribute();
        return att.Luck;
    }
    public int GetRangeMax()
    {
        return ResourceManager.GetWeaponDef(Item.GetEquipWeapon().ID).RangeType.MaxSelectRange;
    }
    public int GetRangeMin()
    {
        return ResourceManager.GetWeaponDef(Item.GetEquipWeapon().ID).RangeType.MinSelectRange;
    }
    public EnumWeaponRangeType GetRangeType()
    {
        return ResourceManager.GetWeaponDef(Item.GetEquipWeapon().ID).RangeType.RangeType;
    }
    public int GetAnger()
    {
        return 0;
    }
    public int GetAttackSpeed()
    {
        var att = GetAttribute();
        return att.Speed - ResourceManager.GetWeaponDef(Item.GetEquipWeapon().ID).Weight;
    }
    #endregion

}
