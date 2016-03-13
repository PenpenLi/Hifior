using UnityEngine;
using System.Collections;
namespace Sequence
{
    public class StopMusic : SequenceEvent
    {

        public override void OnEnter()
        {
            SoundController musicController = SoundController.Instance;
            if (musicController != null)
            {
                musicController.StopMusic();
            }

            Continue();
        }
    }
}