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
            if (gameObject.activeSelf == true) return;
            /*if (ch != null && StateMachine.SManage.CurrentBattleState == BATTLE_STATE.PlayerAction)
            {
                charIcon.sprite = ch.IconGroup.Icon;
                charName.text = ch.attribute.Name;
                hp.text = ch.attribute.CurHP + "/" + ch.attribute.MaxHP;
                lv.text = ch.attribute.LV.ToString();
                WeaponItem item = ch.ItemGroup.getEquipItem();
                if (item != null)
                {
                    weaponName.text = Table._ItemTable.getName(item.ID);
                    weaponIcon.sprite = item.Icon;
                }
                else
                {
                    weaponName.text = null;
                    weaponIcon.sprite = null;
                }
                gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }*/
        }
    }
}
