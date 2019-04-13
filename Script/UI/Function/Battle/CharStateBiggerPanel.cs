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
            Text_Name.text = ch.GetCharacterName();
            Text_Job.text = ch.Logic().GetCareerName();
            WeaponItem item = ch.Logic().Item.GetEquipWeapon();
            if (item != null)
            {
                Text_Weapon.text = item.GetDefinition().CommonProperty.Name;
            }
            else
            {
                Text_Weapon.text = "";
            }
            Text_LV.text = ch.Logic().Info.Level.ToString();
            Text_EXP.text = ch.Logic().GetExp().ToString();
            string curHp = null;
            if (ch.Logic().GetCurrentHP() == ch.Logic().Info.MaxHP)
                curHp = "<color=green>" + ch.Logic().GetCurrentHP() + "</color>";
            if (ch.Logic().GetCurrentHP() >= ch.Logic().Info.MaxHP / 2 && ch.Logic().GetCurrentHP() < ch.Logic().Info.MaxHP)
                curHp = "<color=orange>" + ch.Logic().GetCurrentHP() + "</color>";
            if (ch.Logic().GetCurrentHP() < ch.Logic().Info.MaxHP / 2)
                curHp = "<color=red>" + ch.Logic().GetCurrentHP() + "</color>";
            Text_HP.text = ch.Logic().GetMaxHP() + "/" + curHp;
            icon.sprite = ch.GetPortrait();
        }
    }
}