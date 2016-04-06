using UnityEngine;
using System.Collections;
namespace Sequence
{
    [AddComponentMenu("Sequence/Animate Camera")]
    public class AnimateCamera : SequenceEvent
    {
        public AnimationClip Clip;
        public Camera PlayCamera;
        [Tooltip("等待动画播放完毕再执行下一个片段")]
        public bool waitUntilFinished;
        public override void OnEnter()
        {
            if (Clip == null)
            {
                Continue();
                return;
            }
            if (PlayCamera == null)
                PlayCamera = Camera.current;
            Animator anim = Utils.MiscUtil.GetComponentNotNull<Animator>(PlayCamera.gameObject);
            anim.Play("C0");

            Debug.Log(Clip.legacy);
            if (waitUntilFinished)
            {
                Invoke("DoWait", Clip.length);
            }
            else
            {
                Continue();
            }
        }

        protected virtual void DoWait()
        {
            Continue();
        }

        public override string GetSummary()
        {
            if (Clip == null)
            {
                return "Error: No animation clip selected";
            }

            return Clip.name;
        }
    }
}
