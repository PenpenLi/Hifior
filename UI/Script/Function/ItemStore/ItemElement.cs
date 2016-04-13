using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace RPG.UI
{
    /// <summary>
    /// 用于显示单个UI的初始化脚本
    /// </summary>
    public class ItemElement :MonoBehaviour
    {
        public int Index;
        public Image Icon;
        public Text WeaponName;
        public Text WeaponDesc;
        
        public void RegisterClickEvent(UnityEngine.Events.UnityAction Action)
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(Action);
        }
        public void Show(int Index, Sprite Icon, string Name, string Desc)
        {
            GetComponent<Button>().interactable = true;
            this.Index = Index;
            this.Icon.gameObject.SetActive(true);
            this.Icon.sprite = Icon;
            this.WeaponName.text = Name;
            this.WeaponDesc.text = Desc;
        }
        public void ShowNothing(int Index)
        {
            GetComponent<Button>().interactable = false;
            this.Index = Index;
            this.Icon.gameObject.SetActive(false);
            this.WeaponName.text = string.Empty;
            this.WeaponDesc.text = string.Empty;
        }
    }
}