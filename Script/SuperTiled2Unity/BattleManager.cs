using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : ManagerBase
{

    public enum EBattleState
    {
        Idel,
        Menu,
        SelectMove,
        SelectTarget,
        Lock,
    }
    /// <summary>
    /// 显示攻击范围
    /// </summary>
    public UnityAction<CharacterLogic> ShowMoveRangeAction;
    public UnityAction<List<Vector2Int>> ShowHighlightRangeAction;
    public UnityAction ClearRangeAction;
    public System.Func<bool> IsRangeVisible;

    public bool IsDesktopNo()
    {
        if (inputManager.GetNoInput()) return true;
        var vMouseInputState = inputManager.GetMouseInput();
        return vMouseInputState.active && vMouseInputState.key == InputManager.EMouseKey.Right;
    }

    CharacterLogic currentCharacterLogic;

    EBattleState battleState = EBattleState.Idel;

    public override void Update()
    {
        base.Update();
        HandleInput();
        UpdateScene();
    }

    public CharacterLogic GetCharacterLogic(Vector2Int tilePos)
    {
        var ch = chapterManager.GetCharacterFromCoord(tilePos);
        if (ch == null) return null;
        return ch.Logic();
    }
    public ETileType GetTileType(Vector2Int tilePos)
    {
        return gameMode.GridTileManager.GetTileType(tilePos);
    }
    public void UpdateTileInfo(Vector2Int tilePos)
    {
        uiManager.BattleTileInfo.Show(GetTileType(tilePos));
    }

    public void ChangeState(EBattleState state)
    {
        battleState = state;
    }
    void HandleInput()
    {
        switch (battleState)
        {
            case EBattleState.Idel:
                HandleIdel();
                break;
            case EBattleState.Lock:
                HandleLock();
                break;
            case EBattleState.Menu:
                HandleMenu();
                break;
            case EBattleState.SelectMove:
                HandleSelectMove();
                break;
            case EBattleState.SelectTarget:
                HandleSelectTarget();
                break;
            default:
                break;
        }
    }
    public void HandleIdel()
    {
        var vMouseInputState = inputManager.GetMouseInput();
        var tilePos = vMouseInputState.tilePos;
        UpdateTileInfo(tilePos);
        if (vMouseInputState.IsClickedTile())
        {
            if (vMouseInputState.key == InputManager.EMouseKey.Left)
            {
                currentCharacterLogic = GetCharacterLogic(tilePos);
                if (currentCharacterLogic != null)
                {
                    SelectTarget();
                }
            }
        }
    }

    public void HandleMenu()
    {
        var vMouseInputState = inputManager.GetMouseInput();
        if (IsDesktopNo())
        {
            CloseMenu();
        }
    }
    public void HandleSelectMove()
    {
        if (IsDesktopNo())
        {
            ClearRangeAction();
        }
        var vMouseInputState = inputManager.GetMouseInput();
        if (PositionMath.MoveableAreaPoints.Contains(vMouseInputState.tilePos))
        {
            ShowHighlightRangeAction(new List<Vector2Int> { vMouseInputState.tilePos });
        }
        if (inputManager.GetNoInput())
        {
            CancelMove();
        }
        if (vMouseInputState.IsClickedTile())
        {
            if (PositionMath.MoveableAreaPoints.Contains(vMouseInputState.tilePos))
            {
                if (chapterManager.HasCharacterFromCoord(vMouseInputState.tilePos) == false)
                    ExecuteMove(vMouseInputState.tilePos);
            }
            else
            {
                CancelMove();
            }
        }
    }
    public void HandleSelectTarget()
    {
        if (IsDesktopNo())
        {

        }
    }

    public void HandleLock()
    {

    }
    /// <summary>
    /// 选择目标
    /// </summary>
    /// <param name="pos"></param>
    public void SelectTarget()
    {
        if (currentCharacterLogic == null) return;
        if (ShowMoveRangeAction == null) Debug.LogError("ShowMoveRangeAction is not binded");
        else
        {
            if (currentCharacterLogic.Controllable)
            {
                OpenMenu();
            }
            ShowMoveRangeAction(currentCharacterLogic);
        }
    }
    public void ExecuteMove(Vector2Int destPos)
    {
        Vector2Int srcPos = currentCharacterLogic.GetTileCoord();
        currentCharacterLogic.SetTileCoord(destPos);
        ChangeState(EBattleState.Lock);
        ClearRangeAction();
        gameMode.MoveUnit(srcPos, destPos,OpenMenu);
    }
    public void CancelMove()
    {
        OpenMenu();
    }
    public void ExecuteAttack()
    {

    }
    public void ExecuteSkill()
    {

    }
    public void FinishAction()
    {

    }
    public void CloseMenu()
    {

    }
    public void OpenMenu()
    {
        ChangeState(EBattleState.Menu);
        uiManager.ShowBattleActionMenu(UIManager.EActionMenuState.Main, currentCharacterLogic);
    }
    public void UpdateScene()
    {

    }
}
