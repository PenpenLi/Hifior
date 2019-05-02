using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;
namespace Sequence
{
    /// <summary>
    /// 按照一个路径移动角色，如果第一个值不是当前位置，则瞬间移动到该处
    /// </summary>
    [AddComponentMenu("Sequence/Move Character")]
    public class MoveCharacter : SequenceEvent
    {
        public RPGCharacter Character;
        public EnumCharacterCamp Camp;
        public int CharacterID;
        public EModeSpeed Speed;
        public List<Vector2Int> Routine;
        public bool CameraFollow;
        public bool WaitUntilFinished;
        public override void OnEnter()
        {
            Vector2Int startPos = Routine.First();
            Vector2Int endPos = Routine.Last();
            Assert.IsFalse(startPos == endPos, "移动点和终结点相同");
            RPGCharacter ch = null;
            if (CharacterID >= 0)
            {
                ch = gameMode.ChapterManager.GetCharacterFromID(CharacterID);
                Assert.IsNotNull(ch, startPos + "处不存在角色");
                var chPos = ch.GetTileCoord();
                Assert.IsTrue(startPos == chPos, "移动起始点" + startPos + "与角色所在位置" + chPos + "不相符");
            }
            else
            {
                ch = gameMode.ChapterManager.GetCharacterFromCoord(startPos);
                Assert.IsNotNull(ch, startPos + "处不存在角色");
            }
            ch.Logic.SetTileCoord(endPos);

            if (CameraFollow) gameMode.slgCamera.StartFollowTransform(ch.GetTransform());

            if (WaitUntilFinished)
                gameMode.BattlePlayer.MoveUnitByRoutine(Routine, ConstTable.UNIT_MOVE_SPEED(Speed), Continue);
            else
            {
                gameMode.BattlePlayer.MoveUnitByRoutine(Routine, ConstTable.UNIT_MOVE_SPEED(Speed), null);
                Continue();
            }
        }
        public override void Continue()
        {
            base.Continue();
            if(CameraFollow) gameMode.slgCamera.SetOldControlMode();
        }
    }
}