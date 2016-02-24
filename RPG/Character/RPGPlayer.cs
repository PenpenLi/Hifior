using UnityEngine;
using System.Collections.Generic;

public class RPGPlayer : RPGCharacter
{
    protected PlayerDef PlayerDefinition;

    private int Exp;
    public RPGPlayer()
    {
        base.Definition = PlayerDefinition;
    }
    public override void SetDefaultData(CharacterDef DefaultData)
    {
        base.SetDefaultData(DefaultData);

        PlayerDefinition =(PlayerDef) DefaultData;
    }
}
