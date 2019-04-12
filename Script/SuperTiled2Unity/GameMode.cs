using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoSingleton<GameMode>
{
    public Camera mainCam;
    public PathShower pathShower;
    public UnitShower unitShower;
    public SLGChapter chapter;
    public GridTileDataControl grid;
    private BattleManager battleManager;
    private InputManager inputManager;
    private UIManager uiManager;
    public BattleManager BattleManager { get { return battleManager; } }
    public InputManager InputManager { get { return inputManager; } }
    public UIManager UIManager { get { return uiManager; } }
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        inputManager = new InputManager();
        battleManager = new BattleManager();
        uiManager = new UIManager();

        BattleManager.ShowMoveRangeAction = ShowMoveRange;

        uiManager.InitBattleUI(GameObject.Find("Panel(9/16)").transform);

        grid.InitInputEvent();
        grid.InitTileTypeData();
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
    }
    #region Bind Action For Battle

    public void ShowMoveRange(CharacterLogic logic)
    {
        Vector2Int tilePos = logic.GetTileCoords();
        PositionMath.InitActionScope(EnumCharacterCamp.Player, EMoveClassType.Savege, 6, tilePos, EnumWeaponType.光明, Vector2Int.one);
        pathShower.ShowTiles(PathShower.EPathShowerType.Move, PositionMath.MoveableAreaPoints);
        pathShower.ShowTiles(PathShower.EPathShowerType.Damage, PositionMath.AttackAreaPoints, true, false);
    }
    #endregion
}
