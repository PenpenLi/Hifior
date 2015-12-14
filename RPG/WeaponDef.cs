using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[AddComponentMenu("RPGEditor/Item/Weapon")]
public class WeaponDef : ScriptableObject
{
    public PropertyIDNameDesc CommonProperty;

    public EnumWeaponType WeaponType;
    public Sprite Icon;
    public int SinglePrice;
    public int UseNumber;
    public SelectRangeType RangeType;
    public int Weight;
    public int Power;
    public int Hit;
    public int Crit;
    public int DedicatedCharacter;
    public int CareerEffect;
    public int SuperEffect;
    public EnumWeaponAttackEffectType AttackEffect;
    public bool ImportantWeapon;
    public bool NoExchange;

    //添加的额外属性
    public CharacterAttribute AdditionalAttribute;
    //成长率提高
    public CharacterAttributeGrow AdditionalAttributeGrow;
}
