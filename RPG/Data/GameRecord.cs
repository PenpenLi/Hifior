using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;
#region 序列化类类型
[System.Serializable]
public class CharacterInfo : SerializableBase
{
    public int ID;
    public int Level;
    public int Exp;
    public CharacterAttribute Attribute;
    public List<WeaponItem> Items;
    public override string GetKey()
    {
        return ID.ToString();
    }
    public CharacterInfo(RPGCharacter Character)
    {
        ID = Character.GetCharacterID();
        Level = Character.GetLevel();
        Exp = Character.GetLevel();
        Attribute = Character.GetAttribute();
        Items = Character.Item.Items;
    }
    public override string ToString()
    {
        return
   "ID=" + ID + ";  \n" +
  "Level= " + Level + ";  \n" +
  "Exp= " + Exp + ";  \n" +
  "CharacterAttribute=" + Attribute + ";  \n" +
  "Items= " + Items;
    }
}
[System.Serializable]
public class ChapterRecordCollection : SerializableBase
{
    /// <summary>
    /// 存档顺序
    /// </summary>
    public int Index;
    /// <summary>
    /// 当前队伍的钱
    /// </summary>
    public int Money;
    /// <summary>
    /// 章节
    /// </summary>
    public int Chapter;
    /// <summary>
    /// 存档玩家信息
    /// </summary>
    public PlayerInfoCollection PlayersInfo;
    /// <summary>
    /// 可用的玩家ID
    /// </summary>
    public List<int> AvailablePlayers;

    /// <summary>
    /// 设置存档的顺序
    /// </summary>
    /// <param name="SaveIndex"></param>
    public void SetIndex(int SaveIndex)
    {
        Assert.IsTrue(SaveIndex >= 0, "存档Index需大于等于0");
        Assert.IsTrue(SaveIndex < 10, "存档Index需小于10");
        Index = SaveIndex;
    }
    public void RefreshPlayerInfo(List<RPGCharacter> Characters)
    {
        if (PlayersInfo == null)
            PlayersInfo = new PlayerInfoCollection();
        foreach (RPGCharacter Ch in Characters)
            PlayersInfo.AddContent(new CharacterInfo(Ch));
    }
    public override string GetKey()
    {
        return "ChapterRecord_" + Index;
    }
    public override string GetFullRecordPathName()
    {
        return Application.persistentDataPath + "/ChapterRecord_" + Index.ToString() + ".sav";
    }
}
[System.Serializable]
public class PlayerInfoCollection : SerializableList<CharacterInfo>
{
    public override string GetFullRecordPathName()
    {
        return Application.persistentDataPath + "/PlayerInfoCollection.sav";
    }
    public override string GetKey()
    {
        return "PlayerInfoCollection";
    }
    public override string ToString()
    {
        return GetKey() + "Has " + RecordList.Count + "PlayerInfo \n" + GetPlayersInfo();
    }
    private string GetPlayersInfo()
    {
        string temp = string.Empty;
        foreach (CharacterInfo C in RecordList)
        {
            temp += C.ToString() + "\n";
        }
        return temp;
    }
    [RuntimeInitializeOnLoadMethod]
    public static void III()
    {
        
    }
}
#endregion
/// <summary>
/// 该类从GameInstance里抓取东西
/// </summary>
public static class GameRecord
{
    public static void SaveTo(int index)
    {

    }
    public static void LoadFrom(int index)
    {

    }
}
