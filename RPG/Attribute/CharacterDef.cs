using UnityEngine;
using System.Collections.Generic;

public class CharacterDef : ExtendScriptableObject
{
    [ContextMenu("Json")]
    void Json()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }
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
