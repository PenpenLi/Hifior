using UnityEngine;
/// <summary>
/// 所有实体的基类
/// </summary>
public class UActor : MonoBehaviour
{
    public bool bHidden;
    public bool bBlockInput;
    public bool bActorEnableCollision;
    public bool bActorIsBeingDestroyed;
    public float CreationTime;
    public bool bCanBeDamaged;
    
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
        Tick(Time.deltaTime);
    }
}
