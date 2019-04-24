namespace Sequence
{
    public class IfCharacterAlive : EventIf
    {
        public override bool IsTrue()
        {
            return gameMode.BattleManager.CurrentCharacterLogic.IsAlive();
        }

    }
}