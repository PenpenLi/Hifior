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
    public Sequence.Sequence StartSequence;
    [Tooltip("结束剧情")]
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

}
