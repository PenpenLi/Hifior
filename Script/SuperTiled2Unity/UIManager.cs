using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;
using UnityEngine.Events;

public enum EActionMenuState
{
    Main,
    AfterMove,
    Attack,
    Skill,
    UseItem,
    ExangeItem,
}
public class UIManager : ManagerBase
{
    #region Init 
    public UI_BattleTileInfo BattleTileInfo { private set; get; }
    public UI_BattleActionMenu BattleActionMenu { private set; get; }
    public UI_CharacterInfoPanel CharacterInfo { private set; get; }
    public UI_TurnIndicate TurnIndicate { private set; get; }
    public UI_ScreenMask ScreenMask { private set; get; }
    private T FindPanelInChildren<T>(Transform t) where T : IPanel
    {
        T r = null;
        foreach (Transform i in t)
        {
            r = i.GetComponent<T>();
            if (r != null) break;
        }
        if (r == null) Debug.LogError(typeof(T).Name + "is not find under " + t.name);
        return r;
    }
    public void InitBattleUI(Transform panelParent0_9,Transform panelParent9_16,Transform panelParent0_16)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent9_16);
        BattleActionMenu = FindPanelInChildren<UI_BattleActionMenu>(panelParent9_16);
        CharacterInfo = FindPanelInChildren<UI_CharacterInfoPanel>(panelParent9_16);
        ScreenMask = FindPanelInChildren<UI_ScreenMask>(panelParent0_9);
        TurnIndicate = FindPanelInChildren<UI_TurnIndicate>(panelParent0_9);
    }
    public void InitMainUI(Transform panelParent)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent);
    }
    public void InitPropertyUI(Transform panelParent)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent);
    }
    public void InitSettingUI(Transform panelParent)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent);
    }
    #endregion
    #region Bind 
    public void UpdateTileInfo(Vector2Int tilePos)
    {
        uiManager.BattleTileInfo.Show(gameMode.GetTileType(tilePos));
    }
    public void UpdateCharacterInfo(CharacterLogic logic)
    {
        uiManager.CharacterInfo.Show(logic);
    }

    #endregion
    #region Battle Action Menu
    /// <summary>
    /// UI 撤销操作
    /// </summary>
    public UnityAction MenuUndoAction;
    private EActionMenuState eActionMenuState;
    public EActionMenuState ActionMenuState { get { return eActionMenuState; } }
    public void BattleAction_Move()
    {
        BattleActionMenu.Hide();
        battleManager.ChangeState(BattleManager.EBattleState.SelectMove);
    }
    public void BattleAction_Attack()
    {
        BattleActionMenu.Hide();
        battleManager.ChangeState(BattleManager.EBattleState.SelectTarget);
    }
    public void BattleAction_End()
    { 
        battleManager.FinishAction();
    }
    public void BuildBattleActionMenu_Main(CharacterLogic chLogic)
    {
        BattleActionMenu.Clear();
        var move = new UI_BattleActionMenu.UIActionButtonInfo("移动", BattleAction_Move);
        BattleActionMenu.AddAction(move);
        var attack = new UI_BattleActionMenu.UIActionButtonInfo("攻击", BattleAction_Attack);
        BattleActionMenu.AddAction(attack);
        var end = new UI_BattleActionMenu.UIActionButtonInfo("待机", BattleAction_End);
        BattleActionMenu.AddAction(end);
        BattleActionMenu.Show();
    }
    public void BuildBattleActionMenu_AfterMove(CharacterLogic chLogic)
    {
        BattleActionMenu.Clear();
        var attack = new UI_BattleActionMenu.UIActionButtonInfo("攻击", BattleAction_Attack);
        BattleActionMenu.AddAction(attack);
        var end = new UI_BattleActionMenu.UIActionButtonInfo("待机", BattleAction_End);
        BattleActionMenu.AddAction(end);
        BattleActionMenu.Show();
    }
    public void ShowBattleActionMenu(EActionMenuState state, CharacterLogic chLogic)
    {
        switch (state)
        {
            case EActionMenuState.Main:
                BuildBattleActionMenu_Main(chLogic);
                break;
            case EActionMenuState.AfterMove:
                BuildBattleActionMenu_AfterMove(chLogic);
                break;
        }
        eActionMenuState = state;
    }
    public void HideBattlaActionMenu()
    {
        BattleActionMenu.Hide();
    }
    #endregion
    #region Screen Fade
    public void ScreenDarkToNormal(float duration, UnityEngine.Events.UnityAction action = null)
    {
        ScreenMask.Show(false, true, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); ScreenMask.Hide(); }, duration);
    }

    public  void ScreenNormalToDark(float duration, bool autoDisable, UnityEngine.Events.UnityAction action = null)
    {
        ScreenMask.Show(true, true, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); if (autoDisable) ScreenMask.Hide(); }, duration);
    }
    public  void ScreenWhiteToNormal(float duration, UnityEngine.Events.UnityAction action = null)
    {
        ScreenMask.Show(false, false, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); ScreenMask.Hide(); }, duration);
    }
    public  void ScreenNormalToWhite(float duration, bool autoDisable, UnityEngine.Events.UnityAction action = null)
    {
        ScreenMask.Show(true, false, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); if (autoDisable) ScreenMask.Hide(); }, duration);
    }
    #endregion
}
