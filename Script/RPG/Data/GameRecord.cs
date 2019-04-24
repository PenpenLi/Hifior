using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;
using UnityEngine.Events;
#region 序列化类类型
[System.Serializable]
public class CharacterInfo : SerializableBase
{
    public enum EBattleState
    {
        Normal,
        Frozen,
        Poison
    }
    /// <summary>
    /// 属于哪一方
    /// </summary>
    public EnumCharacterCamp Camp;
    public int ID;
    public bool Alive;
    public bool Available;
    public int Level;
    public int Exp;
    public int Career;
    public int MaxHP;
    public int CurrentHP;
    public EBattleState battleState;
    public Vector2Int tileCoords;
    public CharacterAttribute Attribute;
    public ItemGroup Items;
    public ActionAI AI;
    [NonSerialized]
    public Vector2Int oldTileCoords;
    public override string GetKey()
    {
        return ID.ToString();
    }
    private CharacterInfo() { }

    //[RuntimeInitializeOnLoadMethod]
    //public static void LoadFromRecord() {
    //    CharacterInfo info = new CharacterInfo();
    //   info= info.Load<CharacterInfo>();
    //    Debug.Log(info.Items.Weapons.Count);
    //}
    public CharacterInfo(CharacterLogic Logic)
    {
        ID = Logic.GetID();
        Level = Logic.GetLevel();
        Exp = Logic.GetLevel();
        Career = Logic.GetCareer();
        Attribute = Logic.GetAttribute();
        CurrentHP = Logic.GetCurrentHP();
        MaxHP = Logic.GetMaxHP();
        Items = new ItemGroup();

    }
    public CharacterInfo(PlayerDef def)
    {
        ID = def.CommonProperty.ID;
        Level = def.DefaultLevel;
        Exp = 0;
        Career = def.Career;
        Attribute = def.DefaultAttribute;
        CurrentHP = Attribute.HP;
        MaxHP = Attribute.HP;
        Items = new ItemGroup();
        Items.AddWeapons(def.DefaultWeapons);
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
    /// <summary>
    /// 既存活同时又允许出场
    /// </summary>
    public bool Active { get { return Alive && Available; } }
    public void AfterBattle()
    {
        CurrentHP = MaxHP;
        battleState = EBattleState.Normal;
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
    public bool HasCharacterInfo(int CharacterID, out CharacterInfo OutCharacterInfo)
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
                RecordList[i] = new CharacterInfo(Character.Logic);
                return RecordList[i];
            }
        }
        CharacterInfo NewInfo = new CharacterInfo(Character.Logic);
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
}

[System.Serializable]
public class ChapterRecordCollection : SerializableBase
{
    public ChapterRecordCollection()
    {
        team = new List<TeamCollection>();
        for (int i = 0; i < ConstTable.TEAM_COUNT; i++)
        {
            team.Add(new TeamCollection());
            team[i].TeamID = i;
        }
    }
    [System.Serializable]
    public class TeamCollection
    {
        public int TeamID;
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
        /// 当前运输队
        /// </summary>
        public Warehouse Ware;

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
                if (PlayersInfo.HasCharacterInfo(Ch.Logic.GetID()))
                {
                    PlayersInfo.RefreshCharacterInfo(Ch.Logic.GetID(), Ch);
                }
                else
                {
                    PlayersInfo.AddContent(new CharacterInfo(Ch.Logic));
                }
            }
        }
    }
    /// <summary>
    /// 存档顺序
    /// </summary>
    public int Slot;
    public int CurrentTeamIndex;
    public List<TeamCollection> team;
    public TeamCollection CurrentTeam { get { return team[CurrentTeamIndex]; } }
    public List<CharacterInfo> CurrentTeamPlayerInfo { get { return CurrentTeam.PlayersInfo.RecordList; } }
    /// <summary>
    /// 设置存档的顺序
    /// </summary>
    /// <param name="SaveIndex"></param>
    public void SetIndex(int SaveIndex)
    {
        Assert.IsTrue(SaveIndex >= 0, "存档Index需大于等于0");
        Assert.IsTrue(SaveIndex < 10, "存档Index需小于10");
        Slot = SaveIndex;
    }

    public override string GetKey()
    {
        return "ChapterRecord_" + Slot;
    }
    public override string GetFullRecordPathName()
    {
        return Application.persistentDataPath + "/" + Slot + "/ChapterRecord.sav";
    }
}

