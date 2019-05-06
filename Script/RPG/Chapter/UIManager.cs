using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;
using UnityEngine.Events;
using System;

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
    public UI_StartMenu GameStartMenu { private set; get; }
    public UI_BattleTileInfo BattleTileInfo { private set; get; }
    public UI_BattleActionMenu BattleActionMenu { private set; get; }
    public UI_BattleMainMenu BattleMainMenu { private set; get; }
    public UI_BattleSelectWeaponMenu BattleSelectWeaponMenu { private set; get; }
    public UI_CharacterInfoPanel CharacterInfo { private set; get; }
    public UI_TalkWithoutbg TalkDialog { private set; get; }
    public UI_TurnIndicate TurnIndicate { private set; get; }
    public UI_GetItemOrMoney GetItemOrMoney { private set; get; }
    public UI_ScreenMask ScreenMask { private set; get; }
    public UI_BattleAttackInfo Left_BattleAttackInfo { private set; get; }
    public UI_BattleAttackInfo Right_BattleAttackInfo { private set; get; }
    public UI_WidgetYesNo WidgetYesNo { private set; get; }
    public UI_RecordChapterPanel RecordChapter { private set; get; }

    public UI_ChapterStartPreface ChapterStartPreface { private set; get; }
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
        GameStartMenu = FindPanelInChildren<UI_StartMenu>(panelParent0_16);
        BattleTileInfo = FindPanelInChildren<UI_BattleTileInfo>(panelParent9_16);
        BattleActionMenu = FindPanelInChildren<UI_BattleActionMenu>(panelParent9_16);
        BattleMainMenu = FindPanelInChildren<UI_BattleMainMenu>(panelParent9_16);
        BattleSelectWeaponMenu = FindPanelInChildren<UI_BattleSelectWeaponMenu>(panelParent9_16);
        CharacterInfo = FindPanelInChildren<UI_CharacterInfoPanel>(panelParent9_16);
        TalkDialog = FindPanelInChildren<UI_TalkWithoutbg>(panelParent0_9);
        ScreenMask = FindPanelInChildren<UI_ScreenMask>(panelParent0_16);
        TurnIndicate = FindPanelInChildren<UI_TurnIndicate>(panelParent0_9);
        RecordChapter = FindPanelInChildren<UI_RecordChapterPanel>(panelParent0_16);
        ChapterStartPreface = FindPanelInChildren<UI_ChapterStartPreface>(panelParent0_16);
        WidgetYesNo = FindPanelInChildren<UI_WidgetYesNo>(panelParent0_16);
        GetItemOrMoney = FindPanelInChildren<UI_GetItemOrMoney>(panelParent0_9);
        {
            Left_BattleAttackInfo = FindPanelInChildren<UI_BattleAttackInfo>(panelParent0_9);
            Left_BattleAttackInfo.gameObject.name = "Left_BattleAttackInfo";
            Right_BattleAttackInfo = GameObject.Instantiate(Left_BattleAttackInfo);
            Right_BattleAttackInfo.gameObject.name = "Right_BattleAttackInfo";
            Right_BattleAttackInfo.gameObject.SetActive(true);
            var rrt = Right_BattleAttackInfo.GetComponent<RectTransform>();
            rrt.SetParent(panelParent0_9, false);
            rrt.anchorMin = Vector2.right;
            rrt.anchorMax = Vector2.right;
            rrt.pivot = Vector2.right;
            rrt.anchoredPosition = Vector2.zero;
            Right_BattleAttackInfo.gameObject.SetActive(false);
        }
        MenuUndoAction = new Stack<UnityAction>();
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
        battleManager.ShowMoveRangeAction(battleManager.CurrentCharacterLogic);
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
    public void CheckMoveAction(CharacterLogic chLogic)
    {
        var move = new IActionMenu.UIActionButtonInfo("移动", BattleAction_Move, chLogic.IsActionEnable(EnumActionType.Move));
        BattleActionMenu.AddAction(move);
    }
    public void CheckAttackAction(CharacterLogic chLogic)
    {
        var attack = new IActionMenu.UIActionButtonInfo("攻击", BattleAction_Attack, chLogic.IsActionEnable(EnumActionType.Attack));
        BattleActionMenu.AddAction(attack);

    }
    public void CheckWaitAction(CharacterLogic chLogic)
    {
        var end = new IActionMenu.UIActionButtonInfo("待机", BattleAction_End, chLogic.IsActionEnable(EnumActionType.Wait));
        BattleActionMenu.AddAction(end);

    }
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

        EnumActionType actionType = EnumActionType.OpenTreasureBox;
        if (locationEvent.Caption == EventInfoCollection.EnumLocationEventCaption.占领)
            actionType = EnumActionType.All;
        if (locationEvent.Caption == EventInfoCollection.EnumLocationEventCaption.访问)
            actionType = EnumActionType.Visit;

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
                    if (locationEvent.Caption != EventInfoCollection.EnumLocationEventCaption.占领)//如果是占领，则不再弹出选项菜单了
                        ShowBattleActionMenu(ActionMenuState, chLogic);
                });
            }

            chLogic.ConsumeActionPoint(actionType);

            if (locationEvent.Caption == EventInfoCollection.EnumLocationEventCaption.占领)
            {
                //gameMode.ChapterManager.CheckWin_Seize();
            }
            if (locationEvent.Caption == EventInfoCollection.EnumLocationEventCaption.开门)
            {
                //gameMode.GridTileManager.OpenDoor(new Vector2Int(0, 0));
            }
        };
        info.enable = chLogic.IsActionEnable(actionType);
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
        info.enable = chLogic.IsActionEnable(EnumActionType.Talk);
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
        var point = chLogic.GetActionPoint();
        BattleActionMenu.SetActionPoint(point);
        CheckLocationEvent(chLogic);
        CheckBattleTalkEvent(chLogic);
        CheckMoveAction(chLogic);
        CheckAttackAction(chLogic);
        CheckWaitAction(chLogic);
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

    #region AttackInfo

    public void ShowAttackInfo(CharacterLogic currentCharacterLogic, CharacterLogic logic)
    {
        WeaponItem equipWeapon = currentCharacterLogic.Info.Items.GetEquipWeapon();
        var def = ResourceManager.GetWeaponDef(equipWeapon.ID);
        int afterHP = currentCharacterLogic.GetCurrentHP() - BattleLogic.GetAttackDamage(logic, currentCharacterLogic);
        Left_BattleAttackInfo.Show(currentCharacterLogic.GetPortrait(), def.Icon, def.CommonProperty.Name, currentCharacterLogic.GetMaxHP(), currentCharacterLogic.GetCurrentHP(), afterHP,
            currentCharacterLogic.GetHit(), BattleLogic.GetAttackDamage(currentCharacterLogic, logic), currentCharacterLogic.GetCritical(), BattleLogic.GetAttackCount(currentCharacterLogic, logic));

        equipWeapon = logic.Info.Items.GetEquipWeapon();
        def = ResourceManager.GetWeaponDef(equipWeapon.ID);
        afterHP = logic.GetCurrentHP() - BattleLogic.GetAttackDamage(currentCharacterLogic, logic);
        Right_BattleAttackInfo.Show(logic.GetPortrait(), def.Icon, def.CommonProperty.Name, logic.GetMaxHP(), logic.GetCurrentHP(), afterHP,
            logic.GetHit(), BattleLogic.GetAttackDamage(logic, currentCharacterLogic), logic.GetCritical(), BattleLogic.GetAttackCount(logic, currentCharacterLogic));
    }
    public void HideAfterRecord()
    {
        RecordChapter.Hide();
        WidgetYesNo.Hide();
        GameStartMenu.Hide();
    }
    public void HideAttackInfo()
    {
        Left_BattleAttackInfo.Hide(false);
        Right_BattleAttackInfo.Hide(false);
    }
    public void ShowBattleHPBar(bool show)
    {
        foreach (var v in chapterManager.GetAllCharacters())
        {
            var sr = v.GetHPSpriteRender();
            if (sr != null) sr.gameObject.SetActive(show);
        }
    }
    public void ShowAttackChangeHP(bool left, SpriteRenderer sr, int max, int src, int dest, int speed, UnityAction onComplete = null)
    {
        UI_BattleAttackInfo ui = left ? Left_BattleAttackInfo : Right_BattleAttackInfo;
        gameMode.unitShower.SetHP(sr, max, src, dest, ConstTable.UI_VALUE_BAR_SPEED());
        ui.SetHP(max, src, dest, ConstTable.UI_VALUE_BAR_SPEED(), ConstTable.UI_WAIT_TIME(), onComplete);
    }
    public void ShowAttackChangeHP(bool left, SpriteRenderer sr, int max, int src, int dest, int speed, float waitTime, UnityAction onComplete = null)
    {
        UI_BattleAttackInfo ui = left ? Left_BattleAttackInfo : Right_BattleAttackInfo;
        gameMode.unitShower.SetHP(sr, max, src, dest, ConstTable.UI_VALUE_BAR_SPEED());
        ui.SetHP(max, src, dest, ConstTable.UI_VALUE_BAR_SPEED(), waitTime, onComplete);
    }
    #endregion
    #region Screen Fade
    public void ScreenDarkToNormal(float duration, UnityEngine.Events.UnityAction action = null)
    {
        if (duration <= 0) { ScreenMask.Hide();return; }
        ScreenMask.Show(false, true, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); ScreenMask.Hide(); }, duration);
    }

    public void ScreenNormalToDark(float duration, bool autoDisable, UnityEngine.Events.UnityAction action = null)
    {
        if (duration <= 0) { ScreenMask.ShowBlack(); return; }
        ScreenMask.Show(true, true, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); if (autoDisable) ScreenMask.Hide(); }, duration);
    }
    public void ScreenWhiteToNormal(float duration, UnityEngine.Events.UnityAction action = null)
    {
        if (duration <= 0) { ScreenMask.Hide(); return; }
        ScreenMask.Show(false, false, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); ScreenMask.Hide(); }, duration);
    }
    public void ScreenNormalToWhite(float duration, bool autoDisable, UnityEngine.Events.UnityAction action = null)
    {
        if (duration <= 0) { ScreenMask.ShowWhite(); return; }
        ScreenMask.Show(true, false, duration);
        Utils.GameUtil.DelayFunc(delegate { if (action != null) action(); if (autoDisable) ScreenMask.Hide(); }, duration);
    }
    #endregion
    #region Sequence Break
    public void BreakSequence()
    {
        if (GameMode.Instance.UIManager.TalkDialog.isActiveAndEnabled)
            GameMode.Instance.UIManager.TalkDialog.Hide(true);
    }
    #endregion
}
