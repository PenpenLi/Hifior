using UnityEngine;
using System.Collections.Generic;

public class RPGPlayer : RPGCharacter
{
    protected PlayerDef PlayerDefinition;
    public RPGPlayer()
    {
        base.Definition = PlayerDefinition;
    }
    /// <summary>
    /// 是否是主角
    /// </summary>
    /// <returns></returns>
    public override bool IsLeader()
    {
        return GetCharacterID() == ConstTable.LEADER_0 || GetCharacterID() == ConstTable.LEADER_1 || GetCharacterID() == ConstTable.LEADER_2;
    }
    public override void SetDefaultData(CharacterDef DefaultData)
    {
        base.SetDefaultData(DefaultData);

        PlayerDefinition = (PlayerDef)DefaultData;
        Item.AddWeapons(PlayerDefinition.DefaultWeapons);
    }
    public override void SetupPlayerInputComponent(UInputComponent InInputComponent)
    {
        base.SetupPlayerInputComponent(InInputComponent);
        InInputComponent.BindAction(InputIDentifier.INPUT_A, EInputActionType.IE_Released, A);
        InInputComponent.BindAction(InputIDentifier.INPUT_B, EInputActionType.IE_Released, B);
    }
    private void A()
    {

    }
    private void B()
    {

    }
}
