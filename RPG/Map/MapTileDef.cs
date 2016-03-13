using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
public class MapTileDef : ExtendScriptableObject
{
    public List<TileAttribute> TileProperty;
    private void AttributeExists(int ID)
    {
        Assert.IsTrue(TileProperty.Count > ID, "不存在的图块数据 ID=" + ID);
    }
    public string GetName(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].CommonProperty.Name;
    }
    public string GetDescription(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].CommonProperty.Description;
    }
    public int GetID(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].CommonProperty.ID;
    }
    public int GetAvoid(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].Avoid;
    }
    public int GetPhysicalDefense(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].PhysicalDefense;
    }
    public int GetMagicalDefense(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].MagicalDefense;
    }
    public int GetRecover(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].Recover;
    }
    public int GetBattleBackgroundID(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].BattleBackgroundID;
    }
    public int GetMovementConsume(int ID, int CareerType)
    {
        AttributeExists(ID);
        Assert.IsTrue(CareerType < TileProperty[ID].MovementConsume.Length, "CareerType越界");
        return TileProperty[ID].MovementConsume[CareerType] > 0 ? TileProperty[ID].MovementConsume[CareerType] : 1;
    }
    public bool GetPassUp(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].PassUp;
    }
    public bool GetPassLeft(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].PassLeft;
    }
    public bool GetPassRight(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].PassRight;
    }
    public bool GetPassDown(int ID)
    {
        AttributeExists(ID);
        return TileProperty[ID].PassDown;
    }
}
[System.Serializable]
public class TileAttribute
{
    public PropertyIDNameDesc CommonProperty;
    public int Avoid;
    public int PhysicalDefense;
    public int MagicalDefense;
    public int Recover;
    public int BattleBackgroundID;
    public int[] MovementConsume;
    public bool PassUp = true;
    public bool PassLeft = true;
    public bool PassRight = true;
    public bool PassDown = true;
    public TileAttribute()
    {
        MovementConsume = new int[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    }
}
