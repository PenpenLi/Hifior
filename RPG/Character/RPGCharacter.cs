using UnityEngine;
using System.Collections;
using System;

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
    public void SetTileCoord(int x,int y)
    {
        tileCoords.x = x;
        tileCoords.y = y;
    }
    public bool IsRunning()
    {
        return bRunning;
    }
    public virtual void Run()
    {
        bRunning = true;
    }
    public virtual void StopRun()
    {
        bRunning = false;
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
        int itemType =(int) itemDef.WeaponType;
        int power = itemDef.Power;
        if (itemType > 0 && itemType <= 4)
            return Attribute.PhysicalPower +power;
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
    # endregion
}
