using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using System;

public class RPGCharacter : RPGCharacterBase
{
    protected Material NormalMaterial { get { return ResourceManager.GetUnitMaterial(GetCamp()); } }
    /// <summary>
    /// 是否可以操控行动，行动完毕或者被石化，冻住等则为False
    /// </summary>
    protected bool bEnableAction = true;
    /// <summary>
    /// 是否可以被玩家选择并进行行动
    /// </summary>
    public bool Controllable { get { return bEnableAction; } }
    protected UnityAction Event_OnAttackFinish;

    public RPGCharacter()
    {
    }
    public void SetDataFromDef(PlayerDef DefaultData)
    {
        logic = new CharacterLogic(DefaultData);

        logic.careerDef = ResourceManager.GetCareerDef(logic.characterDef.Career);
        if (logic.careerDef == null) Debug.LogError("不存在的career def");
    }
    /// <summary>
    /// 使角色不可以行动
    /// </summary>
    public void DisableAction(bool changeMaterial)
    {
        bEnableAction = false;
        logic.EndAction();
        if (changeMaterial) GetSpriteRender().material = ResourceManager.UnitGreyMaterial;
    }
    /// <summary>
    /// 使角色可以行动
    /// </summary>
    public void EnableAction(bool changeMaterial)
    {
        bEnableAction = true;
        logic.StartAction();
        if (changeMaterial) GetSpriteRender().material = NormalMaterial;
    }
    public Vector2Int GetTileCoord()
    {
        return logic.GetTileCoord();
    }
}
