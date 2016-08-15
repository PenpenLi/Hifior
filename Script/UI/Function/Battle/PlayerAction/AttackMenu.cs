using UnityEngine;
using UnityEngine.Events;

namespace RPG.UI
{
    public class AttackMenu : IPanel
    {
        public GameObject Panel_CharState;
        public GameObject Panel_AttackInfo;
        public GameObject Panel_WeaponSelect;
        private CharStateBiggerPanel charstate;
        private AttackInfoPanel attackinfo;
        private SelectWeapon weaponselect;

        protected override void Awake()
        {
            base.Awake();

            charstate = Panel_CharState.GetComponent<CharStateBiggerPanel>();
            attackinfo = Panel_AttackInfo.GetComponent<AttackInfoPanel>();
            weaponselect = Panel_WeaponSelect.GetComponent<SelectWeapon>();
            gameObject.SetActive(false);//刚开始不显示
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="OnWeaponClicked"></param>
        public void Show(RPGCharacter ch,UnityAction<int> OnWeaponClicked)
        {
            //在这里做控件初始化
            charstate.Init(ch);
            attackinfo.Init(ch);
            weaponselect.Init(ch,OnWeaponClicked);
            gameObject.SetActive(true);
            weaponselect.Buttons[0].Select();
        }
    }
}
