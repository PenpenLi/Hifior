using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;
using UnityEngine.Events;

public enum EActionMenuState
{
    Main,
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
    public UI_BattleMainMenu BattleMainMenu { private set; get; }
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
        BattleMainMenu = FindPanelInChildren<UI_BattleMainMenu>(panelParent9_16);
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
        battleManager. ShowMoveRangeAction(battleManager.CurrentCharacterLogic);
    }
    public void BattleAction_Attack()
    {
        BattleActionMenu.Hide();
        CharacterLogic logic = battleManager.CurrentCharacterLogic;
        BuildBattleSelectWeaponMenu(logic);
        BattleSelectWeaponMenu.Show();
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
        HideBattlaActionMenu(true);
        battleManager.FinishAction();
        battleManager.ChangeState(BattleManager.EBattleState.Idel);
    }
    #region  事件添加的菜单选项
    /// <summary>
    /// 检查是否拥有位置事件
    /// </summary>
    /// <param name="chLogic"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool GetBattleAction_Location(CharacterLogic chLogic, ref IActionMenu.UIActionButtonInfo info)
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
            gameMode.BeforePlaySequence();
            HideBattlaActionMenu(false);
            if (locationEvent.Sequence != null)
            {
                locationEvent.Execute(chapterManager.Event.EventInfo, () =>
                {
                    gameMode.AfterPlaySequence();
                    ShowBattleActionMenu(ActionMenuState, chLogic);
                });
            }
            if (locationEvent.Caption == EventInfoCollection.EnumLocationEventCaption.占领)
            {
                gameMode.ChapterManager.Event.CheckWin_Seize();
            }
            if (locationEvent.Caption == EventInfoCollection.EnumLocationEventCaption.开门)
            {
                gameMode.GridTileManager.OpenDoor(new Vector2Int(0, 0));
            }
        };
        return true;
    }
    public bool CheckLocationEvent(CharacterLogic chLogic)
    {
        IActionMenu.UIActionButtonInfo location = new IActionMenu.UIActionButtonInfo();
        if (GetBattleAction_Location(chLogic, ref location))
        {
            BattleActionMenu.AddAction(location);
            return true;
        }
        return false;
    }
    public bool GetBattleAction_BattleTalk(CharacterLogic chLogic, ref IActionMenu.UIActionButtonInfo info)
    {
        //找到临近的4个单位的id
        var center = chLogic.GetTileCoord();
        var sidewayCharacter = chapterManager.GetSidewayCharacter(center);
        List<BattleTalkEventActionInfo> talkEvents = new List<BattleTalkEventActionInfo>();
        foreach (var v in sidewayCharacter)
        {
            var t = chapterManager.Event.EventInfo.GetBattleTalkEvent(chLogic.GetID(), v.Logic.GetID());
            if (t != null)
            {
                talkEvents.Add(new BattleTalkEventActionInfo(v.Logic, t));
            }
        }
        if (talkEvents.Count == 0)
        {
            Debug.Log("没有Talk事件");
            return false;
        }
        Debug.Log("找到相匹配的Tald Event" + Utils.TextUtil.GetListString(talkEvents));

        info.name = talkEvents[0].Event.GetButtonText();
        info.action = () =>
        {
            HideBattlaActionMenu(false);
            //进入选择Target阶段，然后将TalkEvents和坐标一并传入到BattleManager里面的SelectTarget阶段。
            //点击选择后执行绑定后的动作
            battleManager.SelectTalkCharacter(talkEvents);
        };
        return true;
    }
    public bool CheckBattleTalkEvent(CharacterLogic chLogic)
    {
        IActionMenu.UIActionButtonInfo location = new IActionMenu.UIActionButtonInfo();
        if (GetBattleAction_BattleTalk(chLogic, ref location))
        {
            BattleActionMenu.AddAction(location);
            return true;
        }
        return false;
    }
    #endregion

    public void BuildBattleActionMenu_Main(CharacterLogic chLogic)
    {
        BattleActionMenu.Clear();
        CheckLocationEvent(chLogic);
        CheckBattleTalkEvent(chLogic);
        var move = new IActionMenu.UIActionButtonInfo("移动", BattleAction_Move);
        BattleActionMenu.AddAction(move);
        var attack = new IActionMenu.UIActionButtonInfo("攻击", BattleAction_Attack);
        BattleActionMenu.AddAction(attack);
        var end = new IActionMenu.UIActionButtonInfo("待机", BattleAction_End);
        BattleActionMenu.AddAction(end);
    }
    public void BuildBattleActionMenu_AfterMove(CharacterLogic chLogic)
    {
        BattleActionMenu.Clear();
        CheckBattleTalkEvent(chLogic);
        CheckLocationEvent(chLogic);
        var attack = new IActionMenu.UIActionButtonInfo("攻击", BattleAction_Attack);
        BattleActionMenu.AddAction(attack);
        var end = new IActionMenu.UIActionButtonInfo("待机", BattleAction_End);
        BattleActionMenu.AddAction(end);
    }
    public void ShowBattleActionMenu(EActionMenuState state, CharacterLogic chLogic)
    {
        HideBattleMainMenu();
        switch (state)
        {
            case EActionMenuState.Main:
                BuildBattleActionMenu_Main(chLogic);
                break;
        }
        eActionMenuState = state;
        BattleActionMenu.Show();
    }
    public void HideBattlaActionMenu(bool showMainMenu)
    {
        Debug.Log("Hide " + showMainMenu);
        if (showMainMenu) ShowBattleMainMenu();
        BattleActionMenu.Hide();
    }

    public void ShowBattleMainMenu()
    {
        BattleMainMenu.Clear();
        var endTurn = new IActionMenu.UIActionButtonInfo("结束行动", EndPlayerTurn);
        BattleMainMenu.AddAction(endTurn);
        BattleMainMenu.Show();
    }
    public void HideBattleMainMenu()
    {
        BattleMainMenu.Hide();
    }
    #endregion
    public void BuildBattleSelectWeaponMenu(CharacterLogic chLogic)
    {
        BattleSelectWeaponMenu.Clear();
        List<WeaponItem> weaponItems = chLogic.Info.Items.GetAllWeapons();
        foreach (var v in weaponItems)
        {
            var weaponAction = new IActionMenu.UIActionButtonInfo(v.GetName(), () => BattleAction_SelectWeapon(v));
            BattleSelectWeaponMenu.AddAction(weaponAction);
        }
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
    private void EndPlayerTurn()
    {
        //将所有玩家的状态设置为已经行动
        battleManager.ClearRangeAction();
        chapterManager.NextTurn();
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
