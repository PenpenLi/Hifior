namespace Sequence
{
    public class IfCharacterInTeam : EventIf
{
    public override bool IsTrue()
    {
        return gameMode.BattleManager.CurrentCharacterLogic.IsInActiveTeam();
    }

}
}