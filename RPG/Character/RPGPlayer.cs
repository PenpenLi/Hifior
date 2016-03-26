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

        PlayerDefinition =(PlayerDef) DefaultData;
    }
}
