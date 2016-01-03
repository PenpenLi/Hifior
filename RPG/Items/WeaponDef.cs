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
}
