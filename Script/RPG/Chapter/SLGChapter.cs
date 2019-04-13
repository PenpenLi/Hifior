using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class SLGChapter : MonoBehaviour
{
    Grid grid;
    #region 事件结构
    [System.Serializable]
    public class EventTypeBase
    {
        public string Description;
        /// <summary>
        /// 是否可用,这个触发完就关闭
        /// </summary>
        public bool Enable;
        public Sequence.Sequence Sequence;
        public EventTypeBase()
        {
            Enable = true;
        }
        /// <summary>
        /// 执行Sequence
        /// </summary>
        public virtual void Execute(UnityAction OnFinish)
        {
            UnityEngine.Assertions.Assert.IsNotNull(Sequence, "即将执行的Sequence为Null");
            Enable = false;
            Sequence.Execute(OnFinish);
        }
    }
    [System.Serializable]
    public class TurnEventType : EventTypeBase
    {
        public int From = 0;
        public int To = 0;
        public EnumCharacterCamp TriggerCamp;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
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
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
        public override void Execute(UnityAction OnFinish)
        {
            base.Execute(OnFinish);
            Enable = false;
        }
    }
    public enum EnumLocationEventCaption
    {
        宝箱,
        访问村庄,
        占领,
        开门,
        使用开关
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
        private bool HasTrigger = false;
        /// <summary>
        /// 指定的人才可以触发
        /// </summary>
        public int DedicatedCharacter = -1;
        /// <summary>
        /// 触发点
        /// </summary>
        public VInt2 Location = VInt2.InvalidPoint;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
        public override void Execute(UnityAction OnFinish)
        {
            base.Execute(OnFinish);

            HasTrigger = true;
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
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
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
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
    }
    [System.Serializable]
    public class EnemyDieEventType : EventTypeBase
    {
        /// <summary>
        /// 触发人物
        /// </summary>
        public int TriggerCharacterID;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
    }
    #endregion

    #region 事件函数
    [System.Serializable]
    public struct EventEnableSwitch
    {
        public EnumEventTriggerCondition EventType;
        int Index;
        public bool Enable;
    }

    [Tooltip("章节的设置")]
    public ChapterSettingDef ChapterSetting;
    // 章节的事件设置 public Event
    // 章节的单位设置 敌方单位集合
    [Tooltip("开始剧情")]
    public Sequence.Sequence StartSequence;
    [Tooltip("结束剧情")]
    public Sequence.Sequence EndSequence;
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

    public void Start()
    {
        grid = GetComponent<Grid>();
        //播放完开始剧情后再OnFinish里添加结束后的事件，显示章节第一回合开始或者弹出准备画面
        //StartSequence.OnFinish.AddListener();
        // StartSequence.Execute(GetGameMode<GM_Battle>().OnStartSequenceFinished);
    }
    public LocationEventType GetLocationEvent(VInt2 TilePosition, int CharacterID)
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
    public RangeEventType GetRangeEvent(VInt2 TilePosition)
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
    #endregion

    #region 章节设置
    public bool CheckWin_KillAllEnemy()
    {
        if (HasWinCondition(EnumWinCondition.全灭敌人))// && GetGameStatus<GS_Battle>().GetNumLocalEnemies() == 0
            return true;
        else
            return false;
    }
    /// <summary>
    /// 每一个加入的Boss都有一个唯一的ID，从0开始增长
    /// </summary>
    /// <param name="BossID"></param>
    /// <returns></returns>
    public bool CheckWin_DefeatBoss(int BossID)
    {
        if (HasWinCondition(EnumWinCondition.击败指定Boss))
            return true;
        return false;
    }
    /// <summary>
    /// 在Sequence里添加Start() 在Start里将GameMode 里的当前CityID加入
    /// </summary>
    /// <returns></returns>
    public bool CheckWin_Seize(int CityID = 0)
    {
        if (HasWinCondition(EnumWinCondition.压制指定城池) && ChapterSetting.WinCondition.CityID == CityID)
            return true;
        return false;
    }
    public bool CheckWin_Leave()
    {
        if (HasWinCondition(EnumWinCondition.领主地点撤离))
            return true;
        return false;
    }
    public bool CheckWin_Round(int Round)
    {
        if (HasWinCondition(EnumWinCondition.回合坚持) && Round == ChapterSetting.WinCondition.Round)
            return true;
        return false;
    }
    public List<string> GetWinConditionText()
    {
        List<string> texts = new List<string>();
        List<EnumWinCondition> L = GetAllWinCondition();
        if (L.Contains(EnumWinCondition.全灭敌人))
            texts.Add("击败所有敌方单位");
        if (L.Contains(EnumWinCondition.击败全部Boss))
            texts.Add("击败所有Boss");
        if (L.Contains(EnumWinCondition.击败指定Boss))
            texts.Add("击败" + ResourceManager.GetEnemyDef(ChapterSetting.WinCondition.BossID));
        if (L.Contains(EnumWinCondition.回合坚持))
            texts.Add("坚持" + ChapterSetting.WinCondition.Round + "个回合");
        if (L.Contains(EnumWinCondition.领主地点撤离))
            texts.Add("在指定地点撤离");
        if (L.Contains(EnumWinCondition.压制指定城池))
            texts.Add("压制指定城池");
        if (L.Contains(EnumWinCondition.压制所有城池))
            texts.Add("压制所有城池");
        return texts;
    }
    public List<EnumWinCondition> GetAllWinCondition()
    {
        int max = 6;
        List<EnumWinCondition> L = new List<EnumWinCondition>();
        for (int i = 0; i < max; i++)
        {
            if (EnumTables.MaskFieldIdentify(ChapterSetting.WinCondition.Condition, i))
            {
                L.Add((EnumWinCondition)i);
            }
        }
        return L;
    }
    public bool HasWinCondition(EnumWinCondition Condition)
    {
        return EnumTables.MaskFieldIdentify(ChapterSetting.WinCondition.Condition, (int)Condition);
    }
    #endregion
}
