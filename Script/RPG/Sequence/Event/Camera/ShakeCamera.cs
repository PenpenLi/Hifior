using UnityEngine;
using System.Collections;
namespace Sequence
{
    [AddComponentMenu("Sequence/Shake Camera")]
    public class ShakeCamera : SequenceEvent
    {
        [Tooltip("晃动摄像机的时间")]
        public float duration = 0.5f;

        [Tooltip("x,y晃动的幅度")]
        public Vector2 amount = new Vector2(1, 1);

        [Tooltip("等待晃动效果结束才开始下一个事件")]
        public bool waitUntilFinished;

        public override void OnEnter()
        {
            Vector3 v = new Vector3();
            v = amount;

            if (!waitUntilFinished)
            {
                Continue();
            }

        }
        public override bool OnStopExecuting()
        {
            Continue();
            return true;
        }

        public override string GetSummary()
        {
            return "For " + duration + " seconds.";
        }
    }
}