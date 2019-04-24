using UnityEngine;
namespace Sequence
{
    public class IfPositionHasCharacter : EventIf
    {
        public Vector2Int TilePos;
        public EnumCharacterCamp Camp;
        public override bool IsTrue()
        {
            var ch = gameMode.BattleManager.GetCharacter(TilePos);
            if (ch == null) return false;
            if (Camp == EnumCharacterCamp.All) return true;
            else return Camp == ch.GetCamp();
        }

    }
}