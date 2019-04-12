using System.Collections;
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
    /// UI 撤销操作
    /// </summary>
    public System.Action MenuUndoAction;
    /// <summary>
    /// 显示攻击范围
    /// </summary>
    public System.Action<CharacterLogic> ShowMoveRangeAction;

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
        var v = new CharacterLogic();

        return v;
    }
    public ETileType GetTileType(Vector2Int tilePos)
    {
        return gameMode.grid.GetTileType(tilePos);
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
        if (vMouseInputState.active)
        {
            if (vMouseInputState.key == InputManager.EMouseKey.Left)
            {
                currentCharacterLogic = GetCharacterLogic(tilePos);
                if (currentCharacterLogic == null) { }
                else
                {
                    currentCharacterLogic.SetTileCoords(tilePos);
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
            HideTargetMoveRange();
        }
    }
    public void HandleSelectTarget()
    {
        if (IsDesktopNo())
        {
            HideTargetAttackRange();
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
            ShowMoveRangeAction(currentCharacterLogic);
        }
    }
    public void ShowTargetMoveRange()
    {

    }
    public void HideTargetMoveRange()
    {

    }
    public void ShowTargetAttackRange()
    {

    }
    public void HideTargetAttackRange()
    {

    }
    public void ExecuteMove()
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

    }
    public void CloseMenu()
    {

    }
    public void OpenMenu()
    {

    }
    public void UpdateScene()
    {

    }
}
