namespace Sequence
{
    public class IfCurrentCharacterID : EventIf
    {
        public int ID;
        public override bool IsTrue()
        {
            return gameMode.BattleManager.CurrentCharacterLogic.GetID()==ID;
        }

    }
}