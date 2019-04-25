using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class GameMode : MonoSingleton<GameMode>
{
    public SLGCamera slgCamera;
    public Transform MainCameraTransform { get { return slgCamera.transform; } }
    public PathShower pathShower;
    public UnitShower unitShower;
    private ChapterManager chapterManager;
    private BattleManager battleManager;
    private InputManager inputManager;
    private UIManager uiManager;
    private GridTileManager gridTileManager;
    public BattleManager BattleManager { get { return battleManager; } }
    public InputManager InputManager { get { return inputManager; } }
    public UIManager UIManager { get { return uiManager; } }
    public GridTileManager GridTileManager { get { return gridTileManager; } }
    public ChapterManager ChapterManager { get { return chapterManager; } }
    #region GameMode Info
    public struct GameModeInfo
    {
        public enum ModeState
        {
            MainMenu,
            BigMap,
            House,
            Battle
        }
        public ModeState modeState;

        public bool lockInput;
        /// <summary>
        /// 是否在战斗中
        /// </summary>
        public bool HasStartBattle() { return modeState == ModeState.Battle; }
    }
    private GameModeInfo modeInfo;
    #endregion

    // Start is called before the first frame update
    protected override void Init()
    {
        inputManager = new InputManager();
        battleManager = new BattleManager();
        uiManager = new UIManager();
        gridTileManager = new GridTileManager();
        chapterManager = new ChapterManager();

        BindKeyInput();
        uiManager.InitBattleUI(GameObject.Find("Panel(0/9)").transform, GameObject.Find("Panel(9/16)").transform, GameObject.Find("Panel(0/16)").transform);

        battleManager.ShowMoveRangeAction = ShowMoveRange;
        battleManager.ShowSelectTargetRangeAction = ShowSelectTargetRange;
        battleManager.ShowEffectTargetRangeAction = ShowEffectTargetRange;

        battleManager.ShowHighlightRangeAction = pathShower.ShowHighLightTiles;
        battleManager.ShowTalkCharacterRangeAction = pathShower.ShowTalkCharacterTiles;
        battleManager.ClearHighlightRangeAction = () => pathShower.HidePath(PathShower.EPathShowerType.HighLight);
        battleManager.IsRangeVisible = pathShower.IsRangeVisible;
        battleManager.ClearRangeAction = pathShower.HideAll;
        battleManager.UpdateSelectTileInfo = uiManager.UpdateTileInfo;
        battleManager.UpdateSelectCharacterInfo = uiManager.UpdateCharacterInfo;
        battleManager.Init();
        gridTileManager.InitMouseInputEvent();

        chapterManager.OnShowTurnIndicate +=uiManager.TurnIndicate.Show;

        LogInitInfo();
        TestFunctionAddHere();
    }

    void LogInitInfo()
    {
        Debug.Log("存档位置：" + GameRecord.RootDataPath);
    }
    void TestFunctionAddHere()
    {
        chapterManager.NewGameData(1);
        // StartCoroutine(TestLoadBattleFromSave());
    }
    IEnumerator TestLoadBattleFromSave()
    {
        LoadBattle();
        yield return new WaitForEndOfFrame();
    }
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (modeInfo.lockInput) return;
        InputManager.Update();
        UIManager.Update();
        if (modeInfo.HasStartBattle())
        {
            BattleManager.Update();
        }
    }

    private void BindKeyInput()
    {
        InputManager.GetNoInput = () =>
        {
            return Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1);
        };

        InputManager.GetYesInput = () =>
        {
            return Input.GetKeyUp(KeyCode.Space);
        };
        InputManager.GetStartInput = () =>
        {
            return Input.GetKeyUp(KeyCode.Space);
        };
        InputManager.GetArrowInput = () =>
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) return InputManager.EKeyArrow.Up;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) return InputManager.EKeyArrow.Left;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) return InputManager.EKeyArrow.Right;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) return InputManager.EKeyArrow.Down;
            return InputManager.EKeyArrow.None;
        };
    }
    #region Bind Action For Battle

    public void ShowMoveRange(CharacterLogic logic)
    {
        PositionMath.InitActionScope(logic.Info.Camp, logic.GetMoveClass(), logic.GetMovement(), logic.GetTileCoord(), Vector2Int.one);
        pathShower.ShowTiles(PathShower.EPathShowerType.Move, PositionMath.MoveableAreaPoints);
        pathShower.ShowTiles(PathShower.EPathShowerType.Damage, PositionMath.AttackAreaPoints, true, false);
    }

    public void ShowSelectTargetRange(CharacterLogic logic)
    {
        Vector2Int tilePos = logic.GetTileCoord();
        List<Vector2Int> atkRange = logic.BattleInfo.TargetChooseRanges;
        pathShower.ShowTiles(PathShower.EPathShowerType.Damage, atkRange);
    }

    public void ShowEffectTargetRange(CharacterLogic logic, Vector2Int mouseTilePos)
    {
        Vector2Int tilePos = logic.GetTileCoord();
        //List<Vector2Int> highlightRange = new List<Vector2Int> { mouseTilePos };
        logic.BattleInfo.SetEffectTarget(mouseTilePos);
        List<Vector2Int> highlightRange = logic.BattleInfo.TargetEffectRanges;
        pathShower.ShowHighLightTiles(highlightRange);
    }

    #endregion
    #region UnitShower

    public ETileType GetTileType(Vector2Int tilePos)
    {
        return gridTileManager.GetTileType(tilePos);
    }
    public void AddUnitToMap(RPGCharacter p, Vector2Int tilePos)
    {
        var logic = p.Logic;
        Transform unit = unitShower.AddUnit(p.GetCamp(), logic.GetName(), logic.GetStaySprites(), logic.GetMoveSprites(), tilePos);
        p.SetTransform(unit);
        logic.SetTileCoord(tilePos);
        var camp = p.GetCamp();
        if (camp == EnumCharacterCamp.Player)
            chapterManager.AddPlayerToBattle(p);
        if (camp == EnumCharacterCamp.Enemy)
            chapterManager.AddEnemyToBattle(p);
    }
    public void MoveUnitAfterAction(Vector2Int unitPos, Vector2Int destPos, float speed, UnityAction onComplete)
    {
        List<Vector2Int> routine = PositionMath.GetMoveRoutine(destPos);
        unitShower.MoveUnit(routine, onComplete, speed);
    }
    public void MoveUnitByRoutine(List<Vector2Int> routine, float speed, UnityAction onComplete)
    {
        unitShower.MoveUnit(routine, onComplete, speed);
    }

    public void KillUnitAt(Vector2Int tilePos, float v, UnityAction onComplete)
    {
        unitShower.DisappearUnit(tilePos, v, onComplete);
    }
    public void KillUnit(int Id, float v, UnityAction onComplete)
    {
        var ch = chapterManager.GetCharacterFromID(Id);
        chapterManager.RemoveCharacter(ch);
        unitShower.DisappearUnit(ch.GetTileCoord(), v, onComplete);
    }
    public void AttackUnit(CharacterLogic attacker,CharacterLogic defender)
    {
        var atkPos = attacker.GetTileCoord();
        var defPos = defender.GetTileCoord();
       //计算处方向 然后在Unitshower里面转向并攻击，抖动
    }
    #endregion
    #region Battle Manager
    /// <summary>
    /// 加载章节数据，先加载存档中的数据，SLGChapter预制体，地图在StartEvent中载入并显示
    /// </summary>
    /// <param name="chapterID">章节ID</param>
    public void LoadChapter(int chapterID)
    {
        chapterManager.LoadChapterData(chapterID);
    }
    public void LoadBattle()
    {
        ChapterManager.LoadBattleData(-1);
        //载入数据后，将各种战斗组件开启
    }
    public void LoadTileMap(int mapId)
    {
        gridTileManager.LoadNewMap(mapId);
        chapterManager.InitMapEvent(mapId);
    }
    public void LockInput(bool bLock)
    {
        modeInfo.lockInput = bLock;
    }
    public void FreeBattleCamera()
    {
        slgCamera.SetControlMode(CameraControlMode.FreeMove);
    }
    /// <summary>
    /// 开始战斗
    /// </summary>
    /// <param name="FirstActionCamp">第一个回合行动的阵营</param>
    public void StartBattle(EnumCharacterCamp FirstActionCamp)
    {
        modeInfo.modeState = GameModeInfo.ModeState.Battle;
        chapterManager.NextTurn();
    }
    #endregion
    #region Camera
    public void CameraMoveTo(Vector2Int tilePos, UnityAction onComplete, float moveTime = 0.0f, bool accelerate = false)
    {
        if (moveTime < 0.01f)
        {
            PositionMath.SetCameraFocusPosition(MainCameraTransform, tilePos);
        }
        else
        {
            var targetPosition = PositionMath.CameraTilePositionFocusOnLocalPosition(tilePos);
            Tweener tw = MainCameraTransform.DOLocalMove(targetPosition, moveTime);
            if (onComplete != null)
                tw.OnComplete(() => onComplete());
        }
    }
    #endregion
    #region Sequence
    public void BeforePlaySequence()
    {
        pathShower.SetRootVisible(false);
        LockInput(true);
    }
    public void AfterPlaySequence()
    {
        pathShower.SetRootVisible(true);
        LockInput(false);
    }
    #endregion
}
