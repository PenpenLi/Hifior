using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 游戏内的参数
/// </summary>
[System.Serializable]
public class GameItem:IComparable<GameItem>
{
    [SerializeField]
    protected int itemID;
    [SerializeField]
    protected int usage;

    public int ID
    {
        get
        {
            return itemID;
        }
    }
    public int Usage
    {
        get
        {
            return usage;
        }
    }
    /// <summary>
    /// 武器是否可用
    /// </summary>
    /// <returns></returns>
    public virtual bool IsValid()
    {
        return usage > 0;
    }

    public virtual bool IsBreakDown()
    {
        return this.usage == 0;
    }

    public void Use()
    {
        this.usage -= 1;
    }

    public int CompareTo(GameItem other)
    {
        if (other.ID > this.ID)
            return 1;
        if (other.ID == this.ID)
            return other.usage > this.usage ? 1 : -1;
        else
            return -1;
    }
    public override string ToString()
    {
        return "item id=" + itemID + " usage=" + usage;
    }
}
[System.Serializable]
public class WeaponItem:GameItem
{
    [System.NonSerialized]
    private WeaponDef def;
    public int GetMaxUsage()
    {
        return def.UseNumber;
    }
    public WeaponItem(int ItemID)
    {
        def = ResourceManager.GetWeaponDef(ItemID);
        this.itemID = def.CommonProperty.ID;
        this.usage = def.UseNumber;
    }
    public WeaponItem(int ItemID, int Usage) : this(ItemID)
    {
        this.usage = Usage;
    }
    public WeaponDef GetDefinition()
    {
        return def;
    }
    public string GetName()
    {
        return def.CommonProperty.Name;
    }
    public string GetDesc()
    {
        return def.CommonProperty.Description;
    }
    public int SellPrice
    {
        get
        {
            return Mathf.Max(10, def.GetPrice() / 2 * (def.GetUsageTime() - Usage) / def.GetUsageTime());
        }
    }
}
[System.Serializable]
public class PropsItem : GameItem
{
    [System.NonSerialized]
    private PropsDef def;
    public int GetMaxUsage()
    {
        return def.UseNumber;
    }
    public PropsItem(int ItemID)
    {
        def = ResourceManager.GetPropsDef(ItemID);
        this.itemID = def.CommonProperty.ID;
        this.usage = def.UseNumber;
    }
    public PropsItem(int ItemID, int Usage) : this(ItemID)
    {
        this.usage = Usage;
    }
    public PropsDef GetDefinition()
    {
        return def;
    }
    /// <summary>
    /// 返回单个售价，如果该装备不可使用，没有使用次数，则返回单价
    /// </summary>
    public int SellPrice
    {
        get
        {
            if (def.GetUsageTime() > 0)
                return Mathf.Max(10, def.GetPrice() / 2 * (def.GetUsageTime() - Usage) / def.GetUsageTime());
            else
                return def.GetSinglePrice();
        }
    }
}