using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 敌方，我方均继承自该类
/// </summary>
public class RPGCharacterBase : UPawn
{
    [Header("CharacterBase")]
    protected int Career;
    protected int Level;
    protected int Exp;
    protected int CurrentHP;

    public int GetCurrentHP()
    {
        return CurrentHP;
    }
    public int GetExp()
    {
        return Exp;
    }
    /// <summary>
    /// 属于哪一方
    /// </summary>
    protected EnumCharacterCamp Camp;
    protected CharacterDef Definition;
    protected CharacterAttribute Attribute;
    public virtual void SetDefaultData(CharacterDef DefaultData)
    {
        Definition = DefaultData;
        Career = DefaultData.Career;
        Level = DefaultData.DefaultLevel;
        Exp = 0;
    }
    public void SetAttribute(CharacterAttribute InAttribute)
    {
        this.Attribute = (CharacterAttribute)InAttribute.Clone();
        CurrentHP = InAttribute.HP;
    }
    public string GetCharacterName()
    {
        return Definition.CommonProperty.Name;
    }
    public int GetCharacterID()
    {
        return Definition.CommonProperty.ID;
    }
    /// <summary>
    /// 是否是领导者，在Player里和Enemy类里重写
    /// </summary>
    /// <returns></returns>
    public virtual bool IsLeader()
    {
        return false;
    }
    public string GetDescription()
    {
        return Definition.CommonProperty.Description;
    }
    public Sprite GetPortrait()
    {
        return Definition.Portrait;
    }
    public GameObject GetStaticMesh()
    {
        return Definition.BattleModel;
    }
    public EnumCharacterCamp GetCamp()
    {
        return Camp;
    }
    public void SetCamp(EnumCharacterCamp Camp)
    {
        this.Camp = Camp;
    }
    public EnumCharacterImportance Importance
    {
        get
        {
            return Definition.CharacterImportance;
        }
    }
    public int GetDefaultCareer()
    {
        return Definition.Career;
    }
    public CharacterAttribute GetDefaultAttribute()
    {
        return Definition.DefaultAttribute;
    }
    public virtual int GetLevel()
    {
        return Level;
    }

    public int GetMaxHP()
    {
        return Attribute.HP;
    }
    public int GetPhysicalPower()
    {
        return Attribute.PhysicalPower;
    }
    public int GetMagicalPower()
    {
        return Attribute.MagicalPower;
    }
    public int GetSkill()
    {
        return Attribute.Skill;
    }
    public int GetSpeed()
    {
        return Attribute.Speed;
    }
    public int GetLuck()
    {
        return Attribute.Luck;
    }
    public int GetPhysicalDefense()
    {
        return Attribute.PhysicalDefense;
    }
    public int GetMagicalDefense()
    {
        return Attribute.MagicalDefense;
    }
    public int GetMovement()
    {
        return Attribute.Movement;
    }
}