[System.Serializable]
public class BattleInfoCollection : ChapterRecordCollection
{
    public int MapID;
    public EventInfoCollection Event;
    public WinCondition WinCondition;
    public override string GetKey()
    {
        return "BattleInfo";
    }
    public override string GetFullRecordPathName()
    {
        return Application.persistentDataPath + "/BattleInfo.sav";
    }
}
[System.Serializable]
public class EventInfoCollection
{
    #region 事件结构

    [System.Serializable]
    public struct EventEnableSwitch
    {
        public EnumEventTriggerCondition EventType;
        public int Index;
        public bool Enable;
    }
    //添加了Serializable标记的变量都需要被记录到存档中
    [System.Serializable]
    public class EventTypeBase
    {
        public string Description;
        /// <summary>
        /// 是否可用,这个触发完就关闭
        /// </summary>
        public bool Enable;
        public string SequenceName;
        public Sequence.Sequence Sequence;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
        public EventTypeBase()
        {
            Enable = true;
        }
        public EventTypeBase(string seqName)
        {
            Enable = true;
        }
        /// <summary>
        /// 执行Sequence
        /// </summary>
        public virtual void Execute(EventInfoCollection eventCollection, UnityAction OnFinish)
        {
            UnityEngine.Assertions.Assert.IsNotNull(Sequence, "即将执行的Sequence为Null");
            Sequence.Execute(OnFinish);
            foreach (var s in Switcher)
            {
                eventCollection.GetEventBase(s.EventType, s.Index).Enable = s.Enable;
            }
        }
        public override string ToString()
        {

            return StringHelper.PrintVariablesOf(this);
        }
    }
    [System.Serializable]
    public class TurnEventType : EventTypeBase
    {
        public int From = 0;
        public int To = 0;
        public EnumCharacterCamp TriggerCamp;
    }
    [System.Serializable]
    public class BattleTalkEventType : EventTypeBase
    {
        /// <summary>
        /// 发送者的ID
        /// </summary>
        public int Sender = -1;
        /// <summary>
        /// 接收者的ID
        /// </summary>
        public int Receiver = -1;
        /// <summary>
        /// 接收者的阵营
        /// </summary>
        public EnumCharacterCamp ReceiverCamp;
        /// <summary>
        /// 是否互相触发
        /// </summary>
        public bool Mutual;
        public override void Execute(EventInfoCollection eventCollection, UnityAction OnFinish)
        {
            Enable = false;
            base.Execute(eventCollection, OnFinish);
        }
        public string GetButtonString()
        {
            return "对话";
        }
    }
    public enum EnumLocationEventCaption
    {
        宝箱,
        访问,
        占领,
        开门,
        开关
    }
    [System.Serializable]
    public class LocationEventType : EventTypeBase
    {
        /// <summary>
        /// 显示的文字
        /// </summary>
        public EnumLocationEventCaption Caption;
        /// <summary>
        /// 是否已经触发过了，触发过不一定关闭该事件，这个只指示是否触发过
        /// </summary>
        public bool HasTrigger = false;
        public bool TriggerOnceOnly = true;
        /// <summary>
        /// 指定的人才可以触发
        /// </summary>
        public int DedicatedCharacter = -1;
        /// <summary>
        /// 触发点
        /// </summary>
        public Vector2Int Location;
        public override void Execute(EventInfoCollection eventCollection, UnityAction OnFinish)
        {
            if (TriggerOnceOnly) Enable = false;
            HasTrigger = true;
            base.Execute(eventCollection, OnFinish);
        }
        public string GetButtonText()
        {
            return Caption.ToString();
        }
    }
    [System.Serializable]
    public class RangeEventType : EventTypeBase
    {
        /// <summary>
        /// 是否已经触发过了，触发过不一定关闭该事件，这个只指示是否触发过
        /// </summary>
        private bool HasTrigger;
        /// <summary>
        /// 指定的人才可以触发
        /// </summary>
        public int DedicatedCharacter;
        /// <summary>
        /// 指定的人才可以触发
        /// </summary>
        public int DedicatedCareer;
        /// <summary>
        /// 触发区域
        /// </summary>
        public Range2D Range;
        public override void Execute(EventInfoCollection eventCollection, UnityAction OnFinish)
        {
            Enable = false;
            base.Execute(eventCollection, OnFinish);
        }
    }
    [System.Serializable]
    public class EnemiesLessEventType : EventTypeBase
    {
        /// <summary>
        /// 是否已经触发过了，触发过不一定关闭该事件，这个只指示是否触发过
        /// </summary>
        private bool HasTrigger;
        /// <summary>
        /// 触发数量
        /// </summary>
        public int TriggerNum;
        public override void Execute(EventInfoCollection eventCollection, UnityAction OnFinish)
        {
            Enable = false;
            base.Execute(eventCollection, OnFinish);
        }
    }
    [System.Serializable]
    public class EnemyDieEventType : EventTypeBase
    {
        /// <summary>
        /// 触发人物
        /// </summary>
        public int TriggerCharacterID;
        public override void Execute(EventInfoCollection eventCollection, UnityAction OnFinish)
        {
            Enable = false;
            base.Execute(eventCollection, OnFinish);
        }
    }
    #endregion

