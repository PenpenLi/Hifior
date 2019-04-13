using UnityEngine;

public class CareerDef : ExtendScriptableObject
{
    [ContextMenu("Json")]
    void Json()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
    public PropertyIDNameDesc CommonProperty;
    /// <summary>
    /// 职业图标
    /// </summary>
    public Sprite Icon;

    [SerializeField]
    public Sprite[] Stay;
    [SerializeField]
    public Sprite[] Move;
    
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
    /// <summary>
    /// 职业模型大小
    /// </summary>
    public EnumCareerModelSize ModelSize;
    /// <summary>
    /// 最大属性限制
    /// </summary>
    public CharacterAttribute MaxAttribute;
}
