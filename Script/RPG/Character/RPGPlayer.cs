using UnityEngine;
using System.Collections.Generic;

public class RPGPlayer : RPGCharacter
{
    public static RPGPlayer Create(int id, CharacterAttribute customAttribute = null)
    {
        PlayerDef def = ResourceManager.GetPlayerDef(id);
        RPGPlayer r = new RPGPlayer(def);
        if (customAttribute != null)
        {
            r.logic.SetAttribute(customAttribute);
        }
        return r;
    }
    public static RPGPlayer Create(CharacterInfo info)
    {
        RPGPlayer r = new RPGPlayer(info);
        return r;
    }
    private RPGPlayer(CharacterInfo info)
    {
        logic = new CharacterLogic(info);
    }
    private RPGPlayer(PlayerDef def)
    {
        SetDataFromDef(def);
    }
    public override bool IsLeader()
    {
        var id = Logic.GetID();
        return id == ConstTable.LEADER_0 || id == ConstTable.LEADER_1 || id == ConstTable.LEADER_2;
    }
}
