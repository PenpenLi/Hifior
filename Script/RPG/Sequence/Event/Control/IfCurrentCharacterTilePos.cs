using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sequence
{
    public class IfCurrentCharacterTilePos : EventIf
    {
        public Range2D Range;
        public override bool IsTrue()
        {
            var pos= gameMode.BattleManager.CurrentCharacterLogic.GetTileCoord() ;
           return Range2D.InRange(pos.x, pos.y, Range);
        }

    }
}