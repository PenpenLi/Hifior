using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    public class AttackShake : SequenceEvent
    {
        [Tooltip("如果是-1,则代表当前角色")]
        public int ID = -1;
        public EDirection direction;
        public float duration = 0.35f;
        [Range(0.0f, 1.0f)]
        public float intensity = 0.45f;
        [Range(0.0f,1.0f)]
        public float delayContinue=1.0f;
        public override void OnEnter()
        {
            int id = ID;
            if (ID < 0)
            {
                id = gameMode.BattleManager.CurrentCharacterLogic.GetID(); ;
            }
            RPGCharacter ch = gameMode.ChapterManager.GetCharacterFromID(id);
            gameMode.unitShower.Shake(ch.GetSpriteRender(), direction, duration, intensity, ()=>DelayContinue(delayContinue));
            
        }
    }
}