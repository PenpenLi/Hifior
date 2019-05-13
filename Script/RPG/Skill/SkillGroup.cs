using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillGroup
{ 
    public SkillGroup(List<EnumPassiveSkillType> defaultSkills = null)
    {
        if (defaultSkills != null) {
            passiveSkillStates = new List<PassiveSkillState>();
            foreach(var v in defaultSkills)
            {
                passiveSkillStates.Add(PassiveSkillState.PassiveSkills[v]);
            }
        }
    }

    [SerializeField]
    private List<PassiveSkillState> passiveSkillStates;

    private PassiveSkillState None { get { return PassiveSkillState.None; } }
    public PassiveSkillState GetPassiveSkill(EnumPassiveSkillEffect effect)
    {
        foreach (var v in passiveSkillStates)
        {
            if (v.Effect == effect) return v;
        }
        return None;
    }
    public List<PassiveSkillState> GetPassiveSkills(EnumPassiveSkillEffect effect)
    {
        List<PassiveSkillState> r = new List<PassiveSkillState>();
        foreach (var v in passiveSkillStates)
        {
            if (v.Effect == effect) r.Add(v);
        }
        return r;
    }
}
