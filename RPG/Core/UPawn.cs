using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 当前PlayerController控制的对象，拥有实体
/// </summary>
public class UPawn : UActor
{
    [Header("Pawn基类参数")]
    public UController Controller;
    UPlayerState PlayerState;
    private bool bInputEnabled=true;
    public string PawnName;

    public UPawn()
    {
        CreatePlayerInputComponent();
    }
    public void SetPlayerState(UPlayerState PlayerState)
    {
        this.PlayerState = PlayerState;
    }
    public override void BeginPlay()
    {
        base.BeginPlay();
        SetupPlayerInputComponent(InputComponent);
    }
    public override void Tick(float DeltaSeconds)
    {
        base.Tick(DeltaSeconds);
        InputComponent.bBlockInput = !bInputEnabled;
    }
    public virtual void Reset() { }
    public virtual void SetupPlayerInputComponent(UInputComponent InInputComponent)
    {
        if (InputComponent == null)
        {
            InputComponent = new UInputComponent(this, "Pawn_InputComponent0");
        }
    }
    public override void SetName(string s)
    {
        PawnName = s;
    }
    public override string GetHumanReadableName()
    {
        return PawnName;
    }
    public virtual void CreatePlayerInputComponent()
    {
        InputComponent = new UInputComponent(this, "PawnInputComponent0");
    }
    public virtual void DestroyPlayerInputComponent()
    {
        if (InputComponent != null)
        {
            InputComponent.ClearBindingValues();
            InputComponent = null;
        }
    }

    public void PossessedBy(UController NewController)
    {
        Controller = NewController;
        this.enabled = true;
        NewController.SetPawn(this);
        if (Controller.PlayerState != null)
        {
            PlayerState = Controller.PlayerState;
        }
    }
    public void UnPossessed()
    {
        UController OldController = Controller;
        OldController.SetPawn(null);
        PlayerState = null;
        transform.SetParent(null);
        Controller = null;
        this.enabled = false;
        // Unregister input component if we created one
        DestroyPlayerInputComponent();
    }
    public bool IsControlled()
    {
        UPlayerController PC = (UPlayerController)(Controller);
        return (PC != null);
    }
    /// <summary>
    /// 获取Controller
    /// </summary>
    /// <returns></returns>
    public UController GetController()
    {
        return Controller;
    }
    public override void EnableInput(UPlayerController PlayerController)
    {
        if (PlayerController == Controller || PlayerController == null)
        {
            InputComponent.bBlockInput = false;
        }
        else
        {
            Debug.LogError("EnableInput can only be specified on a Pawn for its Controller");
        }
    }

    public override void DisableInput(UPlayerController PlayerController)
    {
        if (PlayerController == Controller || PlayerController == null)
        {
            bInputEnabled = false;
        }
        else
        {
            Debug.LogError("DisableInput can only be specified on a Pawn for its Controller");
        }
    }

    public bool IsAnimating()
    {
        return GetComponent<Animation>().isPlaying;
    }

    public void ChangeSkinColor(Color c)
    {
        SkinnedMeshRenderer[] smr = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (smr == null)
            return;
        foreach (SkinnedMeshRenderer sr in smr)
        {
            sr.material.color = c;
        }
    }
}
