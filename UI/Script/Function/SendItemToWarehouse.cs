using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
namespace RPG.UI
{
    public class SendItemToWarehouse : IPanel
    {
        public ItemElement[] Elements;
        List<WeaponItem> WeaponItems;
        List<PropsItem> PropsItems;

        /// <summary>
        /// 显示所有武器，选择一个武器送到运输队里去
        /// </summary>
        /// <param name="Items"></param>
        /// <param name="ItemType"></param>
        public void Show(List<WeaponItem> Items)
        {
            base.Show();
            Elements[0].GetComponent<Button>().Select();

            WeaponItems = Items;
            for (int i = 0; i < Elements.Length; i++)
            {
                if (i < Items.Count)
                    InitButton(i, Items[i]);
                else
                    Elements[i].ShowNothing(i);
            }
        }

        public void Show(List<PropsItem> Items)
        {
            base.Show();
            Elements[0].GetComponent<Button>().Select();

            PropsItems = Items;
            for (int i = 0; i < Elements.Length; i++)
            {
                if (i < Items.Count)
                    InitButton(i, Items[i]);
                else
                    Elements[i].ShowNothing(i);
            }
        }

        /// <summary>
        /// 初始化控件中武器的信息显示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="Weapon"></param>
        public void InitButton(int index, WeaponItem Weapon)
        {
            WeaponDef def = Weapon.GetDefinition();
            Elements[index].Show(index, def.Icon, def.CommonProperty.Name, def.IsInifiniteUsage() ? " -- " : Weapon.Usage + "/" + def.UseNumber,true);
            Elements[index].RegisterClickEvent(() =>
            {
                GetGameInstance().Ware.AddWeapon(Weapon);
                WeaponItems.RemoveAt(index);
                Hide();
            });
        }

        /// <summary>
        /// 初始化控件中道具的信息显示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="Prop"></param>
        public void InitButton(int index, PropsItem Prop)
        {
            PropsDef def = Prop.GetDefinition();
            Elements[index].Show(index, def.Icon, def.CommonProperty.Name, PropsThirdText(def, Prop.Usage),true);
            Elements[index].RegisterClickEvent(() =>
            {
                GetGameInstance().Ware.AddProp(Prop);
                PropsItems.RemoveAt(index);
                Hide();
            });
        }
        /// <summary>
        /// 第三个栏位显示，如果是可装备物品，则不显示
        /// </summary>
        /// <param name="def"></param>
        /// <param name="Usage"></param>
        /// <returns></returns>
        private string PropsThirdText(PropsDef def, int Usage)
        {
            if (def.EquipItem)
                return " E ";
            else
                return Usage + "/" + def.UseNumber;
        }
    }
}
