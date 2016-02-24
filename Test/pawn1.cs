using UnityEngine;
using System.Collections;

public class pawn1 : UPawn {
    public override void BeginPlay()
    {
        base.BeginPlay();
        Debug.Log("second pawn");
    }
    public override void SetupPlayerInputComponent(UInputComponent InInputComponent)
    {
        InInputComponent.BindAction("Jump", EInputActionType.IE_Released, OnJump);
    }
    void OnJump()
    {
        Debug.Log("JUmp");
    }
}
