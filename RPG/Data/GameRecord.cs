using UnityEngine;
using System.Collections.Generic;
using System;
#region 序列化类类型
[System.Serializable]
public class PlayerInfo : SerializableBase
{
    public int ID;
    public int Level;
    public int Exp;
    public CharacterAttribute Attribute;
    public Item[] Items;
    public override string GetKey()
    {
        return ID.ToString();
    }
}
[System.Serializable]
public class PlayerInfoCollection : SerializableList<PlayerInfo>
{
    public override string GetFullRecordPathName()
    {
        return Application.persistentDataPath + "/PlayerInfoCollection.sav";
    }
    public override string GetKey()
    {
        return "PlayerInfoCollection";
    }
    [RuntimeInitializeOnLoadMethod]
    public static void III()
    {
        PlayerInfoCollection sss = new PlayerInfoCollection();

        //PlayerInfo s0 = new PlayerInfo();
        //s0.ID = 5;
        //s0.Level = 1;
        //s0.Exp = 100;
        //s0.Attribute = new CharacterAttribute();
        //s0.Attribute.HP = 1000;
        //s0.Items = new Item[] { new Item(1, 52), new Item(6, 12) };
        //PlayerInfo s1 = new PlayerInfo();
        //s1.ID = 45;
        //s1.Level = 12;
        //s1.Exp = 25;
        //s1.Attribute = new CharacterAttribute();
        //s1.Attribute.HP = 50;
        //s1.Items = new Item[] { new Item(1, 52), new Item(6, 12) };
        //sss.AddContent(s0);
        //sss.AddContent(s1);

        //sss.SaveBinary();
        Debug.Log(sss.GetBinaryContent("0").Attribute);
    }
}
#endregion
public static class GameRecord
{
    public const string SAVEFILENAME = "";
    /// <summary>
    /// 获取指定位置存档的路径
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string GetSavePath(int index)
    {
        return SAVEFILENAME + index.ToString("D2");
    }
    public static void SaveTo(int index)
    {

    }
    public static void LoadFrom(int index)
    {

    }
}
