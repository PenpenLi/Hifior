using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sequence
{
    [AddComponentMenu("Sequence/Start Battle")]
    public class StartBattle : SequenceEvent
    {
        public EnumCharacterCamp FirstActionCamp = EnumCharacterCamp.Player;
        public override void OnEnter()
        {
            gameMode.StartBattle(FirstActionCamp);
            Continue();
        }
        public override string GetSummary()
        {
            return "Start Battle";
        }
        public override bool OnStopExecuting()
        {
            return false;
        }
    }
}