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
            Text_Attack.text = ch.GetAttack().ToString();
            Text_Hit.text = ch.GetHit().ToString();
            Text_Critical.text = ch.GetCritical().ToString();
            Text_Range.text = ch.GetRangeMin() + "-" + ch.GetRangeMax();
            Text_Anger.text = ch.GetAnger().ToString();
            Text_AttackSpeed.text = ch.GetAttackSpeed().ToString();
            Text_AttackEffect.text = "攻击特效显示";//medifyneed
            Text_Avoid.text = ch.GetAvoid().ToString();
            Text_critAvoid.text = ch.GetCriticalAvoid().ToString();
            Text_Movement.text = ch.GetMovement().ToString();
        }
        public void OnWeaponSelectChange(int index)
        {
            SoundController.Instance.PlaySound(WeaponSelectChange);
            m_curGamechar.Item.EquipItem(index);//只改变当前选择的装备标识
            Init(m_curGamechar);
        }
    }
}