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
            var ch = gameMode.BattleManager.CurrentCharacterLogic;
            if (ch == null)
            {
                UnityEngine.Debug.LogError("当前角色不存在 默认返回" + DefaultWhenError);
                return DefaultWhenError;
            }
            var pos = ch.GetTileCoord();
            return Range2D.InRange(pos.x, pos.y, Range);
        }

    }
}