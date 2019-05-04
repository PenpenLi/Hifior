using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.AI
{
    public class AI_AttackIfInRange : BaseAttackAI
    {
        public AI_AttackIfInRange(RPGCharacter ch) : base(ch)
        {
        }

        public override void Action()
        {
            base.Action();
        }

        public override string Name()
        {
            return "攻击在范围内的敌人-" + attackPriority.ToString();
        }

    }
}