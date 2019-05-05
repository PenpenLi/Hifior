using UnityEngine;
using System.Collections;
namespace Sequence
{
    [AddComponentMenu("Sequence/Set Music Volume")]
    public class SetMusicVolume : SequenceEvent
    {
        [Range(0, 10)]
        public int Volume;
        [Tooltip("降低")]
        public bool Lower;
        [Tooltip("恢复")]
        public bool Restore;
        public override void OnEnter()
        {
            SoundManage musicController = SoundManage.Instance;
            if (musicController != null)
            {
                if (Lower)
                {
                    musicController.LowerBGM();
                }
                else if (Restore)
                {
                    musicController.NormalBGM();
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