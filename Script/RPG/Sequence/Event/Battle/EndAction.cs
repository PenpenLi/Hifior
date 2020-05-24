using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sequence
{
    [AddComponentMenu("Sequence/End Action")]
    public class EndAction : SequenceEvent
    {
        public RPGCharacter Character;
        public override void OnEnter()
        {
            Character.DisableAction(true);
            Continue();
        }
        public override string GetSummary()
        {
            return "待机";
        }
    }
}