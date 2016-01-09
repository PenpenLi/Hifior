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
    public CharacterAttributeGrow DefaultAttributeGrow;
    public List<int> DefaultWeapons;
    /// <summary>
    /// 死亡时说的一行话
    /// </summary>
    public string DeadSpeech;
    /// <summary>
    /// 离开战场时说的话
    /// </summary>
    public string LeaveSpeech;
}