    [Tooltip("回合事件")]
    public List<TurnEventType> TurnEvent;
    [Tooltip("位置事件")]
    public List<LocationEventType> LocationEvent;
    [Tooltip("范围事件")]
    public List<RangeEventType> RangeEvent;
    [Tooltip("对话事件")]
    public List<BattleTalkEventType> BattleTalkEvent;
    [Tooltip("敌方少于事件")]
    public List<EnemiesLessEventType> EnemiesLessEvent;
    [Tooltip("敌方死亡事件")]
    public List<EnemyDieEventType> EnemyDieEvent;
    [Tooltip("胜利条件")]
    public List<WinCondition> WinCondition;

    public EventTypeBase GetEventBase(EnumEventTriggerCondition cond, int index)
    {
        switch (cond)
        {
            case EnumEventTriggerCondition.位置事件:
                return LocationEvent[index];
            case EnumEventTriggerCondition.回合事件:
                return TurnEvent[index];
            case EnumEventTriggerCondition.敌人少于事件:
                return EnemiesLessEvent[index];
            case EnumEventTriggerCondition.敌人死亡事件:
                return EnemyDieEvent[index];
            case EnumEventTriggerCondition.范围事件:
                return RangeEvent[index];
            case EnumEventTriggerCondition.战场对话事件:
                return BattleTalkEvent[index];
        }
        return null;
    }
    public LocationEventType GetLocationEvent(Vector2Int TilePosition, int CharacterID)
    {
        foreach (LocationEventType Event in LocationEvent)
        {
            if (Event.Enable && Event.Location == TilePosition)
            {
                if (Event.DedicatedCharacter < 0 || CharacterID == Event.DedicatedCharacter)
                    return Event;
            }
        }
        return null;
    }
    public TurnEventType GetTurnEvent(int Round, EnumCharacterCamp Camp)
    {
        foreach (TurnEventType Event in TurnEvent)
        {
            if (Event.Enable && Event.TriggerCamp == Camp && Round >= Event.From && Round <= Event.To)
            {
                return Event;
            }
        }
        return null;
    }
    public RangeEventType GetRangeEvent(Vector2Int TilePosition)
    {
        foreach (RangeEventType Event in RangeEvent)
        {
            if (Event.Enable && Range2D.InRange(TilePosition.x, TilePosition.y, Event.Range))
            {
                return Event;
            }
        }
        return null;
    }
    public BattleTalkEventType GetBattleTalkEvent(int SenderCharacterID, int ReceiverCharacterID, EnumCharacterCamp ReceiverCamp)
    {
        foreach (BattleTalkEventType Event in BattleTalkEvent)
        {
            if (Event.Enable && Event.ReceiverCamp == ReceiverCamp)
            {
                if (Event.Mutual)
                {
                    if ((Event.Sender == SenderCharacterID && Event.Receiver == ReceiverCharacterID) || (Event.Sender == ReceiverCharacterID && Event.Receiver == SenderCharacterID))
                        return Event;
                }
                else
                {
                    if (Event.Sender == SenderCharacterID && Event.Receiver == ReceiverCharacterID)
                        return Event;
                }
            }
        }
        return null;
    }
    public EnemiesLessEventType GetEnemiesLessEvent(int EnemyCount)
    {
        foreach (EnemiesLessEventType Event in EnemiesLessEvent)
        {
            if (Event.Enable && EnemyCount <= Event.TriggerNum)
            {
                return Event;
            }
        }
        return null;
    }
    public EnemyDieEventType GetEnemyDieEvent(int DiedCharacterID)
    {
        foreach (EnemyDieEventType Event in EnemyDieEvent)
        {
            if (Event.Enable && Event.TriggerCharacterID == DiedCharacterID)
            {
                return Event;
            }
        }
        return null;
    }
}
#endregion

/// <summary>
/// 该类从GameInstance里抓取东西
/// </summary>
public class GameRecord
{
    public UnityAction OnSaveFileBroke;
    public UnityAction OnSaveFileNotExist;
    public UnityAction OnFinishSave;

