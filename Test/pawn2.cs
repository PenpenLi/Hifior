using UnityEngine;
using System.Collections;

public class pawn2 : UPawn {
    public override void SetupPlayerInputComponent(UInputComponent InInputComponent)
    {
        InInputComponent.BindAction("Fire1", EInputActionType.IE_Released, () => { Debug.Log("fire1"); });
    }
}
