using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
public class ItemGroup
{
    public const int MAX_WEAPON_COUNT = 6;
    private List<WeaponItem> weapons = new List<WeaponItem>();
    private List<PropsItem> props = new List<PropsItem>();
    private List<PropsItem> passiveItems = new List<PropsItem>();//被动装备  
    private CharacterAttribute attribute;
    private int lastWeaponIndex;//上一个装备的Index
    private int _currentEquipItemIndex = -1; //当前装备武器的index
    /// <summary>
    /// 在完全获得该道具后执行的事件
    /// </summary>
    private UnityAction AfterSuccessAddItem;
    public ItemGroup(CharacterAttribute attr)
    {
        attribute = attr;
    }
    public List<WeaponItem> Weapons
    {
        get
        {
            return weapons;
        }
    }

    public List<PropsItem> PassiveItems
    {
        get
        {
            return passiveItems;
        }
    }

    #region 装备处理函数
    public void SortWeapons()
    {
        List<WeaponItem> itemNew = new List<WeaponItem>();
        if (_currentEquipItemIndex >= 0)//有装备的武器
        {
            itemNew.Add(Weapons[_currentEquipItemIndex]);
            Weapons.RemoveAt(_currentEquipItemIndex);//删除原items中的装备的武器
            _currentEquipItemIndex = 0;
        }
        else//没有装备武器
        {
            _currentEquipItemIndex = -1;
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (IsWeaponEnabled(Weapons[i].ID))
                {
                    itemNew.Add(Weapons[i]);
                    Weapons.RemoveAt(i);//删除原items中的装备的武器
                    _currentEquipItemIndex = 0;
                    break;
                }
            }
        }
        for (int i = 0; i < Weapons.Count; i++)//然后是没有装备的武器
        {
            if (IsWeaponEnabled(Weapons[i].ID))//武器类型放上面
            {
                itemNew.Add(Weapons[i]);
                Weapons.RemoveAt(i);
            }
        }
        foreach (WeaponItem i in Weapons)//最后是不可以装备的消耗品
        {
            itemNew.Add(i);
        }
        weapons = itemNew;
        if (itemNew.Count > 0)
            EquipWeapon(0);
    }
    /// <summary>
    /// 只添加道具，不做显示处理
    /// </summary>
    /// <param name="Items"></param>
    public void AddWeapons(List<int> Items)
    {
        foreach (int i in Items)
        {
            if (weapons.Count > MAX_WEAPON_COUNT)
            {
                Debug.LogError("武器超过最大可容纳的数量了");
            }
            else {
                AddWeapon(i, null);
            }
        }
    }
    public bool AddWeapon(int ID, UnityAction AfterAddItem)//获得装备
    {
        return AddWeapon(new WeaponItem(ID), AfterAddItem);
    }
    public bool AddWeapon(WeaponItem Item, UnityAction AfterAddItem)//获得装备
    {
        AfterSuccessAddItem = AfterAddItem;

        if (Weapons.Count == MAX_WEAPON_COUNT)//装备已满返回false
        {
            Debug.Log("物品已达上限");
            weapons.Add(Item);
            RPG.UI.SendWeaponToWarehouse Sender = UIController.Instance.GetUI<RPG.UI.SendWeaponToWarehouse>();
            if(AfterSuccessAddItem!=null)
            Sender.RegisterHideEvent(AfterSuccessAddItem);
            Sender.Show(weapons);
            return false;
        }
        else
        {
            if (_currentEquipItemIndex < 0)//没有装备武器，判断武器是否可以装备，如果可以装备到第一格，否则直接添加到末尾
            {
                if (IsWeaponEnabled(Item.ID))//可用的武器
                {

                    Weapons.Insert(0, Item);
                    EquipWeapon(0);
                }
                else
                {
                    Weapons.Add(Item);
                }
            }
            else
            {
                Weapons.Add(Item);
            }
            if (AfterSuccessAddItem != null)
                AfterSuccessAddItem.Invoke();
            return true;
        }
    }

    public bool AddWeapon(WeaponItem Item, int InsertIndex)//添加装备到指定顺序
    {
        if (Weapons.Count == ConstTable.CONST_M_COUNT)//若装备已满，返回FALSE
        {
            Debug.Log("物品已达上限");
            return false;
        }
        if (InsertIndex >= Weapons.Count)
            Weapons.Add(Item);
        else
            Weapons.Insert(InsertIndex, Item);
        return true;
    }
    public void EquipWeapon(int index)
    {
        if (Weapons.Count <= index)
        {
            this._currentEquipItemIndex = -1;
            return;
        }
        this._currentEquipItemIndex = index;
        //ExtraAblityManage();
    }

    public void EquipWeaponWithSort(int index)
    {
        EquipWeapon(index);
        SortWeapons();
    }

    public void DischargeWeapon()
    {
        this._currentEquipItemIndex = -1;
    }
    public List<WeaponItem> GetAllWeapons()
    {
        return this.Weapons;
    }

    public List<WeaponItem> GetAttackWeapon()//获得可以攻击的武器
    {
        List<WeaponItem> attackWeapon = new List<WeaponItem>();
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i] != null)
            {
                if (Weapons[i].GetDefinition().IsWeaponType() && IsWeaponEnabled(Weapons[i].ID))
                {
                    attackWeapon.Add(Weapons[i]);
                }

            }
        }
        return attackWeapon;
    }
    public bool IsWeaponEnabled(int itemID)//medifyneed
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

    public int GetCurrentWeaponMaxRange()
    {
        int maxRange = 0;
        for (int i = 0; i < Weapons.Count; i++)
        {
            int tempMaxRange = Weapons[i].GetDefinition().RangeType.MaxSelectRange;
            if (tempMaxRange > maxRange)
                maxRange = tempMaxRange;
        }
        return maxRange;
    }

    public WeaponItem GetEquipWeapon()
    {
        if (this._currentEquipItemIndex == -1)
            return null;
        return this.Weapons[this._currentEquipItemIndex];
    }
    public WeaponItem GetWeaponByIndex(int index)
    {
        if ((index >= Weapons.Count) || (index < 0)) return null;
        return this.Weapons[index];
    }
    public int GetWeaponCount()
    {
        return this.Weapons.Count;
    }
    public int GetLastWeaponIndex()
    {
        return this.lastWeaponIndex;
    }
    public bool HaveWeapon(int WeaponID)
    {
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (this.Weapons[i].ID == WeaponID)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveAllWeapons()
    {
        Weapons.Clear();
    }

    public void RemoveWeaponByIndex(int Index)
    {
        if ((Index >= this.Weapons.Count) || (Index < 0) || (this.Weapons.Count == 0))
        {
            return;
        }
        this.Weapons.RemoveAt(Index);
    }

    public void RemoveLastWeapon()
    {
        if (this.Weapons.Count == 0)
        {
            return;
        }
        RemoveWeaponByIndex(this.Weapons.Count - 1);
    }
    #endregion
}