using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 敌方，我方均继承自该类
/// </summary>
public class RPGCharacterBase : UPawn
{
    protected int Career;
    protected int Level;
    /// <summary>
    /// 属于哪一方
    /// </summary>
    protected EnumCharacterCamp Camp;
    protected CharacterDef Definition;
    protected CharacterAttribute Attribute;
    public virtual void SetDefaultData(CharacterDef DefaultData)
    {
        Definition = DefaultData;
    }
    
    public string GetCharacterName()
    {
        return Definition.CommonProperty.Name;
    }
    public int GetCharacterID()
    {
        return Definition.CommonProperty.ID;
    }
    public string GetDescription()
    {
        return Definition.CommonProperty.Description;
    }
    public Sprite GetSprite()
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

    public int GetHP()
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
