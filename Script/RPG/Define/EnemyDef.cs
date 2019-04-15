using UnityEngine;
using System.Collections;

public class EnemyDef : ExtendScriptableObject
{
    public PropertyIDNameDesc CommonProperty;
    public PlayerDef PlayerDef;
   
    public EnumEnemyActionAI ActionAI;
    public bool AttackInRange;
    public EnumEnemyCureSelfCondition CureSelf;
}
