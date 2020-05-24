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
        protected virtual void EquipWeapon(CharacterLogic target)
        {
            var weapons = logic.Info.Items.GetAllWeapons();
            if (weapons.Count <= 1) return;
            List<WeaponItem> avWeapons = new List<WeaponItem>();
            foreach (var v in weapons)
            {
                //找到每个武器可以攻击到的范围内的敌方单位

                var rangeType = v.GetDefinition().RangeType;
                EnumSelectEffectRangeType selRangeType = rangeType.SelectType;
                Vector2Int selRange = rangeType.SelectRange;
                EnumSelectEffectRangeType effRangeType = rangeType.EffectType;
                Vector2Int effRange = rangeType.EffectRange;
                logic.BattleInfo.SetSelectTargetParam(CharacterBattleInfo.EBattleActionType.Attack, logic.GetTileCoord(), selRangeType, selRange, effRangeType, effRange);
                if (logic.BattleInfo.TargetChooseRanges.Contains(target.GetTileCoord()))
                {
                    avWeapons.Add(v);
                }
            }
            List<int> damage = new List<int>();
            //根据对方的属性选择伤害最高的武器
            foreach (var v in weapons)
            {
                logic.Info.Items.EquipWeapon(v);
                int dmg = BattleLogic.GetAttackCount(logic, target) * BattleLogic.GetAttackDamage(logic, target);
                damage.Add(dmg);
            }
            int maxDamageIndex = LinqS.IndexOfMax(damage.GetEnumerator());
            logic.Info.Items.EquipWeapon(avWeapons[maxDamageIndex]);
        }
    }
}