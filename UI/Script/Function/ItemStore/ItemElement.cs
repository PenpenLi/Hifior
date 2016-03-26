using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class ItemElement : AbstractUI, IPointerDownHandler
    {
        public int ItemID;
        public Text WeaponName;
        public Text WeaponPrice;
        public Image Icon;
        public void SetItem(int itemID)
        {
            ItemID = itemID;
            WeaponDef def = ResourceManager.GetWeaponDef(itemID);
            WeaponName.text =def.CommonProperty.Name;
            WeaponPrice.text = def.GetPrice().ToString();
            //Icon.sprite = ItemIcon.GetIcon(itemID);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log(ItemID + " " + WeaponName.text + " = " + WeaponPrice.text);
        }
    }
}