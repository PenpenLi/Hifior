using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
/// <summary>
/// 负责游戏的玩法主体，没一个关卡有个独立的GameMode，重新载入地图GameMode会初始化
/// 在一个空物体中创建该物体保存为预制体(不用添加到场景里，这个赋给GameInstance即可)，并将以下几个对象选入,必须要指定以下几个对象，如果不指定则报错
/// 不同性质的关卡设置不同的GameMode,如果是主菜单，设置主菜单的GameMode,如果是战斗，设置战斗的GameMode，如果是大地图设置大地图的GameMode，一个关卡一个GameMode
/// </summary>
public class UGameMode : UActor
{
    public UPawn ActivePawn;
    public UPlayerController ActivePlayerController;
    public UPlayerState ActivePlayerState;
    public UHUD ActiveHUD;
    public UGameState ActiveGameState;
    /// <summary>
    /// 当前的玩家数量
    /// </summary>
    int NumPlayers;

    /// <summary>
    /// 当前AI控制的数量
    /// </summary>
    int NumBots;

    /// <summary>
    /// 死亡后最小可以复活的时间
    /// </summary>
    float MinRespawnDelay;

    /// <summary>
    /// 给每个不同的玩家设定状态
    /// </summary>
    int CurrentID;

    public new T GetPawn<T>() where T : UPawn
    {
        return (T)ActivePawn;
    }
    public new T GetPlayerController<T>() where T : UPlayerController
    {
        return (T)ActivePlayerController;
    }
    public new T GetPlayerState<T>() where T : UPlayerState
    {
        return (T)ActivePlayerState;
    }
    public new T GetHUD<T>() where T : UHUD
    {
        return (T)ActiveHUD;
    }
    public new T GetGameState<T>() where T : UGameState
    {
        return (T)ActiveGameState;
    }
    public virtual void RestartGame() { }
    /** 
	 * Sets the name for a controller 
	 * @param Controller	The controller of the player to change the name of
	 * @param NewName		The name to set the player to
	 * @param bNameChange	Whether the name is changing or if this is the first time it has been set
	 */
    public virtual void ChangeName(UPlayerController Controller, string NewName, bool bNameChange) { }

    /* Send a player to a URL.*/
    public virtual void SendPlayer(UPlayerController aPlayer, string URL) { }

    /** Broadcast a string to all players. */
    public virtual void Broadcast(UActor Sender, string Msg, string MsgType = "") { }

    /**
	 * Make sure pawn properties are back to default
	 * Also a good place to modify them on spawn
	 */
    public virtual void SetPlayerDefaults(UPawn PlayerPawn) { }

    void Awake()
    {
        Assert.IsNotNull<UPawn>(ActivePawn, "You nees assign a Pawn");
        Assert.IsNotNull<UPlayerController>(ActivePlayerController, "You nees assign a PlayerController");
        Assert.IsNotNull<UPlayerState>(ActivePlayerState, "You nees assign a PlayerState");
        Assert.IsNotNull<UHUD>(ActiveHUD, "You nees assign a HUD");
        Assert.IsNotNull<UGameState>(ActiveGameState, "You nees assign a GameState");

        ActivePlayerController = Instantiate<UPlayerController>(ActivePlayerController);
        ActivePlayerController.transform.parent = transform;
        ActivePawn = Instantiate<UPawn>(ActivePawn);
        ActivePawn.transform.parent = transform;
        ActivePlayerController.Possess(ActivePawn);
        ActivePlayerState = Instantiate<UPlayerState>(ActivePlayerState);
        ActivePlayerState.transform.parent = transform;
        ActiveHUD = Instantiate<UHUD>(ActiveHUD);
        ActiveHUD.transform.parent = transform;
        ActiveGameState = Instantiate<UGameState>(ActiveGameState);
        ActiveGameState.transform.parent = transform;

        GetGameInstance().SetGameMode(this);
    }
}
