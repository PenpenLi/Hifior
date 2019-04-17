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
    public UnityAction<CharacterLogic> ShowChooseTargetRangeAction;
    public UnityAction<CharacterLogic, Vector2Int> ShowEffectTargetRangeAction;
    public UnityAction<List<Vector2Int>> ShowHighlightRangeAction;
    public UnityAction HideHighlightRangeAction;
    public UnityAction ClearRangeAction;
    public UnityAction<Vector2Int> UpdateSelectTileInfo;
    public UnityAction<CharacterLogic> UpdateSelectCharacterInfo;
    public System.Func<bool> IsRangeVisible;

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
        return ch.Logic;
    }

    public void ChangeState(EBattleState state)
    {
        if (state == EBattleState.SelectTarget)
        {
            //将需要执行的命令先写入到 CharacterLogic中
            ShowChooseTargetRangeAction(currentCharacterLogic);
        }
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
        UpdateSelectTileInfo(tilePos);
        if (vMouseInputState.IsClickedTile())
        {
            currentCharacterLogic = GetCharacterLogic(tilePos);
            if (currentCharacterLogic == null)
            {
            }
            else
            {
                SelectTarget();
            }
        }
    }

    public void HandleMenu()
    {
        if (inputManager.GetNoInput() && uiManager.MenuUndoAction != null)
        {
            uiManager.MenuUndoAction();
            return;
        }
        var vMouseInputState = inputManager.GetMouseInput();
        if (uiManager.ActionMenuState == EActionMenuState.Main)
        {
            if (vMouseInputState.IsClickedTile())
            {
                currentCharacterLogic = GetCharacterLogic(vMouseInputState.tilePos);
                if (currentCharacterLogic == null)
                {
                    ClearRangeAction();
                    CloseMenu();
                }
                else
                {
                    SelectTarget();
                }
            }
        }
    }
    public void HandleSelectMove()
    {
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
                WarningCannotMove();
            }
        }
    }
    public void HandleSelectTarget()
    {
        if (inputManager.GetNoInput())
        {
            uiManager.MenuUndoAction();
        }
        var vMouseInputState = inputManager.GetMouseInput();
        if (PositionMath.AttackAreaPoints.Contains(vMouseInputState.tilePos))
            ShowEffectTargetRangeAction(currentCharacterLogic, vMouseInputState.tilePos);
        else
            HideHighlightRangeAction();
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
                OpenMenu(EActionMenuState.Main, UndoCancelMainActionAndClearRange);
            }
            ShowMoveRangeAction(currentCharacterLogic);
        }
        UpdateSelectCharacterInfo(currentCharacterLogic);
    }
    public void ExecuteMove(Vector2Int destPos)
    {
        Vector2Int srcPos = currentCharacterLogic.GetTileCoord();
        currentCharacterLogic.SetTileCoord(destPos);
        ChangeState(EBattleState.Lock);
        ClearRangeAction();
        gameMode.MoveUnit(srcPos, destPos, ConstTable.UNIT_MOVE_SPEED(), FinishMoveEvent);
    }
    public void FinishMoveEvent()
    {
        OpenMenu(EActionMenuState.AfterMove, UndoCancelSelectTargetActionAndClearRange);
    }
    public void CancelMove()
    {
        ShowMoveRangeAction(currentCharacterLogic);
        OpenMenu(EActionMenuState.Main, UndoCancelMainActionAndClearRange);
    }
    public void WarningCannotMove()
    {

    }
    public void ExecuteAttack()
    {

    }
    public void ExecuteSkill()
    {

    }
    public void FinishAction()
    {
        chapterManager.SaveChapterData(0);
        chapterManager.SaveBattleData();
        battleManager.ChangeState(EBattleState.Idel);
        currentCharacterLogic.EndAction();
    }
    public void CloseMenu()
    {
        uiManager.HideBattlaActionMenu();
    }

    public void OpenMenu(EActionMenuState menuState, UnityAction undoAction = null)
    {
        ChangeState(EBattleState.Menu);
        uiManager.MenuUndoAction = undoAction;
        uiManager.ShowBattleActionMenu(menuState, currentCharacterLogic);
    }
    #region Undo Action

    public void UndoCancelMainActionAndClearRange()
    {
        ChangeState(EBattleState.Idel);
        ClearRangeAction();
        CloseMenu();
    }
    public void UndoCancelSelectTargetActionAndClearRange()
    {
        ClearRangeAction();
        OpenMenu(EActionMenuState.AfterMove, UndoCancelSelectTargetActionAndClearRange);
    }
    #endregion

    public void UpdateScene()
    {

    }
}
