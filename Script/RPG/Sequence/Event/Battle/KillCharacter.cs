using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;
namespace Sequence
{
    /// <summary>
    /// 按照一个路径移动角色，如果第一个值不是当前位置，则瞬间移动到该处
    /// </summary>
    [AddComponentMenu("Sequence/Kill Character")]
    public class KillCharacter : SequenceEvent
    {
        public RPGCharacter Character;
        public EnumCharacterCamp Camp;
        public EModeSpeed Speed;

        public int CharacterID;
        public Vector2Int TilePos;

        public override void OnEnter()
        {
            RPGCharacter ch = null;
            if (CharacterID >= 0)
            {
                ch = gameMode.ChapterManager.GetCharacterFromID(CharacterID);
                Assert.IsNotNull(ch, CharacterID + " id 角色不存在");
                gameMode.BattlePlayer.KillUnit(CharacterID, ConstTable.UNIT_DISAPPEAR_SPEED(Speed), Continue);
            }
            else
            {
                ch = gameMode.ChapterManager.GetCharacterFromCoord(TilePos);
                Assert.IsNotNull(ch, TilePos + "处不存在角色");
                gameMode.BattlePlayer.KillUnitAt(TilePos, ConstTable.UNIT_DISAPPEAR_SPEED(Speed), Continue);
            }
        }
    }
}