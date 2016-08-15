using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using RPG.UI;
/// <summary>
/// 包含战场上需要用到的逻辑，例如胜利失败条件(操作ChapterDef)，各种事件的触发管理（数据从GS_Battle里面拉取）
/// 添加角色到战场，各种逻辑从此处写。
/// </summary>
public class GM_Battle : UGameMode
{
    public EBattleStatus BattleStatus;
    public int Round;
    public EnumCharacterCamp RoundCamp;

    private Transform m_playerParent;
    private Transform m_enemyParent;
    private SLGMap m_slgMap;
    private SLGChapter m_slgChapter;

    public TurnAnim TurnSwitchText;
    public override void Initialize()
    {
        FindSLGMapChapter();
        FindCharacterParent();
    }
    /// <summary>
    /// 开始剧情结束后执行的事件,如果有准备画面进入准备画面，没有则直接进入战场
    /// </summary>
    public void OnStartSequenceFinished()
    {
        if (GetSLGChapter().ChapterSetting.Preparation)
        {
            BattleReadyPanel Ready = UIController.Instance.GetUI<BattleReadyPanel>();
            Ready.RegisterHideEvent(ShowFirstPlayerRound);
            Ready.Show();
        }
        else
        {
            ShowFirstPlayerRound();
        }
    }
    /// <summary>
    /// 开始进入战场
    /// </summary>
    private void ShowFirstPlayerRound()
    {
        Round = 1;
        RPG.UI.TurnAnim TurnAnim = UIController.Instance.GetUI<RPG.UI.TurnAnim>();
        TurnAnim.RegisterHideEvent(OnFirstPhaseStart);
        TurnAnim.Show(Round, RoundCamp);
    }
    /// <summary>
    /// 开始剧情播放完毕，回合动画播放完毕后执行
    /// </summary>
    private void OnFirstPhaseStart()
    {
        GetPlayerPawn<Pawn_BattleArrow>().Reset();
    }
    public SLGMap GetSLGMap()
    {
        if (m_slgMap == null)
        {
            FindSLGMapChapter();
        }
        return m_slgMap;
    }
    public SLGChapter GetSLGChapter()
    {
        if (m_slgChapter == null)
        {
            FindSLGMapChapter();
        }
        return m_slgChapter;
    }
    private void FindSLGMapChapter()
    {
        GameObject Terrain = GameObject.Find("Terrain");
        if (Terrain == null)
        {
            Debug.Log("没有找到Terrain物体");
        }
        m_slgMap = Terrain.GetComponent<SLGMap>();
        m_slgChapter = Terrain.GetComponent<SLGChapter>();
        if (m_slgMap == null)
        {
            Debug.Log("在物体Terrain上没有找到SLGMap组件");
        }
        if (m_slgChapter == null)
        {
            Debug.Log("在物体Terrain上没有找到SLGChapter组件");
        }
    }
    /// <summary>
    /// 在Scene中查找到Player和Enemy的根节点Transform
    /// </summary>
    public void FindCharacterParent()
    {
        m_playerParent = GameObject.Find("InstCharacter/Player").transform;
        m_enemyParent = GameObject.Find("InstCharacter/Enemy").transform;
    }

