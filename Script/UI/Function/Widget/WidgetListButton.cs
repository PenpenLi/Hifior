using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RPG.UI.Widget
{
    public class WidgetListButton : AbstractUI
    {
        public Text text_caption;
        public UnityEngine.UI.Button buttonPrefab;//拖拉一个进来

        protected override void Awake()
        {
            base.Awake();

            this.gameObject.SetActive(false);
        }
        public void InitCallBack(List<string> texts, params UnityAction[] events)
        {
#if UNITY_EDITOR
            if (texts.Count != events.Length)
            {
                Debug.LogError("事件与文字列表不一致");
                return;
            }
#endif
            for (int i = 0; i < texts.Count; i++)
            {
                UnityEngine.UI.Button b = Instantiate<UnityEngine.UI.Button>(buttonPrefab);
                b.onClick.AddListener(events[i]);
                b.GetComponentInChildren<Text>().text = texts[i];
            }
        }
        public void Show(string caption)
        {
            text_caption.text = caption;
        }
    }
}
