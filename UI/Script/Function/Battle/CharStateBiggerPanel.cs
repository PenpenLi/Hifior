using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class CharStateBiggerPanel : IPanel
    {
        public Text Text_Name;
        public Text Text_Job;
        public Text Text_Weapon;
        public Text Text_LV;
        public Text Text_EXP;
        public Text Text_HP;
        public Image icon;
        public void Init(RPGCharacter ch)
        {
            Text_Name.text = ch.PawnName;
            Text_Job.text = ch.GetCareerName() ;
            WeaponItem item = ch.Item.GetEquipItem();
            if (item != null)
            {
                Text_Weapon.text = item.GetDefinition().CommonProperty.Name;
            }
            else
            {
                Text_Weapon.text = "";
            }
            Text_LV.text = ch.GetLevel().ToString();
            Text_EXP.text = ch.GetExp().ToString();
            string curHp = null;
            if (ch.GetCurrentHP() == ch.GetMaxHP())
                curHp = "<color=green>" + ch.GetCurrentHP() + "</color>";
            if (ch.GetCurrentHP() >= ch.GetMaxHP() / 2 && ch.GetCurrentHP() < ch.GetMaxHP())
                curHp = "<color=orange>" + ch.GetCurrentHP() + "</color>";
            if (ch.GetCurrentHP() < ch.GetMaxHP() / 2)
                curHp = "<color=red>" + ch.GetCurrentHP() + "</color>";
            Text_HP.text = ch.GetMaxHP() + "/" + curHp;
            icon.sprite = ch.GetPortrait();
        }
    }
}