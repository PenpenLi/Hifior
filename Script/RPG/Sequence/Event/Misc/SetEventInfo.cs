using UnityEngine;
using System;

namespace Sequence
{
    public class SetEventInfo : SequenceEvent
    {
        public override void OnEnter()
        {
            Debug.Log(RootSequence.EventRef);
        }
    }
}