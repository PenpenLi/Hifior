using UnityEngine;
using System.Collections.Generic;
namespace RPG.UI
{
    public class PropsUseWeaponEquipPanel : IPanel
    {
        [Tooltip("是否是使用道具")]
        public bool bUseProps;
        /// <summary>
        /// 如果是显示使用道具传入true,装备武器传入false
        /// </summary>
        /// <param name="Props"></param>
        public void Show(RPGCharacter Character, bool UseProps)
        {
            base.Show();
            if (UseProps)
            {

            }
            else
            {

            }
        }
        private void InitUseProps(RPGCharacter Character)
        {
            for (int i = 0; i < Character.Logic().Item.Props.Count; i++)
            {
                PropsItem item = Character.Logic().Item.Props[i];

            }
        }
    }
}