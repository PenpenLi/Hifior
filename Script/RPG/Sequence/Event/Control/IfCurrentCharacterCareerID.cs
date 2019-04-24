namespace Sequence
{
    public class IfCurrentCharacterCareerID : EventIf
    {
        public int CareerID;
        public override bool IsTrue()
        {
            return gameMode.BattleManager.CurrentCharacterLogic.GetCareer() == CareerID;
        }

    }
}