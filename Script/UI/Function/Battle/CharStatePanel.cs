using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class CharStatePanel : IPanel
    {
        public Image charIcon;
        public Text charName;
        public Text lv;
        public Text hp;
        public Image weaponIcon;
        public Text weaponName;
        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void Show(RPGCharacter ch)
        {
            if (ch != null)
            {
                charIcon.sprite = ch.GetPortrait();
                charName.text = ch.GetCharacterName();
                hp.text = ch.Logic().GetCurrentHP() + "/" + ch.Logic().GetMaxHP();
                lv.text = ch.Logic().GetLevel().ToString();
                WeaponItem item = ch.Logic().Item.GetEquipWeapon();
                if (item != null)
                {
                    weaponIcon.gameObject.SetActive(true);
                    weaponName.text = item.GetDefinition().CommonProperty.Name;
                    weaponIcon.sprite = item.GetDefinition().Icon;
                }
                else
                {
                    weaponIcon.gameObject.SetActive(false);
                    weaponName.text = null;
                    weaponIcon.sprite = null;
                }
                gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
