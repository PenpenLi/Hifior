using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace RPG.UI
{
    /// <summary>
    /// 用于显示单个UI的初始化脚本
    /// </summary>
    public class ItemElement : AbstractUI, ISelectHandler,IDeselectHandler
    {
        public int ItemIndex;
        public Image Icon;
        public Text Name;
        public Text Desc;
        public string TipContent;
        /// <summary>
        /// 当前选定按钮的Index
        /// </summary>
        public static int SelectIndex { private set; get; }
        public void RegisterClickEvent(UnityEngine.Events.UnityAction Action)
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(Action);
        }
        /// <summary>
        /// 显示一个物品栏
        /// </summary>
        /// <param name="Index">显示位置</param>
        /// <param name="Icon">显示的图标</param>
        /// <param name="Name">显示的名称</param>
        /// <param name="Desc">额外的介绍，一般是耐久度</param>
        /// <param name="TipContent">按下U键显示的Tooltip</param>
        public void Show(int Index, Sprite Icon, string Name, string Desc, bool Useable, string Tooltip = null)
        {
            TipContent = Tooltip;
            GetComponent<Button>().interactable = true;
            this.ItemIndex = Index;
            this.Icon.gameObject.SetActive(true);
            this.Icon.sprite = Icon;
            this.Name.text = Name;
            this.Desc.text = Desc;
        }
        public void ShowNothing(int Index)
        {
            TipContent = null;
            GetComponent<Button>().interactable = false;
            this.ItemIndex = Index;
            this.Icon.gameObject.SetActive(false);
            this.Name.text = string.Empty;
            this.Desc.text = string.Empty;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (ItemIndex == 0)
                GetComponent<Button>().Select();
        }
        public void ShowTip()
        {
            UIController.ItemTipPanel.Show(transform.position, TipContent);
        }
        public void HideTip()
        {
            UIController.ItemTipPanel.Hide();
        }

        public void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            SelectIndex = ItemIndex;
            if (UIController.ItemTipPanel.gameObject.activeSelf)
            {
                ShowTip();
            }
            else
            {
                HideTip();
            }
        }

        public void OnDeselect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            SelectIndex = -1;
        }
    }
}