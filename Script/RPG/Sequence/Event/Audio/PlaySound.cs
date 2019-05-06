using UnityEngine;
using System.Collections;

namespace Sequence
{
    [AddComponentMenu("Sequence/Play Sound")]
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

            SoundManage musicController = SoundManage.Instance;
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
        public override bool OnStopExecuting()
        {
            return true;
        }
        public override string GetSummary()
        {
            if (soundClip == null)
            {
                return null;
            }

            return soundClip.name;
        }
    }
}