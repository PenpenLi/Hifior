using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 当前PlayerController控制的对象，拥有实体
/// </summary>
public class UPawn : UActor
{
    protected UInputComponent InputComponent;

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
        InputComponent.TickInput();
    }
    public virtual void Reset() { }
    public virtual void SetupPlayerInputComponent(UInputComponent InInputComponent) {  }
    public virtual void CreatePlayerInputComponent()
    {
        InputComponent = new UInputComponent("PawnInputComponent0");
    }
    public virtual void DestroyPlayerInputComponent()
    {
        if (InputComponent != null)
        {
            InputComponent.Clear();
            InputComponent = null;
        }
    }
    public void BindAction(string KeyName, InputActionType ActionType, UnityAction ActionDelegate)
    {
        InputComponent.BindAction(KeyName, ActionType, ActionDelegate);
    }
    public void BindAxisBindAxis(string KeyName, UnityAction<float> ActionDelegate)
    {
        InputComponent.BindAxis(KeyName, ActionDelegate);
    }
}
