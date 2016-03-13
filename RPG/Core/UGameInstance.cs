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
public class UGameInstance : MonoSingleton<UGameInstance>
{
    private UGameMode ActiveGameMode;
    private List<UPawn> LocalPlayers = new List<UPawn>();
    private List<UPawn> LocalEnemies = new List<UPawn>();
    private List<UPawn> LocalPlayerAllys = new List<UPawn>();
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

    /**
	 * Adds a new player.
	 * @param ControllerId - The controller ID the player should accept input from.
	 * @param OutError - If no player is returned, OutError will contain a string describing the reason.
	 * @param SpawnActor - True if an actor should be spawned for the new player.
	 * @return The player which was created.
	 */
    public UPawn CreateLocalPlayer(int ControllerId, out string OutError, bool bSpawnActor)
    {
        OutError = null;
        return ActiveGameMode.ActivePawn;
    }

    /**
	 * Adds a LocalPlayer to the local and global list of Players.
	 *
	 * @param	NewPlayer	the player to add
	 * @param	ControllerId id of the controller associated with the player
	 */
    public int AddLocalPlayer(UPawn NewPlayer, int ControllerId)
    {
        return 0;
    }

    /**
	 * Removes a player.
	 * @param Player - The player to remove.
	 * @return whether the player was successfully removed. Removal is not allowed while connected to a server.
	 */
    public bool RemoveLocalPlayer(UPawn ExistingPlayer)
    {
        return true;
    }

    public int GetNumLocalPlayers()
    {
        return LocalPlayers.Count;
    }
    public UPawn GetFirstGamePlayer()
    {
        if (LocalPlayers.Count > 0)
            return LocalPlayers[0];
        Debug.LogError("No Player exists");
        return null;
    }
    public UPawn GetLocalPlayerByIndex(int Index)
    {
        if (LocalPlayers.Count <= Index)
        {
            Debug.LogError("The Index Player don't exists");
            return null;
        }
        if (LocalPlayers[Index] == null)
        {
            Debug.LogError("Null Pawn");
        }
        return LocalPlayers[Index];
    }
    public UPlayerController GetFirstLocalPlayerController()
    {
        return GetFirstGamePlayer().GetPlayerController<UPlayerController>();
    }
    List<UPawn> GetLocalPlayers()
    {
        return LocalPlayers;
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
