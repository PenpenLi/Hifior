using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class RPGCharacter : RPGCharacterBase
{
    [Header("Character")]
    public ItemGroup Item;

    protected Animator anim;
    [SerializeField]
    protected bool bRunning = false;//是否在移动中

    protected int damageCount = 0;//收到伤害和造成伤害的次数
    [SerializeField]
    protected Point2D tileCoords;
    protected Point2D oldTileCoords;
    public Point2D OldTileCoords
    {
        get
        {
            return oldTileCoords;
        }
    }
    public RPGCharacter()
    {
        Item = new ItemGroup(base.Attribute);
    }

    public int GetCurrentHP()
    {
        return 0;
    }

    public Point2D GetTileCoord()
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
        oldTileCoords = tileCoords;
        tileCoords.x = x;
        tileCoords.y = y;
        if (ChangeWorldPosition)
        {
            transform.position = Point2D.Point2DToVector3(x, y, true);
        }
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
        WeaponItem equipItem = Item.GetEquipItem();
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
        WeaponItem equipItem = Item.GetEquipItem();
        return ResourceManager.GetWeaponDef(equipItem.ID).Hit + Attribute.Skill;//武器命中+技术
    }
    public int GetCritical()
    {
        WeaponItem equipItem = Item.GetEquipItem();
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
        return ResourceManager.GetWeaponDef(Item.GetEquipItem().ID).RangeType.MaxSelectRange;
    }
    public int GetRangeMin()
    {
        return ResourceManager.GetWeaponDef(Item.GetEquipItem().ID).RangeType.MinSelectRange;
    }
    public EnumWeaponRangeType GetRangeType()
    {
        return ResourceManager.GetWeaponDef(Item.GetEquipItem().ID).RangeType.RangeType;
    }
    public int GetAnger()
    {
        return 0;
    }
    public int GetAttackSpeed()
    {
        return Attribute.Speed - ResourceManager.GetWeaponDef(Item.GetEquipItem().ID).Weight;
    }
    #endregion
    #region 地图相关

    public void ShowMovement()
    {
        GetGameMode<GM_Battle>().GetSLGMap().InitActionScope(this, GetMovement());
    }
    public void MoveTo(Point2D Point, UnityAction Start = null, UnityAction End = null)
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
    #endregion
}
