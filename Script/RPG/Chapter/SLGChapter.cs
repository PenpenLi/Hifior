using UnityEngine;
using System.Collections.Generic;
public class SLGChapter : MonoBehaviour
{
    [ContextMenu("Check Sequence Name")]
    public void CheckSequenceName()
    {
        Dictionary<string, Transform> v = new Dictionary<string, Transform>();
        foreach (Transform t in transform)
        {
            if (t.GetComponent<Sequence.Sequence>() != null && v.ContainsKey(t.name))
            {
                Debug.LogError("存在相同的Sequence位于Transform:" + t.name);
            }
            v.Add(t.name, t);
        }
    }
    #region 事件函数

    private ChapterSettingDef chapterDef;
    public ChapterSettingDef ChapterSetting { get { return chapterDef; } }
    public void SetChapterDef(ChapterSettingDef def) { chapterDef = def; }
    // 章节的事件设置 public Event
    // 章节的单位设置 敌方单位集合
    [Tooltip("开始剧情")]
    [System.NonSerialized]
    public Sequence.Sequence StartSequence;
    [Tooltip("结束剧情")]
    [System.NonSerialized]
    public Sequence.Sequence EndSequence;
    [Tooltip("触发事件")]
    public EventInfoCollection EventInfo;
    public void Start()
    {
        //播放完开始剧情后再OnFinish里添加结束后的事件，显示章节第一回合开始或者弹出准备画面
        //StartSequence.OnFinish.AddListener();
        // StartSequence.Execute(GetGameMode<GM_Battle>().OnStartSequenceFinished);
    }
    #endregion

    #region 章节设置 检查胜利

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
    #region 章节设置 检查事件
    public void CheckTurn(int round, EnumCharacterCamp camp)
    {
        var turnEvent = EventInfo.GetTurnEvent(round, camp);
        if (turnEvent == null) Debug.Log("没有Turn事件");
        else Debug.Log("找到相匹配的Turn Event"+turnEvent);
    }
    public void CheckLocation(Vector2Int tilePos,int characterId)
    {
        var locationEvent = EventInfo.GetLocationEvent(tilePos, characterId);
        if (locationEvent == null) Debug.Log("没有Location事件");
        else Debug.Log("找到相匹配的Location Event" + locationEvent);
    }
    public void CheckEnemyLess(int count)
    {
        var lessEvent = EventInfo.GetEnemiesLessEvent(count);
        if (lessEvent == null) Debug.Log("没有Enemy Less事件");
        else Debug.Log("找到相匹配的Enemy Less Event" + lessEvent);
    }
    #endregion
}
