using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System.IO;
/// <summary>
/// 自游戏开始到游戏结束一直存在的一个对象，用于数据的交换，可以被所有的对象获取到
/// 在一个空物体中创建该物体
/// </summary>
[HierarchyIcon("GameInstance.png")]
public class UGameInstance : MonoSingleton<UGameInstance>
{
    /// <summary>
    ///  战场模板场景
    /// </summary>
    public const int SCENEINDEX_BATTLE_TEMPLATE = 5;
    /// <summary>
    /// 章节结束记录场景
    /// </summary>
    public const int SCENEINDEX_CHAPTER_ENDSAVE = 4;
    private UGameMode ActiveGameMode;
    #region UE4
    public new T GetGameMode<T>() where T : UGameMode
    {
        if (ActiveGameMode == null)
            ActiveGameMode = GameObject.FindObjectOfType<UGameMode>();
        return (T)ActiveGameMode;
    }
    public new T GetPlayerPawn<T>() where T : UPawn
    {
        return ActiveGameMode.GetPlayerPawn<T>();
    }
    public new T GetPlayerController<T>() where T : UPlayerController
    {
        return ActiveGameMode.GetPlayerController<T>();
    }
    public new T GetPlayerState<T>() where T : UPlayerStatus
    {
        return ActiveGameMode.GetPlayerState<T>();
    }
    public T GetGameState<T>() where T : UGameStatus
    {
        return ActiveGameMode.GetGameStatus<T>();
    }

    /** virtual function to allow custom GameInstances an opportunity to set up what it needs */
    public virtual void Initiate() { }

    /** virtual function to allow custom GameInstances an opportunity to do cleanup when shutting down */
    public virtual void Shutdown() { }
    public virtual UPawn CreateInitialPlayer(out string OutError)
    {
        OutError = null;
        return null;
    }

    public static bool InputModeUI { get; private set; }
    public static bool InputModeGame { get; private set; }
    /// <summary>
    /// 键盘只接受UI事件
    /// </summary>
    public void SetInputModeUIOnly()
    {
        InputModeUI = true;
        InputModeGame = false;
    }
    /// <summary>
    /// 键盘只接受游戏内Actor
    /// </summary>
    public void SetInputModeGameOnly()
    {
        InputModeGame = true;
        InputModeUI = false;
    }
    /// <summary>
    /// 同时接受UI和游戏事件
    /// </summary>
    public void SetInputModeGameAndUI()
    {
        InputModeUI = true;
        InputModeGame = true;
    }
    public void SetGameMode(UGameMode GameMode)
    {
        UnityEngine.Assertions.Assert.IsNotNull<UGameMode>(GameMode, "The GameMode you assign is null");
        ActiveGameMode = GameMode;
    }
    #endregion

    #region AssetBundle
    protected string BundleURL;
    protected string AssetName;
    protected int version;
    public static T LoadAssetFromBundle<T>(string URL, string AssetName) where T : UnityEngine.Object
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(URL);
        T tem = bundle.LoadAsset<T>(AssetName);
        bundle.Unload(false);
        return tem;
    }

    #endregion

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
        GM_Battle GameMode = GetGameMode<GM_Battle>();
        TempChapterEndRecord = new ChapterRecordCollection();
        TempChapterEndRecord.Chapter = ChapterID;
        TempChapterEndRecord.AfterStartSequence = AfterStartSequence;
        TempChapterEndRecord.AvailablePlayers = AvailablePlayers;
        TempChapterEndRecord.Ware = Ware;
        TempChapterEndRecord.RefreshPlayersInfo(GetGameStatus<UGameStatus>().GetLocalPlayers());
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

    public override void Awake()
    {
        base.Awake();

        SetInputModeGameAndUI();
        ActiveGameMode = GameObject.FindObjectOfType<UGameMode>();
        Initiate();
    }
    /// <summary>
    /// 进入章节场景,BattleTemplate作为起始index为0的章节
    /// </summary>
    /// <param name="ChapterID"></param>
    public void LoadChapterScene(ChapterRecordCollection Record)
    {
        ChapterRecord = Record;
        Debug.Log("载入章节:" + ChapterID);
        //在此通过ChapterRecord 初始化所有的数据
        if (ChapterRecord != null)
        {
            LoadingScreenManager.LoadScene(SCENEINDEX_BATTLE_TEMPLATE + Record.Chapter);
        }
        else
        {
            LoadingScreenManager.LoadScene(SCENEINDEX_BATTLE_TEMPLATE + 0);
        }
    }
}
