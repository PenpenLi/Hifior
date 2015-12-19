using UnityEngine;
using System.Collections.Generic;

public class MapTileDef : ExtendScriptableObject
{

    [ContextMenu("Json")]
    void Json()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
    public List<TileAttribute> TileProperty;
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
    public TileAttribute()
    {
        MovementConsume = new int[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    }
}
