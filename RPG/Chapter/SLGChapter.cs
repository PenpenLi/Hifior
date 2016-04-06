using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 用于战斗场景章节初始化，和SLGMap一起
/// </summary>
[RequireComponent(typeof(SLGMap))]
public class SLGChapter : UActor
{
    #region 事件结构
    [System.Serializable]
    public struct TurnEventType
    {
        public string Description;
        /// <summary>
        /// 是否可用,这个触发完就关闭
        /// </summary>
        public bool Enable;
        public int From;
        public int To;
        public EnumCharacterCamp TriggerCamp;
        public Sequence.Sequence Sequence;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
    }
    [System.Serializable]
    public struct BattltTalkEventType
    {
        public string Description;
        /// <summary>
        /// 是否可用,这个触发完就关闭
        /// </summary>
        public bool Enable;
        public int From;
        public int To;
        /// <summary>
        /// 是否互相触发
        /// </summary>
        public bool Mutual;
        public Sequence.Sequence Sequence;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
    }
    [System.Serializable]
    public struct LocationEventType
    {
        public string Description;
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable;
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
        /// 触发点
        /// </summary>
        public Point2D Location;
        public Sequence.Sequence Sequence;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
    }
    [System.Serializable]
    public struct RangeEventType
    {
        public string Description;
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable;
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
        public Sequence.Sequence Sequence;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
    }
    [System.Serializable]
    public struct EnemiesLessEventType
    {
        public string Description;
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable;
        /// <summary>
        /// 是否已经触发过了，触发过不一定关闭该事件，这个只指示是否触发过
        /// </summary>
        private bool HasTrigger;
        /// <summary>
        /// 触发数量
        /// </summary>
        public int TriggerNum;
        public Sequence.Sequence Sequence;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
    }
    [System.Serializable]
    public struct EnemyDieEventType
    {
        public string Description;
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable;
        /// <summary>
        /// 触发人物
        /// </summary>
        public int TriggerCharacterID;
        public Sequence.Sequence Sequence;
        /// <summary>
        /// 对相关事件设置Enable
        /// </summary>
        public List<EventEnableSwitch> Switcher;
    }
    #endregion
    [System.Serializable]
    public struct EventEnableSwitch
    {
        public EnumEventTriggerCondition EventType;
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
    public List<BattltTalkEventType> BattleTalkEvent;
    [Tooltip("敌方少于事件")]
    public List<EnemiesLessEventType> EnemiesLessEvent;
    [Tooltip("敌方死亡事件")]
    public List<EnemyDieEventType> EnemyDieEvent;

    public void Start()
    {
        StartSequence.gameObject.SetActive(true);
        //播放完开始剧情后再OnFinish里添加结束后的事件，显示章节第一回合开始或者弹出准备画面
        StartSequence.OnFinish.AddListener(() => { GetGameMode<GM_Battle>().OnStartSequenceFinish.Invoke(); });
    }
}
