using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sequence;
using System.Linq;
namespace RPG.AI
{
    public class AI_AttackIfInAttackRange : BaseAttackAI
    {
        public AI_AttackIfInAttackRange(RPGCharacter ch) : base(ch)
        {
        }
        protected override RPGCharacter Target()
        {
            GetInAttackRange();
            return SelectByPriority();
        }
        public override void Action()
        {
            base.Action();
            RPGCharacter target = Target();
            if (target == null)
            {
                return;
            }
            
            BattlePlayer.AssembleAttackSequenceEvent(AddSequenceEvent<AttackAnimation>, logic, target.Logic);

        }

        public override string Name()
        {
            return "傻站着不动，进入攻击范围才会攻击";
        }
    }
}