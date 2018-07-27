using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
/// <summary>
/// 所有实体的基类
/// </summary>
public class UActor : MonoBehaviour
{
    [Header("Actor基类参数")]
    public bool bBlockInput;
    public int InputPriority;
    public UInputComponent InputComponent;
    public bool bCanEverTick = true;
    public UGameInstance GetGameInstance()
    {
        return UGameInstance.Instance;
    }
    public T GetPlayerPawn<T>() where T : UPawn
    {
        return GetGameInstance().GetPlayerPawn<T>();
    }
    public virtual string GetHumanReadableName()
    {
        return gameObject.name;
    }
    public virtual void SetName(string s)
    {
        gameObject.name = s;
    }
    public void SetActorTickEnabled(bool bEnabled)
    {
        bCanEverTick = bEnabled;
    }
    public void BindAction(string KeyName, EInputActionType ActionType, UnityAction ActionDelegate)
    {
        InputComponent.BindAction(KeyName, ActionType, ActionDelegate);
    }
    public void BindAxis(string KeyName, UnityAction<float> ActionDelegate)
    {
        InputComponent.BindAxis(KeyName, ActionDelegate);
    }

    public void EnableTick()
    {
        bCanEverTick = true;
    }
    public void DisableTick()
    {
        bCanEverTick = false;
    }
    public bool IsActorTickEnabled()
    {
        return bCanEverTick;
    }
    public virtual void BeginPlay()
    {

    }
    public virtual void Tick(float DeltaTime)
    {
    }
    void Start()
    {
        BeginPlay();
    }

    void Update()
    {
        if (UGameInstance.InputModeGame && InputComponent != null)
            InputComponent.TickPlayerInput();
        if (bCanEverTick)
            Tick(Time.deltaTime);
    }
}
