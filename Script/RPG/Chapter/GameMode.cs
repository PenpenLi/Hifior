using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

public class GameMode : MonoSingleton<GameMode>
{
    public SLGCamera slgCamera;
    public Transform MainCameraTransform { get { return slgCamera.transform; } }
    public PathShower pathShower;
    public UnitShower unitShower;
    public MapIndicator mapIndicator;
    private ChapterManager chapterManager;
    private BattleManager battleManager;
    private InputManager inputManager;
    private UIManager uiManager;
    private GridTileManager gridTileManager;
    private BattlePlayer battlePlayer;
    public BattleManager BattleManager { get { return battleManager; } }
    public InputManager InputManager { get { return inputManager; } }
    public UIManager UIManager { get { return uiManager; } }
    public GridTileManager GridTileManager { get { return gridTileManager; } }
    public ChapterManager ChapterManager { get { return chapterManager; } }
    public BattlePlayer BattlePlayer { get { return battlePlayer; } }
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
        Application.targetFrameRate = 60;
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

        battlePlayer = new BattlePlayer();

        gridTileManager.InitMouseInputEvent();

        chapterManager.OnShowTurnIndicate += uiManager.TurnIndicate.Show;

        LogInitInfo();
        TestFunctionAddHere();
    }
    public IEnumerator Start()
    {
        yield return null;
        //淡出从白到正常显示主菜单页面
        {
            uiManager.ScreenWhiteToNormal(2.0f);
            uiManager.GameStartMenu.ShowAtStart = true;
            uiManager.GameStartMenu.Init();
            uiManager.GameStartMenu.Show();
        }
    }
    #region Test and Log
    void LogInitInfo()
    {
        Debug.Log("存档位置：" + GameRecord.RootDataPath);
    }
    void TestFunctionAddHere()
    {
        //chapterManager.NewGameData(1);
        //StartCoroutine(TestLoadBattleFromSave());
    }

    IEnumerator TestLoadBattleFromSave()
    {
        yield return null;
        chapterManager.Event.StartSequence.Execute();
        //LoadBattle();
        yield return new WaitForEndOfFrame();
        //ResetSequence("fade");
        //var fade = AddSequenceEvent<Sequence.LoadTileMap>();
        //fade.MapId = 5;

        //var fade1 = AddSequenceEvent<Sequence.FadeScreen>();
        //fade1.duration = 1.0f;
        //fade1.waitUntilFinished = true;
        //fade1.FadeType = Sequence.FadeScreen.渐变类型.黑变正常;
        //PlaySequence(() => Debug.LogError("Finish"));
    }
    #endregion
    private void LoadGameSequence(UnityAction action)
    {
        ResetSequence("Load Game");
        var fade1 = AddSequenceEvent<Sequence.FadeScreen>();
        fade1.duration = 1.0f;
        fade1.waitUntilFinished = true;
        fade1.FadeType = Sequence.FadeScreen.渐变类型.正常变黑;
        var fade2 = AddSequenceEvent<Sequence.FadeScreen>();
        fade2.duration = 1.0f;
        fade2.waitUntilFinished = false;
        fade2.FadeType = Sequence.FadeScreen.渐变类型.黑变正常;
        PlaySequence(action);
    }
    public void NewGame(int slot)
    {
        chapterManager.NewGameData(slot);
    }

    public void SaveGame(int slot)
    {
        chapterManager.SaveChapterData(slot);
        //继续下一章
    }
    public void LoadGame(int slot)
    {
        chapterManager.LoadChapterData(slot);
    }
    public void DeleteGameRecord(int slot)
    {
        chapterManager.DeleteChapterData(slot);
    }
    public void LoadNextChapter()
    {
        chapterManager.LoadChapterDef(chapterManager.ChapterId + 1);
    }
    public void PlayStartSequence()
    {
        uiManager.ChapterStartPreface.RegisterHideEvent(() =>
        {
            uiManager.HideAfterRecord();
            chapterManager.Event.StartSequence.Execute();
        });
        uiManager.ChapterStartPreface.Show(chapterManager.ChapterDef.CommonProperty.Name);
    }
    // Update is called once per frame
    void Update()
    {
        if (modeInfo.lockInput) return;
        inputManager.Update();
        uiManager.Update();
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
        PositionMath.InitActionScope(logic.Info.Camp, logic.GetMoveClass(), logic.GetMovement(), logic.GetTileCoord(), logic.GetSelectRangeType(), logic.GetSelectRange());
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

    public ETileType GetTileType(Vector2Int tilePos)
    {
        return gridTileManager.GetTileType(tilePos);
    }

    public void CheckDefeatBossWin()
    {
        uiManager.HideAttackInfo();
        if (chapterManager.CheckWin_DefeatBoss(0))
        {
            ClearStage();
            return;
        }
        if (chapterManager.CheckWin_KillAllEnemy() && chapterManager.EnemyCount == 0)
        {
            ClearStage();
            return;
        }
        chapterManager.CheckEnemyLessEvent(chapterManager.EnemyCount, () =>
         {
             AfterPlaySequence();
             battleManager.OpenMenu(EActionMenuState.Main);
         });
    }
    /// <summary>
    /// 显示通关，并开始播放通关剧情
    /// </summary>
    public void ClearStage()
    {
        uiManager.TurnIndicate.ShowWinText(() => chapterManager.Event.EndSequence.Execute());
    }
    #region Battle Manager

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
        uiManager.ShowBattleHPBar(true);
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
    public Sequence.Sequence PublicSequencer;
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
    public void StopPlaySequence()
    {
        PublicSequencer.Stop();
        AfterPlaySequence();
    }
    public void ResetSequence(string sequenceName, bool skipable = false)
    {
        PublicSequencer.Clear();
        PublicSequencer.SequenceName = sequenceName;
        PublicSequencer.Skipable = skipable;
    }

    public T AddSequenceEvent<T>() where T : Sequence.SequenceEvent
    {
        GameObject g = new GameObject(typeof(T).Name);
        g.transform.SetParent(PublicSequencer.transform, false);
        return g.AddComponent<T>();
    }
    public void AddSequenceEvents(List<Sequence.SequenceEvent> l)
    {
        foreach (var v in l)
        {
            GameObject g =v.gameObject;
            g.name = v.GetType().Name;
            g.transform.SetParent(PublicSequencer.transform, false);
        }
    }
    public T AddSequenceEvent<T>(string _name) where T : Sequence.SequenceEvent
    {
        GameObject g = new GameObject(_name);
        g.transform.SetParent(PublicSequencer.transform, false);
        return g.AddComponent<T>();
    }
    public void PlaySequence(UnityAction onComplete)
    {
        PublicSequencer.Reset();
        PublicSequencer.Execute(onComplete);
    }
    #endregion
}
