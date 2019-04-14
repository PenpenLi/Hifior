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
    public int MaxHP;
    public int CurrentHP;
    public CharacterAttribute Attribute;
    public ItemGroup Items;
    public override string GetKey()
    {
        return ID.ToString();
    }
    public CharacterInfo() { }
    public CharacterInfo(RPGCharacter Character)
    {
        ID = Character.GetCharacterID();
        Level = Character.Logic().GetLevel();
        Exp = Character.Logic().GetLevel();
        Career = Character.Logic().GetCareer();
        Attribute = Character.Logic().GetAttribute();
        CurrentHP = Character.Logic().GetCurrentHP();
        MaxHP = Character.Logic().GetMaxHP();
    }
    public CharacterInfo(CharacterDef def)
    {
        ID = def.CommonProperty.ID;
        Level = def.DefaultLevel;
        Exp = 0;
        Career = def.Career;
        Attribute = def.DefaultAttribute;
        CurrentHP = Attribute.HP;
        MaxHP = Attribute.HP;
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
public class GameRecord
{

    #region 存档相关函数
    private ChapterRecordCollection ChapterRecord;
    /// <summary>
    /// 章节结束时保存当前章节的信息到这个变量里
    /// </summary>
    private ChapterRecordCollection TempChapterEndRecord;
    /// <summary>
    /// 当前存档章节数
    /// </summary>
    public int ChapterID;
    public Warehouse Ware;
    /// <summary>
    /// 当前可用的角色，保留在存档中，每一章开始时从存档读取，当有我方人物加入时，写入该表，当有角色离开时，从该表里删除，每一章结束后存储到Save文件里
    /// </summary>
    private List<int> AvailablePlayers = new List<int>();
    /// <summary>
    /// 获取所有可用的角色列表
    /// </summary>
    /// <returns></returns>
    public List<int> GetAvailablePlayersID()
    {
        return AvailablePlayers;
    }
    public List<CharacterInfo> GetAvailablePlayersInfo()
    {
        List<CharacterInfo> L = new List<CharacterInfo>();
        for (int i = 0; i < AvailablePlayers.Count; i++)
        {
            CharacterInfo info;
            if (ChapterRecord.PlayersInfo.HasCharacterInfo(AvailablePlayers[i], out info))
            {
                L.Add(info);
            }
        }
        return L;
    }
    /// <summary>
    /// 添加角色为可用
    /// </summary>
    /// <param name="ID"></param>
    public void AddAvailablePlayer(int ID)
    {
        if (AvailablePlayers.Contains(ID))
            return;
        AvailablePlayers.Add(ID);
    }
    /// <summary>
    /// 移除可用角色，当角色因为剧情事件被移除时执行，死亡时不会移除。被移除的角色将不会在准备画面显示
    /// </summary>
    /// <param name="ID"></param>
    public void RemoveAvailablePlayer(int ID)
    {
        if (AvailablePlayers.Contains(ID))
            AvailablePlayers.Remove(ID);
        else
            Debug.LogError("你想要移除的有效角色并不存在");
    }
    /// <summary>
    /// 章节结束保存下数据缓存
    /// </summary>
    /// <param name="AfterStartSequence">是否是开始剧情播放完记录的</param>
    public void SaveChapter(bool AfterStartSequence)
    {
        // GM_Battle GameMode = GetGameMode<GM_Battle>();
        TempChapterEndRecord = new ChapterRecordCollection();
        TempChapterEndRecord.Chapter = ChapterID;
        TempChapterEndRecord.AfterStartSequence = AfterStartSequence;
        TempChapterEndRecord.AvailablePlayers = AvailablePlayers;
        TempChapterEndRecord.Ware = Ware;
        //TempChapterEndRecord.RefreshPlayersInfo(GetGameStatus<UGameStatus>().GetLocalPlayers());
    }

    public void SaveChapterToDisk(int Index)
    {
        Assert.IsNotNull(TempChapterEndRecord, "章节存档尚未初始化");
        TempChapterEndRecord.SetIndex(Index);
        TempChapterEndRecord.SaveBinary();
    }
    public bool HasChapterSave(int Index)
    {
        ChapterRecord = new ChapterRecordCollection();
        ChapterRecord.SetIndex(Index);
        return ChapterRecord.Exists();
    }
    /// <summary>
    /// 从磁盘载入章节，返回章节数，并实例化ChapterRecord文件，如果没有则返回-1,并让ChapterRecord为null
    /// </summary>
    /// <param name="Index">第几个存档</param>
    /// <returns>当前存档的章节数</returns>
    public ChapterRecordCollection LoadChapterFromDisk(int Index)
    {
        if (HasChapterSave(Index))
        {
            ChapterRecord = ChapterRecord.LoadBinary<ChapterRecordCollection>();

            AvailablePlayers = ChapterRecord.AvailablePlayers;
        }
        else
        {
            ChapterRecord = null;
        }
        return ChapterRecord;
    }
    /// <summary>
    /// 是否有可用的章节存档
    /// </summary>
    /// <returns></returns>
    public bool IsChapterRecordAvailable()
    {
        return ChapterRecord != null;
    }
    /// <summary>
    /// 获取当前应用的章节存档
    /// </summary>
    /// <returns></returns>
    public ChapterRecordCollection GetCurrentChapterRecord()
    {
        return ChapterRecord;
    }
    #endregion
    public static void SaveTo(int index)
    {
       // UGameInstance.Instance.SaveChapterToDisk(index);
    }
    /// <summary>
    /// 从磁盘载入章节，返回当前存档的数据，如果不存在则返回null
    /// </summary>
    /// <param name="Index">第几个存档</param>
    /// <returns>当前存档的数据</returns>
    public static ChapterRecordCollection LoadChapterRecordFrom(int index)
    {
        return new ChapterRecordCollection();
        //return UGameInstance.Instance.LoadChapterFromDisk(index);
    }
    /// <summary>
    /// 载入章节，并设置当前章节使用的存档数据
    /// </summary>
    /// <param name="index"></param>
    /// <param name="Record"></param>
    /// <returns></returns>
    public static void LoadChapterSceneWithRecordData( ChapterRecordCollection Record)
    {
        //UGameInstance.Instance.LoadChapterScene(Record);
    }
    /// <summary>
    /// 载入序章0，不使用存档
    /// </summary>
    public static void LoadNewGame()
    {
       // UGameInstance.Instance.LoadChapterScene(null);
    }
    /// <summary>
    /// 该处是否存在存档
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool HasSave(int index)
    {
        // return UGameInstance.Instance.HasChapterSave(index);
        return false;
    }
}
