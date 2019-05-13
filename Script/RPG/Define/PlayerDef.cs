using UnityEngine;
using System.Collections.Generic;

public class PlayerDef : CharacterDef
{
    public CharacterAttributeGrow DefaultAttributeGrow;
    public List<int> DefaultWeapons;
    public List<EnumPassiveSkillType> DefaultSkills;
    /// <summary>
    /// 死亡时说的一行话
    /// </summary>
    public string DeadSpeech;
    /// <summary>
    /// 离开战场时说的话
    /// </summary>
    public string LeaveSpeech;
    public PlayerDef()
    {
        DefaultWeapons = new List<int>();
        DefaultSkills = new List<EnumPassiveSkillType>();
    }
}
