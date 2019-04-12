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
    public int Career;
    public CharacterAttribute Attribute;
    public ItemGroup Items;
    public override string GetKey()
    {
        return ID.ToString();
    }
    public CharacterInfo(RPGCharacter Character)
    {
        ID = Character.GetCharacterID();
        Level = Character.GetLevel();
        Exp = Character.GetLevel();
        Career = Character.GetCareer();
        Attribute = Character.GetAttribute();
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
    /// 当前运输队
    /// </summary>
    public Warehouse Ware;
    /// <summary>
    /// 章节
    /// </summary>
    public int Chapter;
    /// <summary>
    /// 是否已经播放完开始剧情
    /// </summary>
    public bool AfterStartSequence;
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
    /// <summary>
    /// 更新玩家的信息，如果存档中已经存在该玩家，则替换存在的玩家信息，如果不存在则添加该玩家信息
    /// </summary>
    /// <param name="Characters"></param>
    public void RefreshPlayersInfo(List<RPGCharacter> Characters)
    {
        if (PlayersInfo == null)
            PlayersInfo = new PlayerInfoCollection();
        foreach (RPGCharacter Ch in Characters)
        {
            if (PlayersInfo.HasCharacterInfo(Ch.GetCharacterID()))
            {
                PlayersInfo.RefreshCharacterInfo(Ch.GetCharacterID(), Ch);
            }
            else
            {
                PlayersInfo.AddContent(new CharacterInfo(Ch));
            }
        }
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
    /// <summary>
    /// 存档是否已经包含某个角色的信息
    /// </summary>
    /// <returns></returns>
    public bool HasCharacterInfo(int CharacterID,out CharacterInfo OutCharacterInfo)
    {
        CheckRecordList();
        foreach (CharacterInfo info in RecordList)
        {
            if (info.ID == CharacterID)
            {
                OutCharacterInfo = info;
                return true;
            }
        }
        OutCharacterInfo = null;
        return false;
    }    /// <summary>
         /// 存档是否已经包含某个角色的信息
         /// </summary>
         /// <returns></returns>
    public bool HasCharacterInfo(int CharacterID)
    {
        CheckRecordList();
        foreach (CharacterInfo info in RecordList)
        {
            if (info.ID == CharacterID)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 替换一个角色的信息，如果该角色不存在，则添加该角色信息
    /// </summary>
    /// <param name="CharacterID"></param>
    /// <param name="Character"></param>
    /// <returns></returns>
    public CharacterInfo RefreshCharacterInfo(int CharacterID, RPGCharacter Character)
    {
        for (int i = 0; i < RecordList.Count; i++)
        {
            if (RecordList[i].ID == CharacterID)
            {
                RecordList[i] = new CharacterInfo(Character);
                return RecordList[i];
            }
        }
        CharacterInfo NewInfo = new CharacterInfo(Character);
        RecordList.Add(NewInfo);
        return NewInfo;
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
        UGameInstance.Instance.SaveChapterToDisk(index);
    }
    /// <summary>
    /// 从磁盘载入章节，返回当前存档的数据，如果不存在则返回null
    /// </summary>
    /// <param name="Index">第几个存档</param>
    /// <returns>当前存档的数据</returns>
    public static ChapterRecordCollection LoadChapterRecordFrom(int index)
    {
        return UGameInstance.Instance.LoadChapterFromDisk(index);
    }
    /// <summary>
    /// 载入章节，并设置当前章节使用的存档数据
    /// </summary>
    /// <param name="index"></param>
    /// <param name="Record"></param>
    /// <returns></returns>
    public static void LoadChapterSceneWithRecordData( ChapterRecordCollection Record)
    {
        UGameInstance.Instance.LoadChapterScene(Record);
    }
    /// <summary>
    /// 载入序章0，不使用存档
    /// </summary>
    public static void LoadNewGame()
    {
        UGameInstance.Instance.LoadChapterScene(null);
    }
    /// <summary>
    /// 该处是否存在存档
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool HasSave(int index)
    {
        return UGameInstance.Instance.HasChapterSave(index);
    }
}
