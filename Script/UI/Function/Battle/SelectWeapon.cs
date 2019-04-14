using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
namespace RPG.UI
{
    public class SelectWeapon : AbstractUI
    {
        public GameObject AttackMenu;
        public CharacterLogic m_CurCharacter;
        public UnityEngine.UI.Button[] Buttons;
        public Text[] Text_Usage;
        public Text[] Text_WeaponName;
        public Image[] Image_WeaponIcon;

        /// <summary>
        /// 当武器选择完执行的事件
        /// </summary>
        private UnityAction<int> Event_OnWeaponClicked;

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
        public void Init(CharacterLogic ch, UnityAction<int> OnWeaponClicked)
        {
            if (Event_OnWeaponClicked == null)
                Event_OnWeaponClicked = OnWeaponClicked;

            m_CurCharacter = ch;
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
                Text_WeaponName[i].text = attackableItems[i].GetDefinition().CommonProperty.Name;
                Text_Usage[i].text = attackableItems[i].Usage + "/<color=green>" + +attackableItems[i].GetMaxUsage() + "</color>";
                Image_WeaponIcon[i].sprite = attackableItems[i].GetDefinition().Icon;
            }
        }
        public void OnClickWeapon(int ItemIndex)
        {
            AttackMenu.SetActive(false);

            Event_OnWeaponClicked.Invoke(ItemIndex);
        }

    }
}