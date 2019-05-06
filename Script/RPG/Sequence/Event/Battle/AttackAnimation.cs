using System;
using UnityEngine;
using UnityEngine.Events;
namespace Sequence
{
    public class AttackAnimation : SequenceEvent
    {
        public BattleAttackInfo AttackInfo;
        public bool IsLeft;
        EDirection atk_direction;
        EDirection def_direction;
        RPGCharacter atk, def;
        SpriteRenderer atkSr, defSr;
        public float WaitTime;
        public override void OnEnter()
        {
            atk = gameMode.ChapterManager.GetCharacterFromCoord(AttackInfo.attacker.GetTileCoord());
            def = gameMode.ChapterManager.GetCharacterFromCoord(AttackInfo.defender.GetTileCoord());
            atkSr = atk.GetSpriteRender();
            defSr = def.GetSpriteRender();
            atk_direction = PositionMath.GetDirection(atk.GetTileCoord(), def.GetTileCoord());
            gameMode.unitShower.SetDirection(atk.GetSpriteRender(), atk_direction);
            def_direction = PositionMath.GetDirection(def.GetTileCoord(), atk.GetTileCoord());
            gameMode.unitShower.SetDirection(def.GetSpriteRender(), def_direction);
            gameMode.UIManager.ShowAttackInfo(atk.Logic, def.Logic);
            Utils.GameUtil.DelayFunc(this, Shake, 0.25f);
        }
        public override bool OnStopExecuting()
        {
            return false;
        }
        private void Shake()
        {
            gameMode.unitShower.Shake(atkSr, atk_direction, 0.25f, 0.4f, HP);
        }
        private void Avoid()
        {
            EDirection d = (EDirection)(((int)atk_direction + 1) % 4);
            gameMode.unitShower.Shake(defSr, d, 0.4f, 0.3f, Continue);
        }
        private void HP()
        {
            if (AttackInfo.damageToAttack > 0)
            {
                int maxHP1 = atk.Logic.GetMaxHP();
                int srcHp1 = atk.Logic.GetCurrentHP();
                int destHP1 = atk.Logic.Damage(AttackInfo.damageToAttack);
                if (destHP1 == 0)
                    gameMode.UIManager.ShowAttackChangeHP(!IsLeft, defSr, maxHP1, srcHp1, destHP1, ConstTable.UI_VALUE_BAR_SPEED(), WaitTime, null);
                else
                    gameMode.UIManager.ShowAttackChangeHP(!IsLeft, defSr, maxHP1, srcHp1, destHP1, ConstTable.UI_VALUE_BAR_SPEED(), WaitTime, null);
            }
            if (AttackInfo.hit)
            {
                int maxHP = def.Logic.GetMaxHP();
                int srcHP = def.Logic.GetCurrentHP();
                int destHP = def.Logic.Damage(AttackInfo.damageToDefender);
                if (destHP == 0)
                    gameMode.UIManager.ShowAttackChangeHP(IsLeft, atkSr, maxHP, srcHP, destHP, ConstTable.UI_VALUE_BAR_SPEED(), WaitTime, () => Dead(def));
                else
                    gameMode.UIManager.ShowAttackChangeHP(IsLeft, atkSr, maxHP, srcHP, destHP, ConstTable.UI_VALUE_BAR_SPEED(), WaitTime, Continue);
                AttackInfo.damageToAttack = 10;
            }
            else
            {
                //miss
                Avoid();
            }
        }

        private void Dead(RPGCharacter ch)
        {
            gameMode.BattlePlayer.KillUnit(ch, ConstTable.UNIT_DISAPPEAR_SPEED(), Continue, true);
        }
        public override void Continue()
        {
            Debug.Log("Im continue");
            atkSr.GetComponent<MultiSpriteAnimator>().SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Stay);
            defSr.GetComponent<MultiSpriteAnimator>().SetActiveAnimator(MultiSpriteAnimator.EAnimateType.Stay);

            base.Continue();
        }
    }
}