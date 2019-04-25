namespace Sequence
{
    public class IfCharacterInTeam : EventIf
    {
        public override bool IsTrue()
        {
            var ch = gameMode.BattleManager.CurrentCharacterLogic;
            if (ch == null)
            {
                UnityEngine.Debug.LogError("当前角色不存在 默认返回" + DefaultWhenError);
                return DefaultWhenError;
            }
            return ch.IsInActiveTeam();
        }
    }
}