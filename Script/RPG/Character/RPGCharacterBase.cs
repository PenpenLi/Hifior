using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 敌方，我方均继承自该类
/// </summary>
public class RPGCharacterBase
{
    protected Transform transform;
    protected CharacterLogic logic;
    public CharacterLogic Logic { get { return logic; } }

    /// <summary>
    /// 属于哪一方
    /// </summary>
    protected EnumCharacterCamp camp;
    public void SetTransform(Transform t)
    {
        transform = t;
    }
    public Transform GetTransform() { return transform; }
    public GameObject GetGameObject() { return transform.gameObject; }

    public virtual void SetDefaultData(PlayerDef DefaultData)
    {
        logic = new CharacterLogic(DefaultData);
        logic.careerDef = ResourceManager.GetCareerDef(logic.characterDef.Career);
        if (logic.careerDef == null) Debug.LogError("不存在的career def");
    }
    /// <summary>
    /// 是否是领导者，在Player里和Enemy类里重写
    /// </summary>
    /// <returns></returns>
    public virtual bool IsLeader()
    {
        return false;
    }
    public EnumCharacterCamp GetCamp()
    {
        return camp;
    }
    public void SetCamp(EnumCharacterCamp Camp)
    {
        this.camp = Camp;
    }

}
