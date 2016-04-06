using UnityEngine;
using System.Collections.Generic;

public class RPGPlayer : RPGCharacter
{
    protected PlayerDef PlayerDefinition;
    public RPGPlayer()
    {
        base.Definition = PlayerDefinition;
    }
    public override void SetDefaultData(CharacterDef DefaultData)
    {
        base.SetDefaultData(DefaultData);

        PlayerDefinition = (PlayerDef)DefaultData;
        foreach (int itemID in PlayerDefinition.DefaultWeapons)
            Item.AddItem(itemID);
    }
    public override void SetupPlayerInputComponent(UInputComponent InInputComponent)
    {
        base.SetupPlayerInputComponent(InInputComponent);
        InInputComponent.BindAction("A", EInputActionType.IE_Released, A);
        InInputComponent.BindAction("B", EInputActionType.IE_Released, B);
    }
    private void A()
    {
        
    }
    private void B()
    {

    }
}
