using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Pawn_BattleArrow : UPawn
{
    [Header("敌方行动AI控制器")]
    public AC_EnemyRoundAction EnemyAI;
    [Header("素材选定")]
    public AudioClip Audio_OutBounds;
    /// <summary>
    /// 当前光标所在位置
    /// </summary>
    [Header("运行变量")]
    public Point2D Position;
    /// <summary>
    /// 前一个选择的目标位置
    /// </summary>
    public Point2D OldPosition;

    public enum ESelectStatus
    {
        其他,
        无关UI显示中,
        无物体选定,
        选定出移动范围,
        人物移动中,
        未执行任何动作前选定动作,
        选择武器,
        选定要攻击的人物,
        攻击过程,
        选择要对话的人物,
        无法再次返回的动作,
        剧情播放中
    }
    ESelectStatus selectStatus = ESelectStatus.无物体选定;
    public float ArrowHeight = 2.5f;
    /// <summary>
    /// 是否当前光标上有角色可被选择
    /// </summary>
    private bool HasCharacterOnArrow
    {
        get { return ArrowOnCharacter != null && Visible; }
    }
    /// <summary>
    /// 当前光标上的角色
    /// </summary>
    private RPGCharacter ArrowOnCharacter;
    /// <summary>
    /// 当前选择的角色
    /// </summary>
    private RPGCharacter SelectedCharacter;
    /// <summary>
    /// 当前选择的角色的周围人物;
    /// </summary>
    private List<RPGCharacter> Neighbors;
    SLGMap m_slgmap;
    SLGMap SlgMap
    {
        get
        {
            if (m_slgmap == null)
                m_slgmap = GetGameMode<GM_Battle>().GetSLGMap();
            return m_slgmap;
        }
    }
    SLGChapter m_slgchapter;
    SLGChapter SlgChapter
    {
        get
        {
            if (m_slgchapter == null)
                m_slgchapter = GetGameMode<GM_Battle>().GetSLGChapter();
            return m_slgchapter;
        }
    }
    public override void BeginPlay()
    {
        base.BeginPlay();

        SetArrowActive(false);
    }
    /// <summary>
    /// 状态的切换延迟一点时间，以防止Pawn和UI同时响应同一个按钮事件
    /// </summary>
    /// <param name="Status"></param>
    /// <param name="Delay"></param>
    private void DelaySetStatus(ESelectStatus Status, float Delay = 0.2f)
    {
        Utils.GameUtil.DelayFunc(() => selectStatus = Status, Delay);
    }
    /// <summary>
    /// 设置光标的位置，如果光标不可见则跳过
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetPosition(int x, int y)
    {
        if (!Visible)
            return;
        int fx = x;
        int fy = y;
        if (x < 0 || x > SlgMap.MapTileX || y < 0 || y > SlgMap.MapTileY)
        {
            SoundController.Instance.PlaySound(Audio_OutBounds, 1.0f);
            fx = Mathf.Clamp(x, 0, SlgMap.MapTileX);
            fy = Mathf.Clamp(y, 0, SlgMap.MapTileY);
        }
        Position = new Point2D(fx, fy);
        //更新地图信息的显示
        UIController.Instance.GetUI<RPG.UI.BattleTileInfo>().Show(SlgMap.MapTileData.GetTileData(Position.x, Position.y).Type);

        transform.position = Point2D.Point2DToVector3(fx, fy, ArrowHeight + SlgMap.GetTileHeight(fx, fy), true);
        ArrowOnCharacter = GetGameStatus<GS_Battle>().GetAnyUnitAt(new Point2D(fx, fy));
        if (HasCharacterOnArrow)
        {
            UIController.Instance.GetUI<RPG.UI.CharStatePanel>().Show(ArrowOnCharacter);
        }
        else
        {
            UIController.Instance.GetUI<RPG.UI.CharStatePanel>().Hide();
        }
        if (selectStatus == ESelectStatus.选定出移动范围)
        {
            SlgMap.ShowMoveRoutine(fx, fy);
        }
    }
    #region 输入控制
    public override void SetupPlayerInputComponent(UInputComponent InInputComponent)
    {
        base.SetupPlayerInputComponent(InInputComponent);

        InInputComponent.BindAction("Down", EInputActionType.IE_Clicked, Down);
        InInputComponent.BindAction("Up", EInputActionType.IE_Clicked, Up);
        InInputComponent.BindAction("Left", EInputActionType.IE_Clicked, Left);
        InInputComponent.BindAction("Right", EInputActionType.IE_Clicked, Right);
        InInputComponent.BindAction("A", EInputActionType.IE_Pressed, A);
        InInputComponent.BindAction("B", EInputActionType.IE_Pressed, B);
        InInputComponent.BindAction("X", EInputActionType.IE_Pressed, X);
        InInputComponent.BindAction("Y", EInputActionType.IE_Pressed, Y);
    }
    private void Up()
    {
        SetPosition(Position.x, Position.y + 1);
    }
    private void Down()
    {
        SetPosition(Position.x, Position.y - 1);
    }
    private void Left()
    {
        SetPosition(Position.x - 1, Position.y);
    }
    private void Right()
    {
        SetPosition(Position.x + 1, Position.y);
    }
    private void A()
    {
        switch (selectStatus)
        {
            case ESelectStatus.无物体选定:
                {
                    if (HasCharacterOnArrow && ArrowOnCharacter.Controllable)
                    {
                        OldPosition = Position;
                        ArrowOnCharacter.ShowMovement();
                        SelectedCharacter = ArrowOnCharacter;
                        selectStatus = ESelectStatus.选定出移动范围;
                    }
                    else
                    {
                        if (Visible)
                        {
                            UIController.Instance.GetUI<RPG.UI.BattleMainPanel>().Show();
                            SetArrowActive(false);
                        }
                    }
                    break;
                }
            case ESelectStatus.选定出移动范围:
                {
                    ///选择的人物不是我方的则返回
                    if (SelectedCharacter.GetCamp() != EnumCharacterCamp.Player)
                        return;
                    if (Position == SelectedCharacter.GetTileCoord() || SlgMap.CanMoveTo(Position))
                    {
                        SelectedCharacter.MoveTo(Position, null, OnMoveToDest);
                        selectStatus = ESelectStatus.人物移动中;
                    }
                    else
                    {
                        SoundController.Instance.PlaySound(Audio_OutBounds, 1.0f);
                    }
                    break;
                }
            case ESelectStatus.选择武器:
                break;
            case ESelectStatus.选定要攻击的人物:
                {
                    ExecuteAttack();
                    break;
                }
            case ESelectStatus.选择要对话的人物:
                {
                    bActionHasTalk = true;
                    foreach (RPGCharacter Neighbor in Neighbors)
                    {
                        if (Neighbor.GetTileCoord() == Position)
                        {
                           SLGChapter.BattleTalkEventType Talk= SlgChapter.GetBattleTalkEvent(SelectedCharacter.GetCharacterID(), Neighbor.GetCharacterID(), Neighbor.GetCamp());
                            Talk.Execute(ResetToChooseActionAfterTalkEvent);
                            selectStatus = ESelectStatus.剧情播放中;
                            SetArrowActive(false);
                            SlgMap.HideCompanionRange();
                        }
                        else
                        {
                            SoundController.Instance.PlaySound(Audio_OutBounds);
                        }
                    }
                    break;
                }
        }
    }
    private void B()
    {
        switch (selectStatus)
        {

            case ESelectStatus.选定出移动范围:
                {
                    SlgMap.HideMoveRange();
                    SlgMap.HideAttackRange();
                    SetPosition(OldPosition.x, OldPosition.y);
                    selectStatus = ESelectStatus.无物体选定;
                    break;
                }
            case ESelectStatus.未执行任何动作前选定动作:
            case ESelectStatus.人物移动中:
                {
                    ResetToOldPosition();
                    break;
                }
            case ESelectStatus.选择武器:
                {
                    ResetToChooseAction();
                    break;
                }
            case ESelectStatus.选定要攻击的人物:
                {
                    ResetToChooseWeapon();
                    break;
                }
            case ESelectStatus.选择要对话的人物:
                {
                    ResetToChooseAction();
                    break;
                }
        }
    }
    private void X()
    {
        switch (selectStatus)
        {
            case ESelectStatus.无物体选定:
                if (HasCharacterOnArrow)
                {
                    UIController.Instance.GetUI<RPG.UI.AttributePanel>().Show(ArrowOnCharacter);
                    ShowNothingUI();
                    UIController.Instance.GetUI<RPG.UI.AttributePanel>().RegisterOnHide(OnHideNothingUI);
                }
                break;
        }
    }

    private void Y()
    {

    }
    #endregion
    #region 无关UI开启和关闭
    private void ShowNothingUI()
    {
        SetArrowActive(false);
        selectStatus = ESelectStatus.无关UI显示中;
    }
    private void OnHideNothingUI()
    {
        selectStatus = ESelectStatus.无物体选定;
        SetArrowActive(true);
    }
    #endregion
    /// <summary>
    /// 回合结束后临到我方行动回合执行的事件
    /// </summary>
    public override void Reset()
    {
        base.Reset();

        selectStatus = ESelectStatus.无物体选定;
        SetArrowActive(true);
        SetArrowOnDefaultPlayer();
    }
    /// <summary>
    /// 设置光标可见性
    /// </summary>
    /// <param name="Visible">可见性</param>
    public void SetArrowActive(bool Visible)
    {
        transform.GetChild(0).gameObject.SetActive(Visible);
        if (Visible)
            UIController.Instance.GetUI<RPG.UI.BattleTileInfo>().Show(SlgMap.MapTileData.GetTileData(Position.x, Position.y).Type);
        else
            UIController.Instance.GetUI<RPG.UI.BattleTileInfo>().Hide();
    }
    public void SetArrowOnDefaultPlayer()
    {
        RPGCharacter FirstPlayer = GetGameStatus<GS_Battle>().GetFirstGamePlayer() as RPGCharacter;
        Point2D p = FirstPlayer.GetTileCoord();
        SetPosition(p.x, p.y);
    }
    /// <summary>
    /// 设置光标可见性,延时可以防止同时触发UI和Game事件
    /// </summary>
    /// <param name="Visible">可见性</param>
    /// <param name="Delay">延迟的时间</param>
    public void SetArrowActive(bool Visible, float Delay)
    {
        if (Delay > 0f)
        {
            Utils.GameUtil.DelayFunc(() => SetArrowActive(Visible), Delay);
        }
    }
    /// <summary>
    /// 光标可见性,可见则可以地图信息UI也出现
    /// </summary>
    public bool Visible
    {
        get
        {
            return transform.GetChild(0).gameObject.activeSelf;
        }
    }
    /// <summary>
    /// 重置到最开始选择的状态
    /// </summary>
    private void ResetToOldPosition()
    {
        SelectedCharacter.StopRun();
        SetArrowActive(true);
        SetPosition(OldPosition.x, OldPosition.y);
        UIController.Instance.GetUI<RPG.UI.ActionMenu>().Hide();
        SelectedCharacter.SetTileCoord(OldPosition.x, OldPosition.y, true);
        SelectedCharacter.ShowMovement();
        selectStatus = ESelectStatus.选定出移动范围;
    }
    /// <summary>
    /// 重置到 选择动作的界面
    /// </summary>
    private void ResetToChooseAction()
    {
        OnMoveToDest();
        UIController.Instance.GetUI<RPG.UI.AttackMenu>().Hide();
    }
    /// <summary>
    /// 对话事件结束后重新回到选择动作的界面
    /// </summary>
    private void ResetToChooseActionAfterTalkEvent()
    {
        Debug.Log("对话事件结束");
        selectStatus = ESelectStatus.无法再次返回的动作;
        ShowActionMenu();
    }
    /// <summary>
    /// 重置到选择武器的界面
    /// </summary>
    private void ResetToChooseWeapon()
    {
        SlgMap.HideAttackRange();
        SetArrowActive(false);
        Button_Attack();
    }
    /// <summary>
    /// 移动结束后执行的事件
    /// </summary>
    private void OnMoveToDest()
    {
        SlgMap.HideCompanionRange();
        SlgMap.HideRoutine();
        SlgMap.HideMoveRange();
        SlgMap.HideAttackRange();
        selectStatus = ESelectStatus.未执行任何动作前选定动作;
        UIController.Instance.GetUI<RPG.UI.CharStatePanel>().Hide();
        SetArrowActive(false);
        ShowActionMenu();
    }

    #region 执行过的动作记录变量
    private bool bActionHasAttack = false;
    private bool bActionHasTalk = false;
    private bool bActionHasLocation = false;
    /// <summary>
    /// 设置以上标志量为false
    /// </summary>
    private void ClearActionRecord()
    {
        bActionHasAttack = false;
        bActionHasTalk = false;
        bActionHasLocation = false;
    }
    #endregion
    /// <summary>
    /// 计算需要显示那几个按钮
    /// </summary>
    private void ShowActionMenu()
    {
        GS_Battle GameStatus = GetGameStatus<GS_Battle>();
        List<RPG.UI.EventButtonDetail> Details = new List<RPG.UI.EventButtonDetail>();
        //如果已经攻击，或者对话，或者进入村庄开宝箱等则直接等待
        if (bActionHasAttack || bActionHasTalk || bActionHasLocation)
        {
            Details.Add(new RPG.UI.EventButtonDetail("等待", Button_Wait));
        }
        else {
            #region 攻击按钮
            Details.Add(new RPG.UI.EventButtonDetail("攻击", Button_Attack));
            #endregion

            #region 对话按钮
            Neighbors = GameStatus.GetNeighbors(SelectedCharacter.GetTileCoord());
            bool ShouldShowTalk = false;
            foreach (RPGCharacter Neighbor in Neighbors)
            {
                SLGChapter.BattleTalkEventType TalkEvent = SlgChapter.GetBattleTalkEvent(SelectedCharacter.GetCharacterID(), Neighbor.GetCharacterID(), Neighbor.GetCamp());
                if (TalkEvent != null)
                {
                    ShouldShowTalk = true;
                    break;
                }
            }
            if (ShouldShowTalk)
                Details.Add(new RPG.UI.EventButtonDetail("对话", Button_TalkTo));
            #endregion

            #region 村庄，宝箱 

            #endregion
            Details.Add(new RPG.UI.EventButtonDetail("等待", Button_Wait));
        }
        UIController.Instance.GetUI<RPG.UI.ActionMenu>().Show(Details);

    }
    #region 菜单委托事件
    /// <summary>
    /// 按下 攻击 按钮 弹出攻击菜单
    /// </summary>
    private void Button_Attack()
    {
        selectStatus = ESelectStatus.选择武器;
        UIController.Instance.GetUI<RPG.UI.AttackMenu>().Show(SelectedCharacter, Button_OnWeaponClicked);
    }
    private void Button_TalkTo()
    {
        Debug.Log("选择人物进行对话");
        List<Point2D> NeighborsPosition = new List<Point2D>();
        foreach (RPGCharacter Neighbor in Neighbors)
        {
            NeighborsPosition.Add(Neighbor.GetTileCoord());
        }
        UnityEngine.Assertions.Assert.AreNotEqual(NeighborsPosition.Count, 0, "错误，相邻人物数量为0");
        SlgMap.ShowCompanionSprite(NeighborsPosition);
        SetArrowActive(true);
        SetPosition(NeighborsPosition[0].x, NeighborsPosition[0].y);
        DelaySetStatus(ESelectStatus.选择要对话的人物);
    }
    /// <summary>
    /// 按下 待机 按钮 进行待机
    /// </summary>
    private void Button_Wait()
    {
        ClearActionRecord();//清除记录的动作

        UIController.Instance.GetUI<RPG.UI.ActionMenu>().Hide();
        SetArrowActive(true);
        DelaySetStatus(ESelectStatus.无物体选定);
        SelectedCharacter.DisableControl();
    }
    /// <summary>
    /// 选择了一个武器进行攻击 弹出攻击范围选择攻击的对象
    /// </summary>
    public void Button_OnWeaponClicked(int ItemIndex)
    {
        SetArrowActive(true);
        List<Point2D> p = SelectedCharacter.FindAttack(true);
        SelectedCharacter.Item.EquipItemWithSort(ItemIndex);//点击选择武器则重新排列武器并装备第一个
        int x = SelectedCharacter.GetTileCoord().x;
        int y = SelectedCharacter.GetTileCoord().y;
        //延迟0.2s后切换状态
        DelaySetStatus(ESelectStatus.选定要攻击的人物);
        for (int i = 0; i < p.Count; i++)
        {
            if (m_slgmap.IsOccupyByEnemy(p[i].x, p[i].y))
            {
                SetPosition(p[i].x, p[i].y);
                return;
            }
        }
        SetPosition(x, y);
    }
    /// <summary>
    /// 主菜单结束回合按钮
    /// </summary>
    public void EndPlayerTurn()
    {
        Debug.Log("确认结束回合");
        //GetPawn<Pawn_BattleArrow>().SetArrowActive(true, 0.1f);
        int round = GetGameMode<GM_Battle>().Round;
        GetGameMode<GM_Battle>().RoundCamp = EnumCharacterCamp.Enemy;
        UIController.Instance.GetUI<RPG.UI.TurnAnim>().RegisterOnHide(() => EnemyAI.Reset());
        UIController.Instance.GetUI<RPG.UI.TurnAnim>().Show(round, EnumCharacterCamp.Enemy);
        GetGameStatus<GS_Battle>().EnableAllPlayerControl();
    }
    #endregion
    /// <summary>
    /// 执行攻击过程，包含经验值结算，死亡结算
    /// </summary>
    private void ExecuteAttack()
    {
        if (HasCharacterOnArrow)
        {
            if (ArrowOnCharacter.GetCamp() == EnumCharacterCamp.Enemy)
            {
                SlgMap.HideAttackRange();
                SetArrowActive(false);
                SelectedCharacter.BeginAttack(ArrowOnCharacter, OnAttackFinish);
                selectStatus = ESelectStatus.攻击过程;
            }
        }
    }
    /// <summary>
    /// 当攻击结束执行，在经验值结算后，死亡界面后执行
    /// </summary>
    private void OnAttackFinish()
    {
        UIController.Instance.GetUI<RPG.UI.ActionMenu>().Show(new RPG.UI.EventButtonDetail("等待", Button_Wait));
        selectStatus = ESelectStatus.无法再次返回的动作;
    }
    public override void Tick(float DeltaSeconds)
    {
        base.Tick(DeltaSeconds);
    }
}