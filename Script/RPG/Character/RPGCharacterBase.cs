using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 敌方，我方均继承自该类
/// </summary>
public class RPGCharacterBase
{
    protected Transform transform;
    protected CharacterLogic logic;
    public CharacterLogic Logic() { return logic; }

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

    public virtual void SetDefaultData(CharacterDef DefaultData)
    {
        logic =new CharacterLogic(DefaultData);
        logic.careerDef = ResourceManager.GetCareerDef(logic.characterDef.Career);
        if (logic.careerDef == null) Debug.LogError("不存在的career def");
    }
    public CharacterAttribute GetAttribute()
    {
        return logic.GetAttribute();
    }
    public string GetCharacterName()
    {
        return logic.characterDef.CommonProperty.Name;
    }
    public int GetCharacterID()
    {
        return logic.characterDef.CommonProperty.ID;
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
        return logic.characterDef.CommonProperty.Description;
    }
    public Sprite GetPortrait()
    {
        return logic.characterDef.Portrait;
    }
    public GameObject GetStaticMesh()
    {
        return logic.characterDef.BattleModel;
    }
    public Sprite[] GetStaySprites()
    {
        return logic.careerDef.Stay;
    }
    public Sprite[] GetMoveSprites()
    {
        return logic.careerDef.Move;
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
