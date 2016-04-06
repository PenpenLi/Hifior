using UnityEngine;
using System.Collections;

public class Pawn_BattleArrow : UPawn
{
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

    public enum ESelectState
    {
        无物体选定,
        选定出移动范围,
        人物移动中,
        选定动作,
        攻击过程
    }
    ESelectState selectState = ESelectState.无物体选定;
    public float ArrowHeight = 2.5f;
    /// <summary>
    /// 是否当前光标上有角色可被选择
    /// </summary>
    private bool HasCharacterOnArrow
    {
        get { return ArrowOnCharacter != null; }
    }
    /// <summary>
    /// 当前光标上的角色
    /// </summary>
    private RPGCharacter ArrowOnCharacter;
    /// <summary>
    /// 当前选择的角色
    /// </summary>
    private RPGCharacter SelectedCharacter;
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
    public override void BeginPlay()
    {
        base.BeginPlay();

        PossessedBy(GetPlayerController<UPlayerController>());
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
        transform.position = Point2D.Point2DToVector3(fx, fy, ArrowHeight + SlgMap.GetTileHeight(fx, fy), true);
        ArrowOnCharacter = GetGameState<GS_Battle>().GetAnyUnitAt(new Point2D(fx, fy));
        if (HasCharacterOnArrow)
        {
            UIController.Instance.GetUI<RPG.UI.CharStatePanel>().Show(ArrowOnCharacter);
        }
        else
        {
            UIController.Instance.GetUI<RPG.UI.CharStatePanel>().Hide();
        }
        if (selectState == ESelectState.选定出移动范围)
        {
            SlgMap.ShowMoveRoutine(fx, fy);
        }
    }

    public override void SetupPlayerInputComponent(UInputComponent InInputComponent)
    {
        base.SetupPlayerInputComponent(InInputComponent);

        InInputComponent.BindAction("Down", EInputActionType.IE_Clicked, Down);
        InInputComponent.BindAction("Up", EInputActionType.IE_Clicked, Up);
        InInputComponent.BindAction("Left", EInputActionType.IE_Clicked, Left);
        InInputComponent.BindAction("Right", EInputActionType.IE_Clicked, Right);
        InInputComponent.BindAction("A", EInputActionType.IE_Released, A);
        InInputComponent.BindAction("B", EInputActionType.IE_Released, B);
    }
    public void Up()
    {
        SetPosition(Position.x, Position.y + 1);
    }
    public void Down()
    {
        SetPosition(Position.x, Position.y - 1);
    }
    public void Left()
    {
        SetPosition(Position.x - 1, Position.y);
    }
    public void Right()
    {
        SetPosition(Position.x + 1, Position.y);
    }
    public void A()
    {
        switch (selectState)
        {
            case ESelectState.无物体选定:
                {
                    if (HasCharacterOnArrow)
                    {
                        OldPosition = Position;
                        ArrowOnCharacter.ShowMovement();
                        SelectedCharacter = ArrowOnCharacter;
                        selectState = ESelectState.选定出移动范围;
                    }
                    break;
                }
            case ESelectState.选定出移动范围:
                {
                    if (Position == SelectedCharacter.GetTileCoord() || SlgMap.CanMoveTo(Position))
                    {
                        SelectedCharacter.MoveTo(Position, null, OnMoveToDest);
                        selectState = ESelectState.人物移动中;
                    }
                    else
                    {
                        SoundController.Instance.PlaySound(Audio_OutBounds, 1.0f);
                    }
                    break;
                }
        }
    }
    public void B()
    {
        if (selectState == ESelectState.选定出移动范围)
        {
            SlgMap.HideMoveRange();
            SlgMap.HideAttackRange();
            selectState = ESelectState.无物体选定;
        }
        if (selectState == ESelectState.选定动作 || selectState == ESelectState.人物移动中)
        {
            ResetToOldPosition();
        }
    }
    /// <summary>
    /// 设置光标的可见性
    /// </summary>
    /// <param name="Visible"></param>
    public void SetArrowActive(bool Visible)
    {
        transform.GetChild(0).gameObject.SetActive(Visible);
    }
    /// <summary>
    /// 光标可见性
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
        selectState = ESelectState.选定出移动范围;
    }
    /// <summary>
    /// 移动结束后执行的事件
    /// </summary>
    private void OnMoveToDest()
    {
        SlgMap.HideRoutine();
        SlgMap.HideMoveRange();
        SlgMap.HideAttackRange();
        selectState = ESelectState.选定动作;
        UIController.Instance.GetUI<RPG.UI.ActionMenu>().Show();
        SetArrowActive(false);
        Debug.Log("移动到大");
    }
}