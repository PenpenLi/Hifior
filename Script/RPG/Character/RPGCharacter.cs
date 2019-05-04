using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using System;
using RPG.AI;
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
    public BaseAttackAI AI_Attack;
    public BaseSkillAI AI_Skill;

    public RPGCharacter()
    {
        AI_Attack = new AI_AttackIfInRange(this);
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
        if (changeMaterial) GreySprite();
    }
    /// <summary>
    /// 正常时候的显示，判定状态进行材质选择
    /// </summary>
    public void NormalSprite()
    {
        GetSpriteRender().material = NormalMaterial;
    }
    public void GreySprite()
    {
        GetSpriteRender().material = ResourceManager.UnitGreyMaterial;
    }
    /// <summary>
    /// 使角色可以行动
    /// </summary>
    public void EnableAction(bool changeMaterial)
    {
        bEnableAction = true;
        logic.StartAction();
        if (changeMaterial) NormalSprite();
    }
    public Vector2Int GetTileCoord()
    {
        return logic.GetTileCoord();
    }
}
