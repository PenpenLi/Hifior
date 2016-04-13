using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections.Generic;

public abstract class BaseEventData
{
    string EventName;
    public BaseEventData(string name)
    {
        EventName = name;
    }
}
[System.Serializable]
public class CustomEvent : UnityEvent<BaseEventData>
{
}
/// <summary>
/// 负责游戏的玩法主体，没一个关卡有个独立的GameMode，重新载入地图GameMode会初始化
/// 在一个空物体中创建该物体保存为预制体(不用添加到场景里，这个赋给GameInstance即可)，并将以下几个对象选入,必须要指定以下几个对象，如果不指定则报错
/// 不同性质的关卡设置不同的GameMode,如果是主菜单，设置主菜单的GameMode,如果是战斗，设置战斗的GameMode，如果是大地图设置大地图的GameMode，一个关卡一个GameMode
/// </summary>
[HierarchyIcon("GameMode.png")]
public class UGameMode : UActor
{
    public UPawn ActivePawn;
    public UPlayerController ActivePlayerController;
    public UPlayerStatus ActivePlayerStatus;
    public UHUD ActiveHUD;
    public UGameStatus ActiveGameStatus;
    /// <summary>
    /// 当前的玩家数量
    /// </summary>
    int NumPlayers;
    public bool InputModeUI { get; private set; }
    public bool InputModeGame { get; private set; }
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

    public new T GetPlayerPawn<T>() where T : UPawn
    {
        return (T)ActivePawn;
    }
    public new T GetPlayerController<T>() where T : UPlayerController
    {
        return (T)ActivePlayerController;
    }
    public new T GetPlayerState<T>() where T : UPlayerStatus
    {
        return (T)ActivePlayerStatus;
    }
    public new T GetHUD<T>() where T : UHUD
    {
        return (T)ActiveHUD;
    }
    public new T GetGameStatus<T>() where T : UGameStatus
    {
        return (T)ActiveGameStatus;
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

    private UnityEvent Event_GameOver;
    private Dictionary<string, CustomEvent> eventTable = new Dictionary<string, CustomEvent>();
    public void BoardCast(string name, BaseEventData eventData = null)
    {
        if (eventTable[name] != null)
        {
            eventTable[name].Invoke(eventData);
        }
    }
    /*
public class eventdata0 : BaseEventData
{
    public int KK;
    public eventdata0(int value, string name) : base(name)
    {
        KK = value;
    }
}
    eventdata0 d = new eventdata0(12, "fafs");
        AddEventHandler("Game", ss);
        PostNotification("Game", d);
    private void ss(BaseEventData arg0)
    {
        eventdata0 e = arg0 as eventdata0;
        Debug.Log("value = " + e.KK);
    }
    */
    /// <summary>
    /// 添加一个回调函数。
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventHandler(string name, UnityAction<BaseEventData> action)
    {
        if (eventTable.ContainsKey(name))
            eventTable[name].AddListener(action);
        else
        {
            CustomEvent ce = new CustomEvent();
            ce.AddListener(action);
            eventTable.Add(name, ce);
        }
    }
    /// <summary>
    /// 移除了一个回调函数。
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handler"></param>
    public void RemoveEventHandler(string name, UnityAction<BaseEventData> action = null)
    {
        if (eventTable.ContainsKey(name))
            if (action != null)
                eventTable[name].RemoveListener(action);
            else {
                eventTable[name].RemoveAllListeners();
                eventTable.Remove(name);
            }
    }
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
    /// <summary>
    /// 在Awake里进行初始化
    /// </summary>
    public virtual void Initialize() { }
    void Awake()
    {
        Assert.IsNotNull<UPawn>(ActivePawn, "You nees assign a Pawn");
        Assert.IsNotNull<UPlayerController>(ActivePlayerController, "You nees assign a PlayerController");
        Assert.IsNotNull<UPlayerStatus>(ActivePlayerStatus, "You nees assign a PlayerStatus");
        Assert.IsNotNull<UHUD>(ActiveHUD, "You nees assign a HUD");
        Assert.IsNotNull<UGameStatus>(ActiveGameStatus, "You nees assign a GameStatus");
        
        GetGameInstance().SetGameMode(this);
        SetInputModeGameAndUI();

        Initialize();
    }

}
