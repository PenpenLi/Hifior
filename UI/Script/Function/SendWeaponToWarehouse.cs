using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
namespace RPG.UI
{
    public class SendWeaponToWarehouse : IPanel
    {
        public Button WeaponButton;
        public Button PropsButton;
        public ItemElement[] ItemWeapons;
        public ItemElement[] ItemProps;
        List<WeaponItem> Weapons;
        List<PropsItem> Props;
        /// <summary>
        /// 显示所有武器，选择一个武器送到运输队里去
        /// </summary>
        /// <param name="Items"></param>
        /// <param name="ItemType"></param>
        public void Show(List<WeaponItem> Items)
        {
            Weapons = Items;
            WeaponButton.Select();
            WeaponButton.interactable = true;
            PropsButton.interactable = false;
            for (int i = 0; i < ItemWeapons.Length; i++)
            {
                if (i < Items.Count)
                    InitButton(i, Items[i]);
                else
                    ItemWeapons[i].ShowNothing(i);
            }
            Show();
        }
        public void Show(List<PropsItem> Items)
        {
            Props = Items;
            PropsButton.Select();
            WeaponButton.interactable = false;
            PropsButton.interactable = true;
            for (int i = 0; i < ItemWeapons.Length; i++)
            {
                if (i < Items.Count)
                    InitButton(i, Items[i]);
                else
                    ItemProps[i].ShowNothing(i);
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
            ItemWeapons[index].Show(index, def.Icon, def.CommonProperty.Name, Weapon.Usage + "/" + def.UseNumber);
            ItemWeapons[index].RegisterClickEvent(() =>
            {
                GetGameInstance().Ware.AddWeapon(Weapon);
                Weapons.RemoveAt(index);
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
            ItemProps[index].Show(index, def.Icon, def.CommonProperty.Name, Prop.Usage + "/" + def.UseNumber);
            ItemWeapons[index].RegisterClickEvent(() =>
            {
                GetGameInstance().Ware.AddProp(Prop);
                Props.RemoveAt(index);
                Hide();
            });
        }
    }
}
