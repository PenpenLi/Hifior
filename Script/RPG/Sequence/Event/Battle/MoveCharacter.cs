using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    /// <summary>
    /// 按照一个路径移动角色，如果第一个值不是当前位置，则瞬间移动到该处
    /// </summary>
    [AddComponentMenu("Sequence/Move Character")]
    public class MoveCharacter : SequenceEvent
    {
        public RPGCharacter Character;
        public AddCharacter.ECamp Camp;
        public int CharacterID;
        public VInt2[] TileCoords;
        public override void OnEnter()
        {
            //if (Character == null)
            //{
            //    if (Camp == AddCharacter.ECamp.我方)
            //        Character = GetGameMode<GM_Battle>().GetGameStatus<GS_Battle>().GetPlayer(CharacterID);
            //    else
            //        Character = GetGameMode<GM_Battle>().GetGameStatus<GS_Battle>().GetEnemy(CharacterID);
            //}
            //if (Character == null)
            //{
            //    Continue();
            //    Debug.LogError("没有找到该角色", gameObject);
            //}
            //GetGameMode<GM_Battle>().GetSLGMap().MoveByRoutine(Character, TileCoords, null, Continue);
        }
    }
}