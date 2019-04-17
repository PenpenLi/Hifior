using UnityEngine;
using System.Collections;

namespace Sequence
{
    [AddComponentMenu("Sequence/Camera Move To Position")]
    public class CameraMoveToPosition : SequenceEvent
    {
        public Vector3 TargetPoint;
        [Tooltip("移动时间")]
        public float Time;

        [Tooltip("等待移动结束才开始下一个事件")]
        public bool waitUntilFinished;

        public override void OnEnter()
        {
            if (!waitUntilFinished)
            {
                Continue();
            }
        }
        protected virtual void OniTweenComplete(object param)
        {
            SequenceEvent command = param as SequenceEvent;
            if (command != null && command.Equals(this))
            {
                if (waitUntilFinished)
                {
                    Continue();
                }
            }
        }

        public override string GetSummary()
        {
            return "Move to " + TargetPoint + " in " + Time + " seconds";
        }
    }
}