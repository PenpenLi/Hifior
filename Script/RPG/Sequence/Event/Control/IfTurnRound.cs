namespace Sequence
{

    public class IfTurnRound : EventIf
    {
        public EComparer Comparer;
        [UnityEngine.Range(0,100)]
        public int Turn;
        public override bool IsTrue()
        {
            int turn = gameMode.ChapterManager.TurnIndex;
            Compare(Comparer, turn, Turn);
            return false;
        }

    }
}