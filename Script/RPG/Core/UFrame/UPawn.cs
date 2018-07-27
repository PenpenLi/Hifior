using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 当前PlayerController控制的对象，拥有实体
/// </summary>
public class UPawn : UActor
{
    public string PawnName;

    public UPawn()
    {
        CreatePlayerInputComponent();
    }
    public override void BeginPlay()
    {
        base.BeginPlay();
        SetupPlayerInputComponent(InputComponent);
    }
    public override void Tick(float DeltaSeconds)
    {
        base.Tick(DeltaSeconds);
    }
    public virtual void Reset() { }
    public override void SetName(string s)
    {
        PawnName = s;
    }
    public override string GetHumanReadableName()
    {
        return PawnName;
    }
    public virtual void SetupPlayerInputComponent(UInputComponent InInputComponent)
    {
        if (InputComponent == null)
        {
            InputComponent = new UInputComponent(this, "Pawn_InputComponent0");
        }
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
