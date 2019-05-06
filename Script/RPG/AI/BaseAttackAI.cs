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
        /// <summary>
        /// 获取站在原地是否攻击到的范围
        /// </summary>
        public void GetInAttackRange()
        {
            List<RPGCharacter> all = GetTargetCharacter();

            PositionMath.InitAttackScope(logic.GetTileCoord(), logic.Info.Items.Weapons);
            inRangeCharacters.Clear();
            foreach (var v in all)
            {
                if (PositionMath.IsInAttackableRange(v.GetTileCoord()))
                {
                    inRangeCharacters.Add(v);
                }
            }
        }
        /// <summary>
        /// 获取可移动到的区域的单位
        /// </summary>
        public override void GetInActionRange()
        {
            List<RPGCharacter> all = GetTargetCharacter();

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
        protected bool HasWeapon() { return unit.Logic.Info.Items.GetAllWeapons().Count > 0; }
        protected virtual WeaponItem Weapon()
        {
            var weapons = unit.Logic.Info.Items.GetAllWeapons();
            if (weapons.Count == 1) { return weapons[0]; }
            return null;
        }
    }
}