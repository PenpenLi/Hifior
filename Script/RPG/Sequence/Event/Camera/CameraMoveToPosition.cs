using UnityEngine;
using System.Collections;

namespace Sequence
{
    [AddComponentMenu("Sequence/Camera Move To Position")]
    public class CameraMoveToPosition : SequenceEvent
    {
        public Vector3 TargetPoint;
        [Tooltip("平滑类型")]
        public iTween.EaseType EaseType;

        [Tooltip("移动时间")]
        public float Time;

        [Tooltip("等待移动结束才开始下一个事件")]
        public bool waitUntilFinished;

        public override void OnEnter()
        {
            Hashtable tweenParams = new Hashtable();
            tweenParams.Add("easeType", EaseType);
            tweenParams.Add("time", Time);
            tweenParams.Add("loopType", "none");
            tweenParams.Add("x", TargetPoint.x);
            tweenParams.Add("y", TargetPoint.y);
            tweenParams.Add("z", TargetPoint.z);
            tweenParams.Add("oncomplete", "OniTweenComplete");
            tweenParams.Add("oncompletetarget", gameObject);
            tweenParams.Add("oncompleteparams", this);
            iTween.MoveTo(Camera.main.gameObject, tweenParams);

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