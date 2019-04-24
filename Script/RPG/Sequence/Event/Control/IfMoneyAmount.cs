namespace Sequence
{
    public class IfMoneyAmount : EventIf
    {
        public EComparer Comparer;
        public int Money;
        public override bool IsTrue()
        {
            int turn = gameMode.ChapterManager.TurnIndex;
            Compare(Comparer, turn, Money);
            return false;
        }

    }
}