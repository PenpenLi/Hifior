using UnityEngine;

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
        public void Show(RPGCharacter ch)
        {
            //在这里做控件初始化
            charstate.init(ch);
            attackinfo.init(ch);
            weaponselect.init(ch);
            gameObject.SetActive(true);
        }
    }
}
