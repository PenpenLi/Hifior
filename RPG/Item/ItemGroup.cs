using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Item
{
    public int ID;
    public int Usage;
    public Item(int id,int usage)
    {
        ID = id;
        Usage = usage;
    }
}
public class ItemGroup
{
    private List<WeaponItem> items = new List<WeaponItem>();//装备 0-5为可用的，7-8为装备的持有的
    private List<WeaponItem> passiveItems = new List<WeaponItem>();//被动装备  
    private CharacterAttribute attribute;
    private int lastWeaponIndex;//上一个装备的Index
    private int _currentEquipItemIndex = -1; //当前装备武器的index
    public ItemGroup(CharacterAttribute attr)
    {
        attribute = attr;
    }
    public List<WeaponItem> Items
    {
        get
        {
            return items;
        }
    }

    public List<WeaponItem> PassiveItems
    {
        get
        {
            return passiveItems;
        }
    }

    #region 装备处理函数
    public void SortItems()
    {
        List<WeaponItem> itemNew = new List<WeaponItem>();
        if (_currentEquipItemIndex >= 0)//有装备的武器
        {
            itemNew.Add(Items[_currentEquipItemIndex]);
            Items.RemoveAt(_currentEquipItemIndex);//删除原items中的装备的武器
            _currentEquipItemIndex = 0;
        }
        else//没有装备武器
        {
            _currentEquipItemIndex = -1;
            for (int i = 0; i < Items.Count; i++)
            {
                if (IsItemEnabled(Items[i].ID))
                {
                    itemNew.Add(Items[i]);
                    Items.RemoveAt(i);//删除原items中的装备的武器
                    _currentEquipItemIndex = 0;
                    break;
                }
            }
        }
        for (int i = 0; i < Items.Count; i++)//然后是没有装备的武器
        {
            if (IsItemEnabled(Items[i].ID))//武器类型放上面
            {
                itemNew.Add(Items[i]);
                Items.RemoveAt(i);
            }
        }
        foreach (WeaponItem i in Items)//最后是不可以装备的消耗品
        {
            itemNew.Add(i);
        }
        items = itemNew;
        if (itemNew.Count > 0)
            EquipItem(0);
    }
    public bool AddItem(int ID)//获得装备
    {
       return AddItem(new WeaponItem(ID));
    }
    public bool AddItem(WeaponItem Item)//获得装备
    {
        if (Items.Count == ConstTable.CONST_ITEM_COUNT)//装备已满返回false
        {
            Debug.Log("物品已达上限");
            return false;
        }
        if (_currentEquipItemIndex < 0)//没有装备武器，判断武器是否可以装备，如果可以装备到第一格，否则直接添加到末尾
        {
            if (IsItemEnabled(Item.ID))//可用的武器
            {

                Items.Insert(0, Item);
                EquipItem(0);
                return true;
            }
            else
            {
                Items.Add(Item);
                return true;
            }
        }
        else
        {
            Items.Add(Item);
            return true;
        }
    }

    public bool AddItem(WeaponItem Item, int InsertIndex)//添加装备到指定顺序
    {
        if (Items.Count == ConstTable.CONST_ITEM_COUNT)//若装备已满，返回FALSE
        {
            Debug.Log("物品已达上限");
            return false;
        }
        if (InsertIndex >= Items.Count)
            Items.Add(Item);
        else
            Items.Insert(InsertIndex, Item);
        return true;
    }
    public void EquipItem(int index)
    {
        if (Items.Count <= index)
        {
            this._currentEquipItemIndex = -1;
            return;
        }
        this._currentEquipItemIndex = index;
        //ExtraAblityManage();
    }

    public void EquipItemWithSort(int index)
    {
        EquipItem(index);
        SortItems();
    }
    public void DischargeItem()
    {
        this._currentEquipItemIndex = -1;
    }
    public List<WeaponItem> GetAllItems()
    {
        return this.Items;
    }

    public List<WeaponItem> GetAttackWeapon()//获得可以攻击的武器
    {
        List<WeaponItem> attackWeapon = new List<WeaponItem>();
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] != null)
            {
                if (Items[i].GetDefinition().IsWeaponType() && IsItemEnabled(Items[i].ID))
                {
                    attackWeapon.Add(Items[i]);
                }

            }
        }
        return attackWeapon;
    }
    public bool IsItemEnabled(int itemID)//medifyneed
    {
        /*WeaponDef def = ResourceManager.GetWeaponDef(itemID);
        EnumWeaponType t = def.GetWeaponType();//武器类型1剑
        int[] weaponCanUseType = attribute.WeaponType.ToArray();//当前人物可以使用的武器类型数组0是剑1是枪以此类推
                                                                //判断是否是专用武器
        if (Table._ItemTable.getRequireValue(itemID) == 6)//是“*”级别武器，特殊人物和职业可用
        {
            int requireType = Table._ItemTable.getRequireType(itemID);
            if (requireType == 1) //requireType为1时人物0专用
            {
                if (attribute.ID == ConstTable.CONST_LEADER_1)
                    return true;
            }
            if (requireType == 2) //requireType为2时人物1专用
            {
                if (attribute.ID == ConstTable.CONST_LEADER_2)
                    return true;
            }
            return false;
        }
        else
        {
            if (weaponCanUseType[t - 1] >= Table._ItemTable.getRequireValue(itemID))//如果人物武器熟练度大于武器需求
                return true;
            else
                return false;
        }*/
        return true;
    }

    public int GetCurrentItemMaxRange()
    {
        int maxRange = 0;
        for (int i = 0; i < Items.Count; i++)
        {
            int tempMaxRange = Items[i].GetDefinition().RangeType.MaxSelectRange;
            if (tempMaxRange > maxRange)
                maxRange = tempMaxRange;
        }
        return maxRange;
    }

    public WeaponItem GetEquipItem()
    {
        if (this._currentEquipItemIndex == -1)
            return null;
        return this.Items[this._currentEquipItemIndex];
    }
    public WeaponItem GetItem(int index)
    {
        if ((index >= Items.Count) || (index < 0)) return null;
        return this.Items[index];
    }
    public int GetItemCount()
    {
        return this.Items.Count;
    }
    public int getLastWeaponIndex()
    {
        return this.lastWeaponIndex;
    }
    public bool isHaveItem(int itemID)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (this.Items[i].ID == itemID)
            {
                return true;
            }
        }
        return false;
    }
    public void removeAllItem()
    {
        Items.Clear();
    }

    public void removeItem(int paramInt)
    {
        if ((paramInt >= this.Items.Count) || (paramInt < 0) || (this.Items.Count == 0))
        {
            return;
        }
        this.Items.RemoveAt(paramInt);
    }

    public void removeLastItem()
    {
        if (this.Items.Count == 0)
        {
            return;
        }
        removeItem(this.Items.Count - 1);
    }

    public void SetLastWeaponIndex(int Index)//上一个武器id
    {
        this.lastWeaponIndex = Index;
    }
    #endregion
}