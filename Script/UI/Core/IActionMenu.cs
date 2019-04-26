using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
namespace RPG.UI
{
    public class IActionMenu : IPanel
    {
        public struct UIActionButtonInfo
        {
            public string name;
            public UnityAction action;
            public bool enable;
            public UIActionButtonInfo(string _name, UnityAction _action, bool _enable = true) { name = _name; action = _action; enable = _enable; }
        }
        public Transform buttonParent;
        List<UIActionButtonInfo> buttonsInfo;
        protected override void Awake()
        {
            base.Awake();
            if (buttonParent == null)
                buttonParent = transform;
            buttonsInfo = new List<UIActionButtonInfo>();
            //AddAction(new UIActionButtonInfo("open", () => { Debug.Log("click Open"); }));
        }
        public override void Show()
        {
            base.Show();
            RefreshUI();
        }
        public void Clear()
        {
            buttonsInfo.Clear();
        }
        public void AddAction(UIActionButtonInfo info)
        {
            buttonsInfo.Add(info);
        }
        public void AddActions(List<UIActionButtonInfo> info)
        {
            buttonsInfo.AddRange(info);
        }
        public void RefreshUI()
        {
            int infoCount = buttonsInfo.Count;
            int childCount = buttonParent.childCount;
            int approveCount = infoCount - childCount;
            for (int i = 0; i < approveCount; i++)
            {
                var newObj = Instantiate<GameObject>(buttonParent.GetChild(0).gameObject);
                newObj.transform.SetParent(buttonParent, false);
            }
            for (int i = 0; i < childCount; i++)
            {
                var childTransform = buttonParent.GetChild(i);
                if (i < infoCount)
                {
                    SetAction(childTransform, buttonsInfo[i]);
                }
                else
                {
                    ClearAction(childTransform);
                }
            }
        }
        private void SetAction(Transform t, UIActionButtonInfo info)
        {
            t.gameObject.SetActive(true);
            var button = t.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(info.action);
            button.interactable = info.enable;
            t.name = info.name;
            t.GetComponentInChildren<Text>().text = info.name;
        }
        private void ClearAction(Transform t)
        {
            t.gameObject.SetActive(false);
        }
    }
}