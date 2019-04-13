using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class AttackInfoPanel : IPanel
    {
        public AudioClip WeaponSelectChange;
        public GameObject weaponSelect_Panel;
        public Text Text_Attack;
        public Text Text_Hit;
        public Text Text_Critical;
        public Text Text_Range;
        public Text Text_Anger;

        public Text Text_AttackSpeed;
        public Text Text_AttackEffect;
        public Text Text_Avoid;
        public Text Text_critAvoid;
        public Text Text_Movement;
        private RPGCharacter m_curGamechar;

        public void Init(RPGCharacter ch, int index = -1)//数值在GameChar里面有函数计算
        {
            m_curGamechar = ch;
            Text_Attack.text = ch.Logic().GetAttack().ToString();
            Text_Hit.text = ch.Logic().GetHit().ToString();
            Text_Critical.text = ch.Logic().GetCritical().ToString();
            Text_Range.text = ch.Logic().GetRangeMin() + "-" + ch.Logic().GetRangeMax();
            Text_Anger.text = ch.Logic().GetAnger().ToString();
            Text_AttackSpeed.text = ch.Logic().GetAttackSpeed().ToString();
            Text_AttackEffect.text = "攻击特效显示";//medifyneed
            Text_Avoid.text = ch.Logic().GetAvoid().ToString();
            Text_critAvoid.text = ch.Logic().GetCriticalAvoid().ToString();
            Text_Movement.text = ch.Logic().GetMovement().ToString();
        }
        public void OnWeaponSelectChange(int index)
        {
            SoundController.Instance.PlaySound(WeaponSelectChange);
            m_curGamechar.Logic().Item.EquipWeapon(index);//只改变当前选择的装备标识
            Init(m_curGamechar);
        }
    }
}