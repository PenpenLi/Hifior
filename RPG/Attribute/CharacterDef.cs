using UnityEngine;
using System.Collections.Generic;

public class CharacterDef : ExtendScriptableObject
{
    public PropertyIDNameDesc CommonProperty;
    /// <summary>
    /// 战场上的模型
    /// </summary>
    public Sprite Portrait;
    public GameObject BattleModel;
    public EnumCharacterImportance CharacterImportance;
    public int Career;
    public int DefaultLevel;
    public CharacterAttribute DefaultAttribute;
}
