using UnityEngine;
using System.Collections;
namespace Sequence
{
    [AddComponentMenu("Sequence/Set Music Volume")]
    public class SetMusicVolume : SequenceEvent
    {
        [Range(0f, 1.0f)]
        public float Volume;
        [Tooltip("降低")]
        public bool Lower;
        [Tooltip("恢复")]
        public bool Restore;
        public override void OnEnter()
        {
            SoundController musicController = SoundController.Instance;
            if (musicController != null)
            {
                if (Lower)
                {
                    musicController.LowerBGMVolume();
                }
                else if (Restore)
                {
                    musicController.RestoreBGMVolume();
                }
                else
                {
                    musicController.SetBGMVolume(Volume);
                }
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (Lower)
            {
                return "volume lower";
            }
            else if (Restore)
            {
                return "volume normal";
            }
            else
            {
                return "volume=" + Volume;
            }
        }
    }
}