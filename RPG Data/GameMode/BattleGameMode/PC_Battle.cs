using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PC_Battle : UPlayerController {
    private bool HasPossess=false;
    public bool HasPossessPawn()
    {
        return HasPossess;
    }
    public override void Possess(UPawn InPawn)
    {
        base.Possess(InPawn);

        HasPossess = true;
    }
    public override void UnPossess()
    {
        base.UnPossess();

        HasPossess = false;
    }

}
