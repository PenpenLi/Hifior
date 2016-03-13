using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
/// <summary>
/// 无实体，仅处理事件，用于控制切换Pawn
/// </summary>
public class UPlayerController : UController
{
    private bool bInputEnabled;
    protected List<UInputComponent> CurrentInputStack;
    private string PlayerName;
    /** The state of the inputs from cinematic mode */
    protected bool bCinemaDisableInputMove;
    protected bool bCinemaDisableInputLook;


    /** Ignores movement input. Stacked state storage, Use accessor function IgnoreMoveInput() */
    protected bool IgnoreMoveInput;

    /** Ignores look input. Stacked state storage, use accessor function IgnoreLookInput(). */
    protected bool IgnoreLookInput;

    /// <summary>
    /// 是否暂停，如果不可以暂停则执行 CanUnpauseDelegate
    /// </summary>
    /// <param name="bPause"></param>
    /// <param name="CanUnpauseDelegate"></param>
    /// <returns></returns>
    public virtual bool SetPause(bool bPause, UnityAction CanUnpauseDelegate)
    {
        if (bPause)
            Time.timeScale = 0;
        return true;
    }
    public bool IsPaused()
    {
        return Mathf.Approximately(Time.timeScale, 0.0f);
    }
    public virtual void SetupInputComponent()
    {
        if (InputComponent == null)
        {
            InputComponent = new UInputComponent(this, "PC_InputComponent0");
        }
    }
    public override string GetHumanReadableName()
    {
        return PlayerName;
    }
    /** Trys to set the player's name to the given name. */
    public override void SetName(string S)
    {
        PlayerName = S;
    }

    /** SwitchLevel to the given MapURL. */
    public virtual void SwitchLevel(string URL) { }
    /** Enable voice chat transmission */
    public void StartTalking() { }

    /** Disable voice chat transmission */
    public void StopTalking() { }
    public virtual void ToggleSpeaking(bool bInSpeaking) { }
    public void SetInputUIOnly() { }
    public void SetInputUIGame() { }
    public void SetInputGameOnly() { }
    /// <summary>
    /// 设置新的相机模式
    /// </summary>
    /// <param name="NewMode"></param>
    public virtual void Camera(string NewMode) { }
    public bool InputEnabled()
    {
        return bInputEnabled;
    }
    public virtual void DestroyInputComponent()
    {
        if (InputComponent != null)
        {
            InputComponent.ClearBindingValues();
            InputComponent = null;
        }
    }
    public override void EnableInput(UPlayerController PlayerController)
    {
        if (PlayerController == this && PlayerController != null)
        {
            bInputEnabled = true;
        }
    }
    public override void DisableInput(UPlayerController PlayerController)
    {
        if (PlayerController == this && PlayerController != null)
        {
            bInputEnabled = false;
        }
    }
    public void PushInputComponent(UInputComponent InputComponent)
    {
        if (InputComponent != null)
        {
            bool bPushed = false;
            CurrentInputStack.Remove(InputComponent);
            for (int Index = CurrentInputStack.Count - 1; Index >= 0; --Index)
            {
                UInputComponent IC = CurrentInputStack[Index];
                if (IC == null)
                {
                    CurrentInputStack.RemoveAt(Index);
                }
                else if (IC.Priority <= InputComponent.Priority)
                {
                    CurrentInputStack.Insert(Index + 1, InputComponent);
                    bPushed = true;
                    break;
                }
            }
            if (!bPushed)
            {
                CurrentInputStack.Insert(0, InputComponent);
            }
        }
    }

    public bool PopInputComponent(UInputComponent InputComponent)
    {
        if (InputComponent != null)
        {
            if (CurrentInputStack.Remove(InputComponent))
            {
                InputComponent.ClearBindingValues();
                return true;
            }
        }
        return false;
    }
    public virtual void BuildInputStack(List<UInputComponent> InputStack)
    {
        InputStack.Add(GetPawn().InputComponent);
    }

    public override void UnPossess()
    {
        if (GetPawn() != null)
        {
            GetPawn().UnPossessed();
        }
        SetPawn(null);
    }

    public override void BeginPlay()
    {
        base.BeginPlay();

        SetupInputComponent();
    }
    public override void Tick(float DeltaSeconds)
    {
        base.Tick(DeltaSeconds);

        if (bInputEnabled)
            InputComponent.TickPlayerInput();
    }
}