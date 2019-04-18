using UnityEngine;
using System.Collections;
[System.Serializable]
public class ActionAI
{
    public bool IsLeader;
    public EnumEnemyActionAI ActionCommand;
    public EnumEnemyCureSelfCondition CureSelf;
}

[System.Serializable]
public class DynamicAttribute
{
    public bool UsePlayerDef;
    public int Level;
    public int HardMode;
}
public class EnemyDef : ExtendScriptableObject
{
    public PropertyIDNameDesc CommonProperty;
    public PlayerDef PlayerDef;
    public ActionAI AI;
    public DynamicAttribute Attribute;
}
