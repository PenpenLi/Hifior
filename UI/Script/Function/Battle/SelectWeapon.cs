using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SelectWeapon : AbstractUI
    {
        public GameObject AttackMenu;

        public UnityEngine.UI.Button[] Buttons;
        public Text[] Text_Usage;
        public Text[] Text_WeaponName;
        public Image[] Image_WeaponIcon;

        private List<WeaponItem> attackableItems = new List<WeaponItem>();

        public List<WeaponItem> getAttackableItems()
        {
            return attackableItems;
        }

        private void disable()
        {
            foreach (UnityEngine.UI.Button b in Buttons)
            {
                b.enabled = false;
                b.image.enabled = false;
            }
            foreach (Text text in Text_Usage)
            {
                text.enabled = false;
            }
            foreach (Text text in Text_WeaponName)
            {
                text.enabled = false;
            }
            foreach (Image image in Image_WeaponIcon)
            {
                image.enabled = false;
            }

        }
        public void init(RPGCharacter ch)
        {
            disable();
            attackableItems.Clear();
            attackableItems = ch.Item.GetAttackWeapon();

            for (int i = 0; i < attackableItems.Count; i++)
            {
                Buttons[i].enabled = true;
                Buttons[i].image.enabled = true;
                Text_Usage[i].enabled = true;
                Text_WeaponName[i].enabled = true;
                Image_WeaponIcon[i].enabled = true;
                Text_WeaponName[i].text =attackableItems[i].ID.ToString();
                Text_Usage[i].text = attackableItems[i].Usage + "/<color=green>" + +attackableItems[i].GetMaxUsage() + "</color>";
                Image_WeaponIcon[i].sprite = attackableItems[i].GetDefinition().Icon;
            }
        }
        public void OnClickWeapon(int index)
        {
            /*AttackMenu.SetActive(false);
            slg.getCurrentSelectGameChar().ItemGroup.EquipItemWithSort(index);//点击选择武器则重新排列武器并装备第一个
            SManage.Transition(new ASMInput(ACTION_STATE.ACTION_SELECTTARGETENEMY));*/
        }
        void OnDisable()
        {
            /*if (slg == null) return;
            if (slg.getCurrentSelectGameChar() != null)
                slg.getCurrentSelectGameChar().ItemGroup.EquipItem(0);*/
        }
    }
}