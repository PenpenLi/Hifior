using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace RPG.UI
{
    public class UI_BattleActionMenu : IPanel
    {
        public struct UIActionButtonInfo
        {
            public string name;
            public UnityAction action;
            public UIActionButtonInfo(string _name, UnityAction _action) { name = _name; action = _action; }
        }

        List<UIActionButtonInfo> buttonsInfo;
        public List<Transform> buttons;
        protected override void Awake()
        {
            base.Awake();
            buttonsInfo = new List<UIActionButtonInfo>();
            //AddAction(new UIActionButtonInfo("open", () => { Debug.Log("click Open"); }));
        }
        public override void BeginPlay()
        {
            base.BeginPlay();
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
            int childCount = transform.childCount;
            int approveCount =infoCount - childCount ;
            for (int i = 0; i < approveCount; i++)
            {
                var newObj = Instantiate<GameObject>(transform.GetChild(0).gameObject);
                newObj.transform.SetParent(transform, false);
            }
            for (int i = 0; i < childCount; i++)
            {
                var childTransform = transform.GetChild(i);
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
            t.GetComponent<Button>().onClick.AddListener(info.action);
            t.name = info.name;
            t.GetComponentInChildren<Text>().text = info.name;
        }
        private void ClearAction(Transform t)
        {
            t.GetComponent<Button>().onClick.RemoveAllListeners();
            t.gameObject.SetActive(false);
        }
    }
}