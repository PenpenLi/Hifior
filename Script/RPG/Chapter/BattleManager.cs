using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public struct BattleTalkEventActionInfo
{
    public CharacterLogic Logic;
    public EventInfoCollection.BattleTalkEventType Event;
    public BattleTalkEventActionInfo(CharacterLogic l, EventInfoCollection.BattleTalkEventType e)
    {
        Logic = l;
        Event = e;
    }
}
public class BattleManager : ManagerBase
{

    public enum EBattleState
    {
        Idel,
        Menu,
        SelectMove,
        SelectTarget,
        SelectTalkTarget,
        Lock,
    }
    /// <summary>
    /// 显示攻击范围
    /// </summary>
    public UnityAction<CharacterLogic> ShowMoveRangeAction;
    public UnityAction<CharacterLogic> ShowSelectTargetRangeAction;
    public UnityAction<CharacterLogic, Vector2Int> ShowEffectTargetRangeAction;
    public UnityAction<List<Vector2Int>> ShowTalkCharacterRangeAction;
    public UnityAction<List<Vector2Int>> ShowHighlightRangeAction;
    public UnityAction ClearHighlightRangeAction;
    public UnityAction ClearRangeAction;
    public UnityAction<Vector2Int> UpdateSelectTileInfo;
    public UnityAction<CharacterLogic> UpdateSelectCharacterInfo;
    public System.Func<bool> IsRangeVisible;
    private List<BattleTalkEventActionInfo> talkEventInfo;
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
            case EBattleState.SelectTalkTarget:
                HandleSelectTalkCharacter();
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
            else
            {
                UnityAction undo = uiManager.MenuUndoAction.Pop();
                if (AppConst.DebugMode) Debug.Log("Menu Undo Event = " + undo.Method);
                undo.Invoke();
            }
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
                WarningCannotSelect();
            }
        }
    }
    public void HandleSelectTarget()
    {
        gameMode.slgCamera.SetControlMode(CameraControlMode.FreeMove);
        if (inputManager.GetNoInput())
        {
            UnityAction undo = uiManager.MenuUndoAction.Pop();
            if (AppConst.DebugMode) Debug.Log(undo.Method);
            undo.Invoke();
        }
        var vMouseInputState = inputManager.GetMouseInput();
        bool dirty = vMouseInputState.IsMouseTilePosChanged();
        var targetSelectRange = CurrentCharacterLogic.BattleInfo.TargetChooseRanges;
        if (dirty)
        {
            if (targetSelectRange.Contains(vMouseInputState.tilePos))
            {
                ShowEffectTargetRangeAction(CurrentCharacterLogic, vMouseInputState.tilePos);

                var enemy = chapterManager.GetCharacterFromCoord(vMouseInputState.tilePos, EnumCharacterCamp.Enemy);
                if (enemy != null)
                {
                    uiManager.ShowAttackInfo(CurrentCharacterLogic, enemy.Logic);
                }
                else
                {
                    uiManager.HideAttackInfo();
                }
            }
            else
            {
                ClearHighlightRangeAction();
                uiManager.HideAttackInfo();
            }
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
                WarningCannotSelect();
            }
        }
    }
    public void HandleSelectTalkCharacter()
    {
        gameMode.slgCamera.SetControlMode(CameraControlMode.FreeMove);
        var vMouseInputState = inputManager.GetMouseInput();
        bool dirty = vMouseInputState.IsMouseTilePosChanged();
        if (dirty)
        {
            if (talkEventInfo.Exists((BattleTalkEventActionInfo x) => { return x.Logic.GetTileCoord() == vMouseInputState.tilePos; }))
            {
                ShowHighlightRangeAction(new List<Vector2Int> { vMouseInputState.tilePos });
            }
        }
        if (inputManager.GetNoInput())
        {
            CancelTalk();
        }
        if (vMouseInputState.IsClickedTile())
        {
            bool has = false;
            foreach (var v in talkEventInfo)
            {
                if (v.Logic.GetTileCoord() == vMouseInputState.tilePos)
                {
                    has = true;
                    ClearRangeAction();
                    gameMode.BeforePlaySequence();
                    CurrentCharacterLogic.ConsumeActionPoint(EnumActionType.Talk);
                    v.Event.Execute(chapterManager.Event.EventInfo, () =>
                    {
                        gameMode.AfterPlaySequence();
                        OpenMenu(EActionMenuState.Main);
                    });
                }
            }
            if (!has)
            {
                WarningCannotSelect();
            }
        }
    }

    public void SelectTalkCharacter(List<BattleTalkEventActionInfo> talkAction)
    {
        ChangeState(EBattleState.SelectTalkTarget);
        talkEventInfo = talkAction;
        var range = new List<Vector2Int>();
        foreach (var v in talkEventInfo)
        {
            range.Add(v.Logic.GetTileCoord());
            Debug.Log(v.Logic.GetID() + "  " + v.Logic.GetTileCoord());
        }
        ClearRangeAction();
        ShowTalkCharacterRangeAction(range);

    }
    private void CancelTalk()
    {
        ClearRangeAction();
        OpenMenu(EActionMenuState.Main);
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
        Vector2Int srcPos = currentCharacter.GetTileCoord();
        CurrentCharacterLogic.SetTileCoord(destPos);
        CurrentCharacterLogic.ConsumeActionPoint(EnumActionType.Move);
        ChangeState(EBattleState.Lock);
        ClearRangeAction();
        gameMode.BattlePlayer.MoveUnitAfterAction(currentCharacter.GetCamp(),srcPos, destPos, ConstTable.UNIT_MOVE_SPEED(), CheckRangeEvent);
    }

    public void CheckRangeEvent()
    {
        var rangeEvent = chapterManager.Event.EventInfo.GetRangeEvent(CurrentCharacterLogic.GetID(), CurrentCharacterLogic.GetCareer(), CurrentCharacterLogic.GetTileCoord());
        if (AppConst.DebugMode)
        {
            if (rangeEvent == null) Debug.Log("没有Range事件");
            else Debug.Log("找到相匹配的Range Event" + rangeEvent);
        }
        if (rangeEvent == null || rangeEvent.Sequence == null)
        {
            OpenMenu(EActionMenuState.Main);
            return;
        }

        //如果有事件发生，则在事件发生后显示回合条
        {
            gameMode.BeforePlaySequence();
            rangeEvent.Execute(chapterManager.Event.EventInfo, () =>
            {
                gameMode.AfterPlaySequence();
                OpenMenu(EActionMenuState.Main);
            });
        }
    }

    public void CancelMove()
    {
        ShowMoveRangeAction(CurrentCharacterLogic);
        OpenMenu(EActionMenuState.Main, UndoCancelMainActionAndClearRange);
    }
    public void WarningCannotSelect()
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

                ClearRangeAction();
                ClearHighlightRangeAction();
                CurrentCharacterLogic.ConsumeActionPoint(EnumActionType.Attack);
                gameMode.BattlePlayer.AttackUnit(CurrentCharacterLogic, enemy.Logic, gameMode.CheckDefeatBossWin);
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
        uiManager.HideBattlaActionMenu(true);
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
        OpenMenu(EActionMenuState.Main, UndoCancelSelectTargetActionAndClearRange);
    }
    #endregion

    public void UpdateScene()
    {

    }
}
