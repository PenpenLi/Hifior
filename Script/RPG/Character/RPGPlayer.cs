using UnityEngine;
using System.Collections.Generic;

public class RPGPlayer : RPGCharacter
{
    public static RPGPlayer Create(int id, CharacterAttribute customAttribute = null)
    {
        PlayerDef def = ResourceManager.GetPlayerDef(id);
        RPGPlayer r = new RPGPlayer();
        r.SetDefaultData(def);
        if (customAttribute != null)
        {
            r.logic.SetAttribute(customAttribute);
        }
        return r;
    }
    public static RPGPlayer Create(CharacterInfo info)
    {
        RPGPlayer r = new RPGPlayer();
        return r;
    }
    /// <summary>
    /// 是否是主角
    /// </summary>
    /// <returns></returns>
    public override bool IsLeader()
    {
        return GetCharacterID() == ConstTable.LEADER_0 || GetCharacterID() == ConstTable.LEADER_1 || GetCharacterID() == ConstTable.LEADER_2;
    }

    private void A()
    {

    }
    private void B()
    {

    }
}
