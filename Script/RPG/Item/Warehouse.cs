using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Warehouse
{
    public int Money;
    public List<WeaponItem> Weapons;
    public List<PropsItem> Props;
    public void Sort()
    {
        Weapons.Sort();
        Props.Sort();
    }
    public void AddWeapon(WeaponItem Weapon)
    {
        Weapons.Add(Weapon);
    }
    public void RemoveWeapon(int index)
    {
        Weapons.RemoveAt(index);
    }
    public void RemoveWeapon(WeaponItem Weapon)
    {
        Weapons.Remove(Weapon);
    }
    public void AddProp(PropsItem Prop)
    {
        Props.Add(Prop);
    }
    public void RemoveProp(int index)
    {
        Props.RemoveAt(index);
    }
    public void RemoveProp(PropsItem Prop)
    {
        Props.Remove(Prop);
    }
    public void AddMoney(int MoneyAmount)
    {
        Money += MoneyAmount;
    }
}
