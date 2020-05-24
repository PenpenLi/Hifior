using System.Collections.Generic;
using UnityEngine;
using Sequence;
namespace RPG.AI
{
    public class AI_Stay : BaseAttackAI
    {
        public AI_Stay(RPGCharacter ch) : base(ch)
        {
        }

        public override void Action()
        {
            sequenceEvents = new List<Sequence.SequenceEvent>();
            var end = AddSequenceEvent<EndAction>();
            end.Character = unit;
        }

        public override string Name()
        {
            return "傻站着不动";
        }
    }
}