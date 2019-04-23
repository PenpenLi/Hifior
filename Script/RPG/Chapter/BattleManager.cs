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
    public UnityAction<CharacterLogic> ShowSelectTargetRangeAction;
    public UnityAction<CharacterLogic, Vector2Int> ShowEffectTargetRangeAction;
    public UnityAction<List<Vector2Int>> ShowHighlightRangeAction;
    public UnityAction ClearHighlightRangeAction;
    public UnityAction ClearRangeAction;
    public UnityAction<Vector2Int> UpdateSelectTileInfo;
    public UnityAction<CharacterLogic> UpdateSelectCharacterInfo;
    public System.Func<bool> IsRangeVisible;

    private RPGCharacter currentCharacter;
    public CharacterLogic CurrentCharacterLogic { get { if (currentCharacter == null) return null; return currentCharacter.Logic; } }
    private RPGCharacter selectTargetCharacter;
    private List<RPGCharacter> selectEffectCharacter;

    EBattleState battleState = EBattleState.Idel;

    /// <summary>
    /// 
    /// </summary>
    private bool bClearRangeFlag = false;
    public override void Init()
    {
        base.Init();
        ClearRangeAction += () => { bClearRangeFlag = true; };
    }
    public override void Update()
    {
        base.Update();
        HandleInput();
        UpdateScene();
    }
    public RPGCharacter GetCharacter(Vector2Int tilePos)
    {
        var ch = chapterManager.GetCharacterFromCoord(tilePos);
        return ch;
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
        gameMode.slgCamera.SetControlMode(CameraControlMode.FreeMove);
        if (inputManager.GetNoInput())
        {
            ClearRangeAction();
            return;
        }
        var vMouseInputState = inputManager.GetMouseInput();
        var tilePos = vMouseInputState.tilePos;
        bool dirty = vMouseInputState.IsMouseTilePosChanged();
        if (dirty)
            UpdateSelectTileInfo(tilePos);
        if (vMouseInputState.IsClickedTile())
        {
            currentCharacter = GetCharacter(tilePos);
            if (CurrentCharacterLogic == null)
            {
                ClearRangeAction();
            }
            else
            {
                if (bClearRangeFlag)
                    SelectMoveTarget();
                else
                    ClearRangeAction();
            }
        }
    }

    public void HandleMenu()
    {
        if (inputManager.GetNoInput() && uiManager.MenuUndoAction != null)
        {
            if (uiManager.MenuUndoAction.Count == 0) Debug.LogWarning("Undo operation is empty");
            else uiManager.MenuUndoAction.Pop().Invoke();
            return;
        }
        var vMouseInputState = inputManager.GetMouseInput();
        if (uiManager.ActionMenuState == EActionMenuState.Main)
        {
            if (vMouseInputState.IsClickedTile())
            {
                currentCharacter = GetCharacter(vMouseInputState.tilePos);
                if (CurrentCharacterLogic == null)
                {
                    ClearRangeAction();
                    CloseMenu();
                }
                else
                {
                    SelectMoveTarget();
                }
            }
        }
    }
    public void HandleSelectMove()
    {
        gameMode.slgCamera.SetControlMode(CameraControlMode.FreeMove);
        var vMouseInputState = inputManager.GetMouseInput();
        bool dirty = vMouseInputState.IsMouseTilePosChanged();
        if (dirty)
        {
            if (PositionMath.MoveableAreaPoints.Contains(vMouseInputState.tilePos))
            {
                ShowHighlightRangeAction(new List<Vector2Int> { vMouseInputState.tilePos });
            }
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
        gameMode.slgCamera.SetControlMode(CameraControlMode.FreeMove);
        if (inputManager.GetNoInput())
        {
            uiManager.MenuUndoAction.Pop().Invoke();
        }
        var vMouseInputState = inputManager.GetMouseInput();
        bool dirty = vMouseInputState.IsMouseTilePosChanged();
        var targetSelectRange = CurrentCharacterLogic.BattleInfo.TargetChooseRanges;
        if (dirty)
        {
            if (targetSelectRange.Contains(vMouseInputState.tilePos))
            {
                ShowEffectTargetRangeAction(CurrentCharacterLogic, vMouseInputState.tilePos);
            }
            else
                ClearHighlightRangeAction();
        }
        if (vMouseInputState.IsClickedTile())
        {
            if (targetSelectRange.Contains(vMouseInputState.tilePos))
            {
                ShowEffectTargetRangeAction(CurrentCharacterLogic, vMouseInputState.tilePos);
                var targetEffectRange = CurrentCharacterLogic.BattleInfo.TargetEffectRanges;
                var effectCamps = CurrentCharacterLogic.BattleInfo.GetEffectCamps();
                switch (CurrentCharacterLogic.BattleInfo.BattleActionType)
                {
                    case CharacterBattleInfo.EBattleActionType.Attack:
                        {
                            if (chapterManager.HasCharacterFromCoord(targetEffectRange, effectCamps))
                                ExecuteAttack();
                            break;
                        }
                    case CharacterBattleInfo.EBattleActionType.Skill:
                        {
                            if (chapterManager.HasCharacterFromCoord(targetEffectRange, effectCamps))
                                ExecuteAttack();
                            break;
                        }
                    case CharacterBattleInfo.EBattleActionType.Heal:
                        {
                            if (chapterManager.HasCharacterFromCoord(targetEffectRange, effectCamps))
                                ExecuteAttack();
                            break;
                        }
                    case CharacterBattleInfo.EBattleActionType.Stole:
                        {
                            if (chapterManager.HasCharacterFromCoord(targetEffectRange, effectCamps))
                                ExecuteAttack();
                            break;
                        }
                }
            }
            else
            {
                WarningCannotMove();
            }
        }
    }

    public void HandleLock()
    {

    }
    /// <summary>
    /// 选择目标
    /// </summary>
    /// <param name="pos"></param>
    public void SelectMoveTarget()
    {
        if (CurrentCharacterLogic == null) return;
        if (ShowMoveRangeAction == null) Debug.LogError("ShowMoveRangeAction is not binded");
        else
        {
            if (CurrentCharacterLogic.Controllable)
            {
                OpenMenu(EActionMenuState.Main, UndoCancelMainActionAndClearRange);
            }
            ShowMoveRangeAction(CurrentCharacterLogic);
            bClearRangeFlag = false;
        }
        UpdateSelectCharacterInfo(CurrentCharacterLogic);
    }
    public void ExecuteMove(Vector2Int destPos)
    {
        Vector2Int srcPos = CurrentCharacterLogic.GetTileCoord();
        CurrentCharacterLogic.SetTileCoord(destPos);
        ChangeState(EBattleState.Lock);
        ClearRangeAction();
        gameMode.MoveUnitAfterAction(srcPos, destPos, ConstTable.UNIT_MOVE_SPEED(), FinishMoveEvent);
    }
    public void FinishMoveEvent()
    {
        OpenMenu(EActionMenuState.AfterMove, UndoCancelSelectTargetActionAndClearRange);
    }
    public void CancelMove()
    {
        ShowMoveRangeAction(CurrentCharacterLogic);
        OpenMenu(EActionMenuState.Main, UndoCancelMainActionAndClearRange);
    }
    public void WarningCannotMove()
    {

    }
    public void ExecuteAttack()
    {
        var targetEffectRange = CurrentCharacterLogic.BattleInfo.TargetEffectRanges;
        var effectCamps = CurrentCharacterLogic.BattleInfo.GetEffectCamps();
        Debug.Log("执行Attack 生效网格数=" + targetEffectRange.Count);
        foreach (var v in targetEffectRange)
        {
            var enemy = chapterManager.GetCharacterFromCoord(v, EnumCharacterCamp.Enemy);
            if (enemy != null)
            {
                Debug.Log(v + "处发现敌方单位 ID=" + enemy.Logic.GetID());
                //先转向然后进行抖动攻击
            }
        }
    }
    public void ExecuteSkill()
    {

    }
    public void FinishAction()
    {
        //chapterManager.SaveChapterData(0);
        //chapterManager.SaveBattleData();
        ClearRangeAction();
        currentCharacter.DisableAction(true);
    }
    public void CloseMenu()
    {
        uiManager.HideBattlaActionMenu();
        gameMode.slgCamera.SetControlMode(CameraControlMode.FreeMove);
    }

    public void OpenMenu(EActionMenuState menuState, UnityAction undoAction = null)
    {
        ChangeState(EBattleState.Menu);
        gameMode.slgCamera.SetControlMode(CameraControlMode.DisableControl);
        if (undoAction != null)
            uiManager.MenuUndoAction.Push(undoAction);
        uiManager.ShowBattleActionMenu(menuState, CurrentCharacterLogic);
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
