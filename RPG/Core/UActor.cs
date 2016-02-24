﻿using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 所有实体的基类
/// </summary>
public class UActor : MonoBehaviour
{
    public bool bHidden;
    public bool bActorEnableCollision;
    public bool bActorIsBeingDestroyed;
    public float CreationTime;
    public bool bCanBeDamaged;
    public bool bBlockInput;
    public int InputPriority;
    public UInputComponent InputComponent;
    public bool bCanEverTick = true;
    public UGameInstance GetGameInstance()
    {
        return UGameInstance.Instance;
    }
    public T GetGameMode<T>() where T : UGameMode
    {
        return GetGameInstance().GetGameMode<T>();
    }
    public T GetPawn<T>() where T : UPawn
    {
        return GetGameInstance().GetPawn<T>();
    }
    public T GetPlayerController<T>() where T : UPlayerController
    {
        return GetGameInstance().GetPlayerController<T>();
    }
    public T GetGameState<T>() where T : UGameState
    {
        return GetGameInstance().GetGameState<T>();
    }
    public T GetHUD<T>() where T : UHUD
    {
        return GetGameInstance().GetHUD<T>();
    }
    public T GetPlayerState<T>() where T : UPlayerState
    {
        return GetGameInstance().GetPlayerState<T>();
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
    public virtual void EnableInput(UPlayerController PlayerController)
    {
        if (InputComponent == null)
        {
            InputComponent = new UInputComponent(this, transform.name + "_Input");
            InputComponent.Priority = InputPriority;
            InputComponent.bBlockInput = bBlockInput;
        }
        else
        {
            // Make sure we only have one instance of the InputComponent on the stack
            PlayerController.PopInputComponent(InputComponent);
        }

        PlayerController.PushInputComponent(InputComponent);
    }
    public virtual void DisableInput(UPlayerController PlayerController)
    {
        if (InputComponent != null)
        {
            if (PlayerController)
            {
                PlayerController.PopInputComponent(InputComponent);
            }
            else
            {
                /*将所有的PlayerControllerPop下
                for (FConstPlayerControllerIterator PCIt = GetWorld()->GetPlayerControllerIterator(); PCIt; ++PCIt)
                {
                    (*PCIt)->PopInputComponent(InputComponent);
                }*/
            }
        }
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
        if (InputComponent != null)
            InputComponent.TickPlayerInput();
        if (bCanEverTick)
            Tick(Time.deltaTime);
    }
}