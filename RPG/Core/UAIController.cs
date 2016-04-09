using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public enum EPathFollowingStatus
{
    /// <summary>
    /// No requests 
    /// </summary>
    Idle,

    /// <summary>
    /// Request with incomplete path, will start after UpdateMove()
    /// </summary>
    Waiting,

    /// <summary>
    /// Request paused, will continue after ResumeMove()
    /// </summary>
    Paused,

    /// <summary>
    ///  /** Following path */
    /// </summary>
    Moving,
}
public class UAIController : UController
{
    private int TeamID;
    public void SetGenericTeamID(int NewTeamID)
    {
        TeamID = NewTeamID;
    }
    public int GetGenericTeamID()
    {
        return TeamID;
    }
    public virtual void Enable()
    {
        this.enabled = true;
    }
    public virtual void Disable()
    {
        this.enabled = false;
    }
    /// <summary>
    /// 当前Pawn移动结束
    /// </summary>
    public UnityAction<UPawn> ReceiveMoveCompleted;

    void OnPossess(UPawn PossessedPawn) { }

    /// <summary>
    /// 当一个给定的Pawn被UnPossess时触发
    /// </summary>
    /// <param name="UnpossessedPawn"></param>
    void OnUnpossess(UPawn UnpossessedPawn)
    {

    }

    public override void SetPawn(UPawn InPawn)
    {
        base.SetPawn(InPawn);

    }
    public EPathFollowingStatus GetMoveStatus()
    {
        return EPathFollowingStatus.Idle;
    }

    public bool PauseMove()
    {
        return true;
    }

    bool ResumeMove()
    {
        return true;
    }

    public override void StopMovement()
    {
        base.StopMovement();
    }
    public override void OnMoveCompleted(EPathFollowingStatus Result)
    {
        base.OnMoveCompleted(Result);
    }
    public override void PostInitializeComponents()
    {
        base.PostInitializeComponents();
    }
    public override void Possess(UPawn InPawn)
    {
        base.Possess(InPawn);
    }
    public override void UnPossess()
    {
        base.UnPossess();
    }
    public override void Reset()
    {
        base.Reset();
    }
}