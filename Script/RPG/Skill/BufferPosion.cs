using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 中毒效果,Params[0]=比率扣除的,Params[1]=固定扣除的，先扣比率再扣固定
/// </summary>
public class BufferPosion : BufferBase, ISkillLogic
{
    private int previousHP;
    //计算方式，0=覆盖 1=叠加
    private int calcMode;
    public void ChangeValue(CharacterAttribute att)
    {
        previousHP = att.HP;
        att.HP -= (int)(att.HP * (Def.Params[0] / 100f)) + Def.Params[1];
        if (att.HP < 0)
        {
            att.HP = 0;
        }
    }

    public void RevertValue(CharacterAttribute att)
    {
        att.HP = previousHP;
    }

    public override EnumBufferTrigger TriggerCondition()
    {
        return EnumBufferTrigger.回合开始时;
    }

    public override EnumBufferType Type()
    {
        return EnumBufferType.中毒;
    }

    public void Visual()
    {
        Debug.Log("Play skill visual");
    }
}