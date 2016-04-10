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
    private UGameMode ActiveGameMode;
    #region UE4
    public new T GetGameMode<T>() where T : UGameMode
    {
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
    public virtual void Init() { }

    /** virtual function to allow custom GameInstances an opportunity to do cleanup when shutting down */
    public virtual void Shutdown() { }
    public virtual UPawn CreateInitialPlayer(out string OutError)
    {
        OutError = null;
        return null;
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
    public T LoadAssetFromBundle<T>(string URL, string AssetName) where T : UnityEngine.Object
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
    /// 章节结束保存下数据缓存
    /// </summary>
    public void SaveChapter()
    {
        ChapterRecord = new ChapterRecordCollection();
        ChapterRecord.Chapter = 1;
        List<int> AvailablePlayer = new List<int>();
        AvailablePlayer.Add(1);
        AvailablePlayer.Add(2);
        ChapterRecord.AvailablePlayers = AvailablePlayer;
        ChapterRecord.Money = 10000;
        ChapterRecord.RefreshPlayerInfo(GetGameStatus<UGameStatus>().GetLocalPlayers());
    }
    public void SaveToDisk(int Index)
    {
        Assert.IsNotNull(ChapterRecord, "章节存档尚未初始化");
        ChapterRecord.SetIndex(Index);
        ChapterRecord.SaveBinary();
    }
    public void LoadFromDisk(int Index)
    {
        ChapterRecord = new ChapterRecordCollection();
        if (ChapterRecord.Exists())
        {
            ChapterRecord = ChapterRecord.LoadBinary<ChapterRecordCollection>();

            Debug.Log(ChapterRecord.Money);
            Debug.Log(ChapterRecord.PlayersInfo);
        }
        else
        {
            Debug.LogError("请先保存" + ChapterRecord.GetFullRecordPathName());
        }
    }
    #endregion

    public override void Awake()
    {
        base.Awake();

        ActiveGameMode = GameObject.FindObjectOfType<UGameMode>();
        Init();
    }
}
