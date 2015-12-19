using UnityEngine;
using System.Collections;

[AddComponentMenu("RPGEditor/Career")]
public class CareerDef : ExtendScriptableObject
{
    [ContextMenu("Json")]
    void Json()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
    public PropertyIDNameDesc CommonProperty;

    public Sprite Icon;
    public EnumCareerLevel Level;
    /// <summary>
    /// 兵种类型
    /// </summary>
    public EnumCareerSeries Series;
    /// <summary>
    /// 可以使用的武器等级 -1：不可用 0:D ,1:C,2:B,3:A,4:S,5:SSS,6:*
    /// </summary>
    public int[] UseWeaponTypeLevel;
    /// <summary>
    /// 职业特技
    /// </summary>
    public int Skill;
    public EnumCareerModelSize ModelSize;
    public CharacterAttribute MaxAttribute;
}
