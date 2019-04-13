using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMode : MonoSingleton<GameMode>
{
    public Camera mainCam;
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
    // Start is called before the first frame update
    protected override void Init()
    {
        inputManager = new InputManager();
        battleManager = new BattleManager();
        uiManager = new UIManager();
        gridTileManager = new GridTileManager();
        chapterManager = new ChapterManager();
        BindKeyInput();

        battleManager.ShowMoveRangeAction = ShowMoveRange;
        battleManager.ShowHighlightRangeAction = pathShower.ShowHighLightTiles;
        battleManager.IsRangeVisible = pathShower.IsRangeVisible;
        battleManager.ClearRangeAction = pathShower.HideAll;

        uiManager.InitBattleUI(GameObject.Find("Panel(9/16)").transform);

        gridTileManager.LoadNewMap(5);
        gridTileManager.InitMouseInputEvent();
    }
    void Start()
    {
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        BattleManager.Update();
        InputManager.Update();
        UIManager.Update();
    }

    private void BindKeyInput()
    {
        InputManager.GetNoInput = () =>
        {
            return Input.GetKeyUp(KeyCode.Escape);
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
        Vector2Int tilePos = logic.GetTileCoord();
        PositionMath.InitActionScope(EnumCharacterCamp.Player, EMoveClassType.Savege, 6, tilePos, EnumWeaponType.光明, Vector2Int.one);
        pathShower.ShowTiles(PathShower.EPathShowerType.Move, PositionMath.MoveableAreaPoints);
        pathShower.ShowTiles(PathShower.EPathShowerType.Damage, PositionMath.AttackAreaPoints, true, false);
    }

    #endregion
    #region UnitShower
    public void AddUnitToMap(RPGCharacter p, Vector2Int tilePos)
    {
        Transform unit = unitShower.AddUnit(p.GetCamp(), p.GetCharacterName(), p.GetStaySprites(), p.GetMoveSprites(), tilePos);
        p.SetTransform(unit);
        p.Logic().SetTileCoord(tilePos);
        chapterManager.AddPlayerToBattle(p);
    }
    public void MoveUnit(Vector2Int unitPos, Vector2Int destPos, UnityAction onFinish)
    {
        List<Vector2Int> routine = PositionMath.GetMoveRoutine(destPos);
        unitShower.MoveUnit(routine, onFinish);
    }
    #endregion
}