    private RPGCharacter AddCharacter(CharacterDef Def, EnumCharacterCamp Camp, int x, int y, CharacterAttribute CustomAttribute = null)
    {
        GameObject playerObj = Def.BattleModel;
        Transform instObj = Instantiate(playerObj).transform;
        instObj.rotation = Quaternion.identity;
        if (Camp == EnumCharacterCamp.Player)
            instObj.parent = m_playerParent;
        else
            instObj.parent = m_enemyParent;
        RPGCharacter Character = instObj.GetComponent<RPGCharacter>();
        if (CustomAttribute != null)
        {
            Character.SetAttribute(CustomAttribute);
        }
        else
        {
            Character.SetAttribute(Def.DefaultAttribute);
        }
        Character.SetCamp(Camp);
        Character.SetTileCoord(x, y, true);
        return Character;
    }
    /// <summary>
    /// 添加一个Player到场景中
    /// </summary>
    /// <param name="ID">加载人物ID</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="FromRecord">从存档加载人物属性</param>
    public void AddPlayer(int ID, int x, int y, bool FromRecord = true, CharacterAttribute CustomAttribute = null)
    {
        PlayerDef def = ResourceManager.GetPlayerDef(ID);
        AddPlayer(def, x, y, CustomAttribute);
    }
    /// <summary>
    /// 通过PlayerDef添加Player到场景中
    /// </summary>
    /// <param name="Def"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AddPlayer(PlayerDef Def, int x, int y, CharacterAttribute CustomAttribute = null)
    {
        RPGPlayer Character = AddCharacter(Def, EnumCharacterCamp.Player, x, y, CustomAttribute) as RPGPlayer;
        Character.SetDefaultData(Def);
        GetGameStatus<UGameStatus>().AddLocalPlayer(Character);

        GetGameInstance().AddAvailablePlayer(Def.CommonProperty.ID);//添加到可用角色列表
    }
    /// <summary>
    /// 添加一个Enemy到场景中
    /// </summary>
    /// <param name="ID">加载人物ID</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="FromRecord">从存档加载人物属性</param>
    public void AddEnemy(int ID, int x, int y, List<int> Items, CharacterAttribute CustomAttribute = null)
    {
        EnemyDef def = ResourceManager.GetEnemyDef(ID);
        AddEnemy(def, x, y, Items, CustomAttribute);
    }
    /// <summary>
    /// 通过EnemyDef添加Enemy到场景中
    /// </summary>
    /// <param name="Def"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AddEnemy(EnemyDef Def, int x, int y, List<int> Items, CharacterAttribute CustomAttribute = null)
    {
        RPGEnemy Character = AddCharacter(Def, EnumCharacterCamp.Enemy, x, y, CustomAttribute) as RPGEnemy;
        Character.Item.AddWeapons(Items);
        Character.SetDefaultData(Def);
        GetGameStatus<UGameStatus>().AddLocalEnemy(Character);
    }

    #region 回合结束处理

    private UnityAction OnRoundAnimationFinished;
    /// <summary>
    /// 主菜单结束回合按钮
    /// </summary>
    public void EndRound(UnityAction OnRoundAnimationFinish)
    {
        OnRoundAnimationFinished = OnRoundAnimationFinish;
        if (RoundCamp == EnumCharacterCamp.Player)
        {
            Debug.Log("Player  结束行动");
            RoundCamp = EnumCharacterCamp.Enemy;
            GetGameStatus<GS_Battle>().EnableAllPlayerControl();
        }
        else
        {
            Debug.Log("Enemy  结束行动");
            RoundCamp = EnumCharacterCamp.Player;
            Round++;
        }
        //如果获得指定回合到了，胜利
        if (GetSLGChapter().CheckWin_Round(Round))
        {
            ClearTheStage();
        }
        //否则执行回合事件
        else
        {
            CheckTurnEvent();
        }
    }
    private void CheckTurnEvent()
    {
        SLGChapter.TurnEventType Event = m_slgChapter.GetTurnEvent(Round, RoundCamp);
        if (Event != null)
        {
            Event.Execute(ShowRoundAnimation);
        }
        else
        {
            ShowRoundAnimation();
        }
    }
    private void ShowRoundAnimation()
    {
        RPG.UI.TurnAnim turn = UIController.Instance.GetUI<RPG.UI.TurnAnim>();
        turn.RegisterHideEvent(OnRoundAnimationFinished);
        turn.Show(Round, RoundCamp);
    }
    /// <summary>
    /// 过关状态，显示过关动画
    /// </summary>
    public void ClearTheStage()
    {
        ClearStagePanel clear = UIController.Instance.GetUI<RPG.UI.ClearStagePanel>();
        clear.RegisterHideEvent(WinTheChapter);
        clear.Show();
    }
    /// <summary>
    /// 本章过关，执行结束事件，然后去下一关
    /// </summary>
    public void WinTheChapter()
    {
        Debug.Log("过关");
        GetSLGChapter().EndSequence.Execute(EndThisChapter);
    }
    /// <summary>
    /// 结束事件执行完毕后执行，结束本章，进入存档场景
    /// </summary>
    private void EndThisChapter()
    {
        Debug.Log("进入下一关");
        GetGameInstance().SaveChapter(false);
        LoadingScreenManager.LoadScene(UGameInstance.SCENEINDEX_CHAPTER_ENDSAVE);
    }
    #endregion
    public override void BeginPlay()
    {
        base.BeginPlay();

        //GetGameInstance().ChapterID = GetSLGChapter().ChapterSetting.CommonProperty.ID;
    }
}
