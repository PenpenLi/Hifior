using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class GetItemOrMoney : IPanel
    {
        public Text text;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void ShowGetWeapon(int itemID)
        {
            gameObject.SetActive(true);
            text.text = "得到 <color=yellow>" + ResourceManager.GetWeaponDef(itemID).CommonProperty.Name + "</color>";
        }
        public void ShowGetMoney(int money)
        {
            gameObject.SetActive(true);
            text.text = "得到金钱 <color=green>" + money + "</color>";
        }
    }
}