using UnityEngine;
using System.Collections;

namespace Sequence
{
    [AddComponentMenu("Sequence/Wait Frame")]
    public class WaitFrame : SequenceEvent
    {
        [Tooltip("等待的Frame")]
        public int count = 60;

        public override void OnEnter()
        {
            StartCoroutine(WaitForComplete());
        }

        IEnumerator WaitForComplete()
        {
            int i = 0;
            while (i++ < count)
            {
                yield return null;
            }
            Continue();
        }

        public override string GetSummary()
        {
            return count + " frames";
        }
    }
}