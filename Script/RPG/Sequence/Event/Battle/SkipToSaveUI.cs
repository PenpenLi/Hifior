using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sequence
{
    [AddComponentMenu("Sequence/Skip To Save UI")]
    public class SkipToSaveUI : SequenceEvent
    {
        public override void OnEnter()
        {
            gameMode.UIManager.RecordChapter.Show();
            gameMode.UIManager.RecordChapter.RegisterHideEvent(Continue);
            Continue();
        }

        public override string GetSummary()
        {
            return "Skip to Record";
        }
    }
}