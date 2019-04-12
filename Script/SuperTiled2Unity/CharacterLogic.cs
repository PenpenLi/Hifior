using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic
{
    private CharacterInfo info;

    protected Animator anim;
    /// <summary>
    /// 是否可以操控行动，行动完毕或者被石化，冻住等则为False
    /// </summary>
    protected bool bEnableAction = true;
    /// <summary>
    /// 是否可以被玩家选择并进行行动
    /// </summary>
    public bool Controllable
    {
        get
        {
            return bEnableAction;
        }
    }
    /// <summary>
    /// 是否在移动中
    /// </summary>
    protected bool bRunning = false;
    /// <summary>
    /// 是否在攻击过程中
    /// </summary>
    protected bool bAttacking = false;
    protected int damageCount = 0;//收到伤害和造成伤害的次数
    [SerializeField]
    protected Vector2Int tileCoords;
    protected Vector2Int oldTileCoords;

    public int GetMovement()
    {
        return 6;
    }
    public Vector2Int GetTileCoords()
    {
        return tileCoords;
    }
    public Vector2Int GetOldTileCoords()
    {
        return oldTileCoords;
    }
    public void SetTileCoords(Vector2Int tilePos)
    {
        oldTileCoords = tileCoords;
        tileCoords = tilePos;
    }
}
