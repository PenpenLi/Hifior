using UnityEngine;
using System.Collections;

public class WeaponItem
{
    private int itemID;
    private int usage;
    private WeaponDef def;

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
    public bool IsValid()
    {
        return usage > 0;
    }
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
    public int SellPrice
    {
        get
        {
            return Mathf.Max(10, def.GetPrice() / 2 * (def.GetUsageTime() - Usage) / def.GetUsageTime());
        }
    }

    public bool IsBreakDown()
    {
        return this.usage == 0;
    }

    public void Use()
    {
        this.usage -= 1;
    }
}