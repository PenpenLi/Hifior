using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI
{
    public class ItemPanel : IPanel
    {
        public enum Mode
        {
            仅查看武器属性,
            选择攻击的武器,
            选择装备的武器,
            仅查看物品属性,
            选择使用的物品
        }
        public Mode ShowMode;
        public Image ItemsBG;
        private int currentSelectIndex
        {
            get
            {
                return ItemElement.SelectIndex;
            }
        }

        public ItemElement[] Elements;

        private WeaponItem[] weaponItems;
        private PropsItem[] propsItems;

        protected override void Awake()
        {
            base.Awake();
        }
        /// <summary>
        /// 使所有的子按钮不可用不可见，但是仍然占用位置
        /// </summary>
        private void DisableAllUIElement()
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                DisableItem(i);
            }
        }

        private void DisableItem(int Index)
        {
            Elements[Index].ShowNothing(Index);
        }

        public void Init(CharacterLogic ch, Mode ShowMode = Mode.仅查看武器属性)
        {
            this.ShowMode = ShowMode;

            DisableAllUIElement();

            if (ItemsBG != null)
            {
                ItemsBG.sprite = ch.characterDef.Portrait;
            }
            var item = ch.Info.Items;
            if (ShowMode <= Mode.选择装备的武器)
            {
                int itemCount = item.GetWeaponCount();
                weaponItems = item.Weapons.ToArray();
                int EquipIndex = item.GetEquipIndex();
                for (int i = 0; i < itemCount; i++)
                {
                    WeaponDef def = weaponItems[i].GetDefinition();
                    string ShowName = EquipIndex == i ? def.CommonProperty.Name + Utils.TextUtil.GetColorString("  E", Color.yellow) : def.CommonProperty.Name;
                    Elements[i].Show(i, def.Icon, ShowName, weaponItems[i].Usage + "/" + Utils.TextUtil.GetColorString(weaponItems[i].GetMaxUsage().ToString(), Color.green), item.IsWeaponEnabled(weaponItems[i].ID), def.Tooltip);
                }
            }
            else
            {
                int propsCount = item.GetPropsCount();
                propsItems = item.Props.ToArray();

                for (int i = 0; i < propsCount; i++)
                {
                    PropsDef def = propsItems[i].GetDefinition();
                    Elements[i].Show(i, def.Icon, def.CommonProperty.Name, def.EquipItem ? "<color=green> E </ color > " : weaponItems[i].Usage + "/<color=green>" + +weaponItems[i].GetMaxUsage() + "</color>", true, def.Tooltip);
                }
            }
        }
        public void ShowTip(int index)
        {
            WeaponDef def = ResourceManager.GetWeaponDef(weaponItems[currentSelectIndex].ID);

            string content = def.GetWeaponTypeName() + " " + def.GetWeaponLevelName() + "  " + "威力" + " " + def.Power + "  " + "命中" + " " + def.Hit + "  " + "必杀" + " " + def.Crit + "  " +
                "重量" + " " + def.Weight + "  " + "射程" + " " + def.RangeType.SelectRange.x + "-" + def.RangeType.SelectRange.y + "\n" + def.CommonProperty.Description;
            Debug.Log(content);
            //ItemTipControl.Show(Input.mousePosition, content);

        }
        public void HideTip()
        {
            UIController.ItemTipPanel.Hide();
        }
        public override void Tick(float DeltaTime)
        {
            //if (Input.GetButtonDown("X"))
            //{
            //    if (currentSelectIndex < 0)
            //        return;
            //    if (UIController.ItemTipPanel.gameObject.activeSelf)
            //        UIController.ItemTipPanel.Hide();
            //    else
            //        Elements[currentSelectIndex].ShowTip();
            //}
        }
    }
}