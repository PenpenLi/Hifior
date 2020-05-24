using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sequence;
using System.Linq;
namespace RPG.AI
{
    public class AI_AttackIfInMoveRange : BaseAttackAI
    {
        public AI_AttackIfInMoveRange(RPGCharacter ch) : base(ch)
        {
        }

        public override void Action()
        {
            base.Action();
            RPGCharacter target = Target();
            if (target == null)
            {
                return;
            }
            EquipWeapon(target.Logic);

            var side = PositionMath.GetSidewayTilePos(target.GetTileCoord());
            var avSidePos = PositionMath.MoveableAreaPoints.Intersect(side).ToList();
            if (avSidePos.Count == 0) return;
            Vector2Int bestPos = PositionMath.GetBestTilePos(avSidePos);
            List<Vector2Int> routines = PositionMath.GetMoveRoutine(bestPos);
            var move = AddSequenceEvent<MoveCharacter>();
            move.CameraFollow = true;
            move.Routine = routines;
            move.CharacterID = -1;
            move.WaitUntilFinished = true;
            move.Speed = EModeSpeed.Normal;

            BattlePlayer.AssembleAttackSequenceEvent(AddSequenceEvent<AttackAnimation>, logic, target.Logic);

        }

        public override string Name()
        {
            return "攻击在范围内的敌人-" + attackPriority.ToString();
        }

    }
}