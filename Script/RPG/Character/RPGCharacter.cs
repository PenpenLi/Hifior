using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using System;

public class RPGCharacter : RPGCharacterBase
{
    [Header("Character")]
    public ItemGroup Item;

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
    protected VInt2 tileCoords = VInt2.InvalidPoint;
    protected VInt2 oldTileCoords = VInt2.InvalidPoint;
    protected UnityAction Event_OnAttackFinish;
    public RPGCharacter()
    {
        Item = new ItemGroup(base.Attribute);
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
    public VInt2 GetTileCoord()
    {
        return tileCoords;
    }
    /// <summary>
    /// 更改Tile坐标
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="ChangeWorldPosition">为True则直接修改其实际的世界坐标</param>
    public void SetTileCoord(int x, int y, bool ChangeWorldPosition)
    {
        if (tileCoords.x == x && tileCoords.y == y)
            return;
        oldTileCoords = tileCoords;
        tileCoords.x = x;
        tileCoords.y = y;
        if (ChangeWorldPosition)
        {
            transform.position = VInt2.VInt2ToVector3(x, y, true);
        }
        if (GetCamp() == EnumCharacterCamp.Player)
        {
            GetGameMode<GM_Battle>().GetSLGMap().SetTilePlayerOccupied(tileCoords.x, tileCoords.y);
        }
        else
        {
            GetGameMode<GM_Battle>().GetSLGMap().SetTileEnemyOccupied(tileCoords.x, tileCoords.y);
        }
        GetGameMode<GM_Battle>().GetSLGMap().ResetTileOccupyStatus(oldTileCoords.x, oldTileCoords.y);
    }
    public void SetOldTileCoord(bool ChangeWorldPosition)
    {
        SetTileCoord(oldTileCoords.x, oldTileCoords.y, ChangeWorldPosition);
    }
    public bool IsRunning()
    {
        return bRunning;
    }
    public virtual void StopRun()
    {
        bRunning = false;
        GetGameMode<GM_Battle>().GetSLGMap().StopMoveImmediate();
    }
    public int GetCareer()
    {
        return 0;
    }
    public string GetCareerName()
    {
        return null;
    }
    #region 人物战斗获取函数
    public int GetAttack()//攻击力等于自身的伤害加武器伤害
    {
        WeaponItem equipItem = Item.GetEquipWeapon();
        if (equipItem == null)
            return 0;
        WeaponDef itemDef = ResourceManager.GetWeaponDef(equipItem.ID);
        int itemType = (int)itemDef.WeaponType;
        int power = itemDef.Power;
        if (itemType > 0 && itemType <= 4)
            return Attribute.PhysicalPower + power;
        if (itemType > 4 && itemType <= 8)
            return this.Attribute.MagicalPower + power;
        if (itemType > 8)
            return this.Attribute.PhysicalPower + this.Attribute.MagicalPower + power;
        return 0;
    }

    public int GetHit()
    {
        WeaponItem equipItem = Item.GetEquipWeapon();
        return ResourceManager.GetWeaponDef(equipItem.ID).Hit + Attribute.Skill;//武器命中+技术
    }
    public int GetCritical()
    {
        WeaponItem equipItem = Item.GetEquipWeapon();
        return ResourceManager.GetWeaponDef(equipItem.ID).Crit + (Attribute.Skill + Attribute.Luck / 2) / 2;
    }
    public int GetAvoid()
    { //自身速度+自身幸运+支援效果+地形效果
        return Attribute.Speed + Attribute.Luck;//getMapAvoid()
    }
    public int GetCriticalAvoid()
    {
        return Attribute.Luck;
    }
    public int GetRangeMax()
    {
        return ResourceManager.GetWeaponDef(Item.GetEquipWeapon().ID).RangeType.MaxSelectRange;
    }
    public int GetRangeMin()
    {
        return ResourceManager.GetWeaponDef(Item.GetEquipWeapon().ID).RangeType.MinSelectRange;
    }
    public EnumWeaponRangeType GetRangeType()
    {
        return ResourceManager.GetWeaponDef(Item.GetEquipWeapon().ID).RangeType.RangeType;
    }
    public int GetAnger()
    {
        return 0;
    }
    public int GetAttackSpeed()
    {
        return Attribute.Speed - ResourceManager.GetWeaponDef(Item.GetEquipWeapon().ID).Weight;
    }
    #endregion
    #region 地图相关

    public void ShowMovement()
    {
        GetGameMode<GM_Battle>().GetSLGMap().InitActionScope(this);
    }
    public void MoveTo(VInt2 Point, UnityAction Start = null, UnityAction End = null)
    {
        if (Point == GetTileCoord())
        {
            End();
            return;
        }
        SLGMap slgmap = GetGameMode<GM_Battle>().GetSLGMap();
        if (slgmap.CanMoveTo(Point))
        {
            bRunning = true;
            slgmap.Move(this, Start, () => { bRunning = false; End(); });
            SetTileCoord(Point.x, Point.y, false);
        }
        else
        {
            Debug.LogError("无法移动到目标点，SLGMap中CanMoveTo 返回False");
        }
    }
    /// <summary>
    /// 获取攻击范围的数组
    /// </summary>
    /// <param name="ShowRange">在战场上显示出范围</param>
    /// <returns></returns>
    public List<VInt2> FindAttack(bool ShowRange)
    {
        SLGMap m_slgmap = GetGameMode<GM_Battle>().GetSLGMap();
        List<VInt2> p = ShowRange ? m_slgmap.FindAttackRange(tileCoords.x, tileCoords.y, Item.GetEquipWeapon().GetDefinition()) : m_slgmap.FindAttackRangeWithoutShow(this);
        return p;
    }
    /// <summary>
    /// 是否已经完成攻击的过程了
    /// </summary>
    /// <returns></returns>
    public bool IsFinishAttack()
    {
        return !bAttacking;
    }
    /// <summary>
    /// 开始攻击一个敌人
    /// </summary>
    /// <param name="arrowOnCharacter">攻击的目标</param>
    /// <param name="OnAttackFinish">攻击结束后执行的事件</param>
    public void BeginAttack(RPGCharacter arrowOnCharacter, UnityAction OnAttackFinish)
    {
        Debug.Log(GetCharacterName() + " 开始攻击 " + arrowOnCharacter.GetCharacterName());

        Event_OnAttackFinish = OnAttackFinish;
        bAttacking = true;
        StartCoroutine(UpdateAttack());
    }
    IEnumerator UpdateAttack()
    {
        yield return new WaitForSeconds(1.5f);
        yield return null;
        EndAttack();
    }
    /// <summary>
    /// 跳过该攻击显示的过程，但数值之类的产生效果，且攻击后的经验值，死亡等情况还是要进行结算
    /// </summary>
    public void EndAttack()
    {
        Debug.Log("攻击结束");
        Event_OnAttackFinish.Invoke();

        bAttacking = false;
    }
    #endregion
}
