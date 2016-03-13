using UnityEngine;
using System.Collections;

namespace Sequence
{
    public class WaitTime : SequenceEvent
    {
        [Tooltip("等待的时间")]
        public float duration = 1;

        public override void OnEnter()
        {
            Invoke("OnWaitComplete", duration);
        }

        void OnWaitComplete()
        {
            Continue();
        }

        public override string GetSummary()
        {
            return duration.ToString() + " seconds";
        }
    }
}