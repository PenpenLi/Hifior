using System;
using System.Collections.Generic;
using UnityEngine;
public class RPGEnemy : RPGCharacter
{
    public static RPGEnemy Create(int id, CharacterAttribute customAttribute = null)
    {
        EnemyDef def = ResourceManager.GetEnemyDef(id);
        RPGEnemy r = new RPGEnemy(def);
        if (customAttribute != null)
        {
            r.logic.SetAttribute(customAttribute);
        }
        return r;
    }
    public static RPGEnemy Create(CharacterInfo info)
    {
        RPGEnemy r = new RPGEnemy(info);
        return r;
    }
    private RPGEnemy(CharacterInfo info)
    {
        logic = new CharacterLogic(info);

        if (logic.Info.Camp != EnumCharacterCamp.Enemy) Debug.LogError("Camp 不是 Enemy 却想要创建 RPGEnemy");
    }

    private RPGEnemy(EnemyDef def)
    {
        if (def.PlayerDef == null) Debug.LogError("EnemyDef_" + def.CommonProperty.ID + " is null");
        SetDataFromDef(def.PlayerDef);
        SetAI(def.AI);
        SetCamp(EnumCharacterCamp.Enemy);
    }
    public override bool IsLeader()
    {
        return logic.Info.AI.IsLeader;
    }
}
