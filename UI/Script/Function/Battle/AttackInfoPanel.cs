using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class AttackInfoPanel : AbstractUI
    {
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

        public void init(RPGCharacter ch, int index = -1)//数值在GameChar里面有函数计算
        {
            m_curGamechar = ch;
           /* Text_Attack.text = ch.getAttack().ToString();
            Text_Hit.text = ch.getHit().ToString();
            Text_Critical.text = ch.getCritical().ToString();
            Text_Range.text = ch.getRangeMin() + "-" + ch.getRangeMax();
            Text_Anger.text = ch.getAnger().ToString();
            Text_AttackSpeed.text = ch.getAttackSpeed().ToString();
            Text_AttackEffect.text = ch.getAttackEffect1().ToString();//medifyneed
            Text_Avoid.text = ch.getAvoid().ToString();
            Text_critAvoid.text = ch.getCriticalAvoid().ToString();
            Text_Movement.text = ch.getMovement().ToString();*/
        }
        public void OnWeaponSelectChange(int index)
        {
           /* SLGLevel.SLG._sound.Play2DEffect(18);
            curGamechar.ItemGroup.EquipItem(index);//只改变当前选择的装备标识
            init(curGamechar);*/
        }
    }
}