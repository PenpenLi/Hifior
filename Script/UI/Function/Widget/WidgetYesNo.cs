using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RPG.UI.Widget
{
    public class WidgetYesNo : IPanel
    {
        public UnityEngine.UI.Button button_yes;
        public UnityEngine.UI.Button button_no;
        public UnityEngine.UI.Text text_tip;
        public UnityEngine.UI.Image image_panel;
        private static WidgetYesNo instance;
        public static WidgetYesNo Instance
        {
            get
            {
                return instance;
            }
        }
        protected override void Awake()
        {
            base.Awake();

            if (instance == null)
                instance = this;
            this.gameObject.SetActive(false);
        }
        public void InitCallBack(UnityAction callYes, UnityAction callNo)
        {
            button_yes.onClick.AddListener(callYes);
            button_no.onClick.AddListener(callNo);
        }
        public void InitCallBack(UnityAction callYes)
        {
            button_yes.onClick.AddListener(callYes);
            button_no.onClick.AddListener(delegate { Hide(); });
        }
        public void Show(string tip)
        {
            text_tip.text = tip;
            gameObject.SetActive(true);
        }
        public void Show(string tip, Sprite sprite)
        {
            text_tip.text = tip;
            image_panel.sprite = sprite;
            gameObject.SetActive(true);
        }
    }
}