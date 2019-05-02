using UnityEngine;
namespace Sequence
{
    [AddComponentMenu("Sequence/Go to next chapter")]
    public class GoToNextChapter : SequenceEvent
    {
        public override void OnEnter()
        {
            gameMode.UIManager.RecordChapter.Show_Save(true);
            Continue();
        }
        public override string GetSummary()
        {
            return "Go to next chapter id = " + gameMode.ChapterManager.ChapterId;
        }
    }
}