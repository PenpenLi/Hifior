using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;

public class UIManager : ManagerBase
{
    #region Init 
    public UI_BattleTileInfo BattleTileInfo { private set; get; }
    public UI_BattleActionMenu BattleActionMenu { private set; get; }
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
    public void InitBattleUI(Transform panelParent)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent);
        BattleActionMenu = FindPanelInChildren<UI_BattleActionMenu>(panelParent);
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

    /// <summary>
    /// UI 撤销操作
    /// </summary>
    public System.Action MenuUndoAction;
    #region Battle Action Menu
    public enum EActionMenuState
    {
        Main,
        Attack,
        Skill,
        UseItem,
        ExangeItem,
    }

    public void BattleAction_Move() {
        BattleActionMenu.Hide();
        battleManager.ChangeState(BattleManager.EBattleState.SelectMove);
    }
    public void BattleAction_Attack() { }
    public void BattleAction_End() { }
    public void BuildBattleActionMenu(CharacterLogic chLogic)
    {
        var move = new UI_BattleActionMenu.UIActionButtonInfo("移动", BattleAction_Move);
        BattleActionMenu.AddAction(move);
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
                BuildBattleActionMenu(chLogic);
                break;
        }
    }
    #endregion
}
