using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ItemPanel : IPanel
    {
        public GameObject TipPanel;
        public Image itemsBG;
        public Image EquipIcon;
        private ItemTip itemTipControl;
        private int currentSelectIndex = -1;
        private int currentSelectItemID = -1;
        public Text[] Text_Usage;
        public Text[] Text_WeaponName;
        public Image[] Image_WeaponIcon;
        private WeaponItem[] items;
        protected override void Awake()
        {
            base.Awake();

            itemTipControl = TipPanel.GetComponent<ItemTip>();
        }
        private void disable()
        {
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
        public void Init(RPGCharacter ch)
        {
            disable();
            int itemCount = ch.Item.GetWeaponCount();
            items = ch.Item.GetAllWeapons().ToArray();
            if (EquipIcon != null)
            {
                if (ch.Item.GetEquipWeapon() != null)
                    EquipIcon.gameObject.SetActive(true);
                else
                {
                    EquipIcon.gameObject.SetActive(false);
                }
            }
            if (itemsBG != null)
            {
                itemsBG.sprite = ch.GetPortrait();
            }
            for (int i = 0; i < itemCount; i++)
            {
                Text_Usage[i].enabled = true;
                Text_WeaponName[i].enabled = true;
                Image_WeaponIcon[i].enabled = true;
                Text_WeaponName[i].text =items[i].GetDefinition().CommonProperty.Name;
                if (!ch.Item.IsWeaponEnabled(items[i].ID))
                    Text_WeaponName[i].color = Color.grey;
                else
                    Text_WeaponName[i].color = Color.white;
                Text_Usage[i].text = items[i].Usage + "/<color=green>" + +items[i].GetMaxUsage() + "</color>";
                Image_WeaponIcon[i].sprite = items[i].GetDefinition().Icon;
            }
        }
        public void ShowTip(int index)
        {
            currentSelectIndex = index;
            WeaponDef def =ResourceManager.GetWeaponDef( items[currentSelectIndex].ID);

            string content = def.GetWeaponTypeName() + " " + def.GetWeaponLevelName() + "  " + "威力" + " " + def.Power + "  " + "命中" + " " + def.Hit + "  " + "必杀" + " " + def.Crit + "  " +
                "重量" + " " + def.Weight + "  " + "射程" + " " + def.RangeType.MinSelectRange + "-" + def.RangeType.MaxSelectRange + "\n" + def.CommonProperty.Description;
            itemTipControl.Show(Input.mousePosition, content);

        }
        public void HideTip()
        {
            currentSelectIndex = -1;
            currentSelectItemID = -1;
            itemTipControl.Hide();
        }
    }
}