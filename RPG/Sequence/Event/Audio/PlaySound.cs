using UnityEngine;
using System.Collections;

namespace Sequence
{
    [HierarchyIcon("Sound.jpg",2)]
    public class PlaySound : SequenceEvent
    {
        [Tooltip("需要播放的声音效果")]
        public AudioClip soundClip;

        [Range(0, 1)]
        [Tooltip("音量")]
        public float volume = 1;

        [Tooltip("等待直到声音播放完毕再执行下一个片段")]
        public bool waitUntilFinished;

        public override void OnEnter()
        {
            if (soundClip == null)
            {
                Continue();
                return;
            }

            SoundController musicController = SoundController.Instance;
            if (musicController != null)
            {
                musicController.PlaySound(soundClip, volume);
            }

            if (waitUntilFinished)
            {
                Invoke("DoWait", soundClip.length);
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
            if (soundClip == null)
            {
                return "Error: No music clip selected";
            }

            return soundClip.name;
        }
    }
}