    #region 存档相关函数
    /// <summary>
    /// 章节结束时保存当前章节的信息到这个变量里
    /// </summary>
    private ChapterRecordCollection currentTeamRecord;
    public static string RootDataPath { get { return Application.persistentDataPath; } }
    private List<CharacterInfo> currentTeamCharacterInfo { get { return currentTeamRecord.CurrentTeam.PlayersInfo.RecordList; } }
    public List<CharacterInfo> GetAvailablePlayersInfo()
    {
        List<CharacterInfo> L = new List<CharacterInfo>();
        foreach (var v in currentTeamCharacterInfo)
        {
            if (v.Active)
            {
                L.Add(v);
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
        foreach (var v in currentTeamCharacterInfo)
        {
            if (v.ID == ID && v.Available == false) v.Available = true;
        }
    }
    /// <summary>
    /// 移除可用角色，当角色因为剧情事件被移除时执行，死亡时不会移除。被移除的角色将不会在准备画面显示
    /// </summary>
    /// <param name="ID"></param>
    public void RemoveAvailablePlayer(int ID)
    {
        foreach (var v in currentTeamCharacterInfo)
        {
            if (v.ID == ID && v.Available) v.Available = false;
        }
        Debug.LogError("你想要移除的有效角色并不存在");
    }
    /// <summary>
    /// 章节结束保存下数据缓存
    /// </summary>
    /// <param name="AfterStartSequence">是否是开始剧情播放完记录的</param>
    public void SaveChapter(int slot, int teamIndex, bool AfterStartSequence, int chapterId, Warehouse ware, List<CharacterInfo> infos)
    {
        //设置存储位置，存储队伍的信息
        currentTeamRecord = new ChapterRecordCollection();
        currentTeamRecord.Slot = slot;
        currentTeamRecord.CurrentTeamIndex = teamIndex;
        //设置队伍数据
        ChapterRecordCollection.TeamCollection teamData = new ChapterRecordCollection.TeamCollection();
        teamData.AfterStartSequence = AfterStartSequence;
        teamData.Chapter = chapterId;
        teamData.Ware = ware;
        teamData.PlayersInfo = new PlayerInfoCollection();
        foreach (var v in infos)
        {
            teamData.PlayersInfo.AddContent(v);
        }
        currentTeamRecord.team[teamIndex] = teamData;
        currentTeamRecord.Save();
    }
    public void SaveBattle(int teamIndex, int chapterId, int mapId, Warehouse ware, List<CharacterInfo> infos, EventInfoCollection eventInfo)
    {
        BattleInfoCollection battleInfo = new BattleInfoCollection();
        battleInfo.Slot = -1;
        battleInfo.CurrentTeamIndex = teamIndex;
        battleInfo.MapID = mapId;
        ChapterRecordCollection.TeamCollection teamData = new ChapterRecordCollection.TeamCollection();
        teamData.AfterStartSequence = true;
        teamData.Chapter = chapterId;
        teamData.Ware = ware;
        teamData.PlayersInfo = new PlayerInfoCollection();
        foreach (var v in infos)
        {
            teamData.PlayersInfo.AddContent(v);
        }
        battleInfo.team[teamIndex] = teamData;
        battleInfo.Event = eventInfo;
        battleInfo.Save();

    }
    public bool HasChapterSave(int slot)
    {
        var v = new ChapterRecordCollection();
        v.SetIndex(slot);
        return v.Exists();
    }
    /// <summary>
    /// 从磁盘载入章节，返回章节数，并实例化ChapterRecord文件，如果没有则返回-1,并让ChapterRecord为null
    /// </summary>
    /// <param name="slot">第几个存档</param>
    /// <returns>当前存档的章节数</returns>
    public ChapterRecordCollection LoadChapterFromDisk(int slot)
    {
        ChapterRecordCollection v =new ChapterRecordCollection();
        if (HasChapterSave(slot))
        {
            v = v.Load<ChapterRecordCollection>();
        }
        else
        {
            Debug.LogError("没有发现可以被载入的存储文件");
            OnSaveFileNotExist();
        }
        return v;
    }
    public BattleInfoCollection LoadBattleFromDisk(int slot)
    {
        BattleInfoCollection v = new BattleInfoCollection();
        if (v.Exists())
        {
            v = v.Load<BattleInfoCollection>();
        }
        else
        {
            Debug.LogError("没有发现可以被载入的存储文件");
            OnSaveFileNotExist();
        }
        return v;
    }
    /// <summary>
    /// 获取当前应用的章节存档
    /// </summary>
    /// <returns></returns>
    public ChapterRecordCollection GetCurrentChapterRecord()
    {
        return currentTeamRecord;
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
    public static void LoadChapterSceneWithRecordData(ChapterRecordCollection Record)
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
