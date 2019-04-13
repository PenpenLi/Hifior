using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using System;

public class RPGCharacter : RPGCharacterBase
{
    protected Animator anim;
    /// <summary>
    /// 是否可以操控行动，行动完毕或者被石化，冻住等则为False
    /// </summary>
    protected bool bEnableAction = true;
    /// <summary>
    /// 是否可以被玩家选择并进行行动
    /// </summary>
    public bool Controllable { get { return bEnableAction; } }
    /// <summary>
    /// 是否在移动中
    /// </summary>
    protected bool bRunning = false;
    /// <summary>
    /// 是否在攻击过程中
    /// </summary>
    protected bool bAttacking = false;
    protected int damageCount = 0;//收到伤害和造成伤害的次数

    protected UnityAction Event_OnAttackFinish;

    public RPGCharacter()
    {
    }
    /// <summary>
    /// 使角色不可以行动
    /// </summary>
    public void DisableControl()
    {
        bEnableAction = false;
    }
    /// <summary>
    /// 使角色可以行动
    /// </summary>
    public void EnableControl()
    {
        bEnableAction = true;
    }
    public Vector2Int GetTileCoord()
    {
        return logic.GetTileCoord();
    }

    public bool IsRunning()
    {
        return bRunning;
    }
    public virtual void StopRun()
    {
        bRunning = false;
        //GetGameMode<GM_Battle>().GetSLGMap().StopMoveImmediate();
    }
}
