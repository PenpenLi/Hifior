using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
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
    public new T GetPawn<T>() where T : UPawn
    {
        return ActiveGameMode.GetPawn<T>();
    }
    public new T GetPlayerController<T>() where T : UPlayerController
    {
        return ActiveGameMode.GetPlayerController<T>();
    }
    public new T GetPlayerState<T>() where T : UPlayerState
    {
        return ActiveGameMode.GetPlayerState<T>();
    }
    public new T GetGameState<T>() where T : UGameState
    {
        return ActiveGameMode.GetGameState<T>();
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

    public override void Awake()
    {
        base.Awake();

        ActiveGameMode = GameObject.FindObjectOfType<UGameMode>();
        Init();
    }
}
