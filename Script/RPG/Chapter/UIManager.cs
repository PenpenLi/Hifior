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
    public UI_BattleSelectWeaponMenu BattleSelectWeaponMenu { private set; get; }
    public UI_CharacterInfoPanel CharacterInfo { private set; get; }
    public UI_TurnIndicate TurnIndicate { private set; get; }
    public UI_GetItemOrMoney GetItemOrMoney { private set; get; }
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
    public void InitBattleUI(Transform panelParent0_9, Transform panelParent9_16, Transform panelParent0_16)
    {
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent9_16);
        BattleActionMenu = FindPanelInChildren<UI_BattleActionMenu>(panelParent9_16);
        BattleSelectWeaponMenu = FindPanelInChildren<UI_BattleSelectWeaponMenu>(panelParent9_16);
        CharacterInfo = FindPanelInChildren<UI_CharacterInfoPanel>(panelParent9_16);
        ScreenMask = FindPanelInChildren<UI_ScreenMask>(panelParent0_9);
        TurnIndicate = FindPanelInChildren<UI_TurnIndicate>(panelParent0_9);
        GetItemOrMoney = FindPanelInChildren<UI_GetItemOrMoney>(panelParent0_9);
        MenuUndoAction = new Stack<UnityAction>();
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

    public Stack<UnityAction> MenuUndoAction;
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
        CharacterLogic logic = battleManager.CurrentCharacterLogic;
        BuildBattleSelectWeaponMenu(logic);
        MenuUndoAction.Push(UndoShowBattleActionMenu);
    }
    /// <summary>
    /// 不显示其他的Menu
    /// </summary>
    public void UndoShowBattleActionMenu()
    {
        BattleActionMenu.Show();
        BattleSelectWeaponMenu.Hide();
    }
    public void BattleAction_End()
    {
        BattleActionMenu.Hide();
        battleManager.FinishAction();
        battleManager.ChangeState(BattleManager.EBattleState.Idel);
    }
    /// <summary>
    /// 检查是否拥有位置事件
    /// </summary>
    /// <param name="chLogic"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool GetBattleAction_LocationAction(CharacterLogic chLogic, ref UI_BattleActionMenu.UIActionButtonInfo info)
    {
        var locationEvent = chapterManager.Event.EventInfo.GetLocationEvent(chLogic.GetTileCoord(), chLogic.GetID());
        if (locationEvent == null)
        {
            Debug.Log("没有Location事件");
            return false;
        }
        Debug.Log("找到相匹配的Location Event" + locationEvent);
        info.name = locationEvent.GetButtonText();
        info.action = () =>
        {
            gameMode.pathShower.SetRootVisible(false);
            gameMode.LockInput(true);
            HideBattlaActionMenu();
            if (locationEvent.Sequence != null)
            {
                locationEvent.Execute(chapterManager.Event.EventInfo, () =>
                {
                    gameMode.pathShower.SetRootVisible(true);
                    gameMode.LockInput(false);
                    ShowBattleActionMenu(ActionMenuState, chLogic);
                });
            }
            if(locationEvent.Caption== EventInfoCollection.EnumLocationEventCaption.占领)
            {
                gameMode.ChapterManager.Event.CheckWin_Seize();
            }
            if (locationEvent.Caption == EventInfoCollection.EnumLocationEventCaption.开门)
            {
                gameMode.GridTileManager.OpenDoor(new Vector2Int(0,0));
            }
        };
        return true;
    }
    public bool CheckHasLocationEvent(CharacterLogic chLogic)
    {
        UI_BattleActionMenu.UIActionButtonInfo location = new UI_BattleActionMenu.UIActionButtonInfo();
        if (GetBattleAction_LocationAction(chLogic, ref location))
        {
            BattleActionMenu.AddAction(location);
            return true;
        }
        return false;
    }
    public void BuildBattleActionMenu_Main(CharacterLogic chLogic)
    {
        BattleActionMenu.Clear();
        CheckHasLocationEvent(chLogic);
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
        CheckHasLocationEvent(chLogic);
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
    public void BuildBattleSelectWeaponMenu(CharacterLogic chLogic)
    {
        BattleSelectWeaponMenu.Clear();
        List<WeaponItem> weaponItems = chLogic.Info.Items.GetAllWeapons();
        foreach (var v in weaponItems)
        {
            var weaponAction = new UI_BattleActionMenu.UIActionButtonInfo(v.GetName(), () => BattleAction_SelectWeapon(v));
            BattleSelectWeaponMenu.AddAction(weaponAction);
        }
        BattleSelectWeaponMenu.Show();
    }
    private void BattleAction_SelectWeapon(WeaponItem item)
    {
        battleManager.ChangeState(BattleManager.EBattleState.SelectTarget);
        if (item == null) Debug.LogError("选择的武器是null");
        else Debug.Log(item.ToString());
        BattleActionMenu.Hide();
        BattleSelectWeaponMenu.Hide();
        CharacterLogic logic = battleManager.CurrentCharacterLogic;
        logic.Info.Items.EquipWeapon(item);
        //从选择的武器确定
        var rangeType = item.GetDefinition().RangeType;
        EnumSelectEffectRangeType selRangeType = rangeType.SelectType;
        Vector2Int selRange = rangeType.SelectRange;
        EnumSelectEffectRangeType effRangeType = rangeType.EffectType;
        Vector2Int effRange = rangeType.EffectRange;
        logic.BattleInfo.SetSelectTargetParam(CharacterBattleInfo.EBattleActionType.Attack, logic.GetTileCoord(), selRangeType, selRange, effRangeType, effRange);
        battleManager.ShowSelectTargetRangeAction(logic);
        MenuUndoAction.Push(UndoSelectWeapon);
    }
    private void UndoSelectWeapon()
    {
        battleManager.ChangeState(BattleManager.EBattleState.Menu);
        battleManager.ClearRangeAction();
        BattleSelectWeaponMenu.Show();
        BattleActionMenu.Hide();
    }
    #region Battle Choose Weapon Action Menu

    #endregion
    #region Screen Fade
    public void ScreenDarkToNormal(float duration, UnityEngine.Events.UnityAction action = null)
    {
        ScreenMask.Show(false, true, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); ScreenMask.Hide(); }, duration);
    }

    public void ScreenNormalToDark(float duration, bool autoDisable, UnityEngine.Events.UnityAction action = null)
    {
        ScreenMask.Show(true, true, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); if (autoDisable) ScreenMask.Hide(); }, duration);
    }
    public void ScreenWhiteToNormal(float duration, UnityEngine.Events.UnityAction action = null)
    {
        ScreenMask.Show(false, false, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); ScreenMask.Hide(); }, duration);
    }
    public void ScreenNormalToWhite(float duration, bool autoDisable, UnityEngine.Events.UnityAction action = null)
    {
        ScreenMask.Show(true, false, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); if (autoDisable) ScreenMask.Hide(); }, duration);
    }
    #endregion
}
