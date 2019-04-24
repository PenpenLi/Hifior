using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class UI_GetItemOrMoney : IPanel
    {
        public Text text;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void ShowGetWeapon(int WeaponID)
        {
            gameObject.SetActive(true);
            text.text = "得到 <color=yellow>" + ResourceManager.GetWeaponDef(WeaponID).CommonProperty.Name + "</color>";
        }
        public void ShowGetProps(int PropsID)
        {
            gameObject.SetActive(true);
            text.text = "得到 <color=cyan>" + ResourceManager.GetPropsDef(PropsID).CommonProperty.Name + "</color>";
        }
        public void ShowGetMoney(int MoneyAmount)
        {
            gameObject.SetActive(true);
            text.text = "得到金钱 <color=green>" + MoneyAmount + "</color>";
        }
    }
}