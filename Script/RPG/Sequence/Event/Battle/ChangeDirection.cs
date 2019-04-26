using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    public class ChangeDirection : SequenceEvent
    {
        [Tooltip("如果是-1,则代表当前角色")]
        public int ID;
        public bool resetStay;
        public EDirection direction;
        [Tooltip("考虑到以后是旋转可能需要时间")]
        public float duration = 1.0f;
        public override void OnEnter()
        {
            int id = ID;
            if (ID < 0)
            {
                id = gameMode.BattleManager.CurrentCharacterLogic.GetID(); ;
            }
            RPGCharacter ch = gameMode.ChapterManager.GetCharacterFromID(id);
            if (resetStay)
            {
                ch.GetMultiSpriteAnimator().SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Stay);
            }
            else
            {
                gameMode.unitShower.SetDirection(ch.GetSpriteRender(), direction);
            }
            Continue();
        }
    }
}