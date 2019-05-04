using System.Collections.Generic;
using UnityEngine;
namespace RPG.AI
{
    public abstract class BaseAttackAI : BaseSingleTargetAI
    {
        protected EAttackPriority attackPriority;
        public BaseAttackAI(RPGCharacter ch) : base(ch)
        {
        }
        public void SetPriority(EAttackPriority _attackPriority) { attackPriority = _attackPriority; }

        public RPGCharacter SelectByPriority()
        {
            if (inRangeCharacters.Count == 0) return null;
            if (inRangeCharacters.Count == 1) return inRangeCharacters[0];
            switch (attackPriority)
            {
                case EAttackPriority.Random:
                    return GetRandomInRange();
                case EAttackPriority.Lethal:
                    break;
                case EAttackPriority.LeastHP:
                    break;
                case EAttackPriority.MostDamage:
                    break;
            }
            return null;
        }
        public override void GetInActionRange()
        {
            List<RPGCharacter> all = null;
            switch (targetCamp)
            {
                case ETargetCamp.All:
                    all = gameMode.ChapterManager.GetAllCharacters();
                    break;
                case ETargetCamp.Player:
                    all = gameMode.ChapterManager.GetAllCharacters(EnumCharacterCamp.Player);
                    break;
                case ETargetCamp.Enemy:
                    all = gameMode.ChapterManager.GetAllCharacters(EnumCharacterCamp.Enemy);
                    break;
            }

            PositionMath.InitActionScope(logic.Info.Camp, logic.GetMoveClass(), logic.GetMovement(), logic.GetTileCoord(), logic.GetSelectRangeType(), logic.GetSelectRange());
            inRangeCharacters.Clear();
            foreach (var v in all)
            {
                if (PositionMath.IsInAttackableRange(v.GetTileCoord()))
                {
                    inRangeCharacters.Add(v);
                }
            }
        }
        protected override RPGCharacter Target()
        {
            GetInActionRange();
            return SelectByPriority();
        }
    }
}