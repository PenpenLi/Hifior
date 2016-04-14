using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;

namespace RPG.UI
{
    public class ModalPanel : IPanel
    {
        public HorizontalLayoutGroup HorizontalLayoutControl;

        public Text Question;
        public Image WidgetBG;
        public Image IconImage;
        public Button button1;
        public Button button2;
        public Button button3;

        public Text button1Text;
        public Text button2Text;
        public Text button3Text;
        public override void OnCancelKeyDown()
        {
            if (button3.gameObject.activeSelf)
                button3.onClick.Invoke();
            else
                button2.onClick.Invoke();
        }
        public void Show(ModalPanelDetail details)
        {
            base.Show();

            button1.Select();

            WidgetBG.sprite = details.PanelBackgroundImage;

            this.IconImage.gameObject.SetActive(false);
            button3.gameObject.SetActive(false);
            button2.gameObject.SetActive(false);
            this.Question.text = details.Question;

            if (details.IconImage)
            {
                this.IconImage.sprite = details.IconImage;
                this.IconImage.gameObject.SetActive(true);
                Question.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, -45);//如果有图片显示则调整文字的显示位置
            }
            else
            {
                IconImage.gameObject.SetActive(false);
                Question.GetComponent<RectTransform>().anchoredPosition = new Vector2(160, -45);
            }

            button1.onClick.RemoveAllListeners();
            button1.onClick.AddListener(details.button1Details.action);
            button1.onClick.AddListener(Hide);
            button1Text.text = details.button1Details.buttonTitle;
            button1.gameObject.SetActive(true);

            if (details.button2Details != null)
            {
                button2.onClick.RemoveAllListeners();
                button2.onClick.AddListener(details.button2Details.action);
                button2.onClick.AddListener(Hide);
                button2Text.text = details.button2Details.buttonTitle;
                button2.gameObject.SetActive(true);
            }

            if (details.button3Details != null)
            {
                button3.onClick.RemoveAllListeners();
                button3.onClick.AddListener(details.button3Details.action);
                button3.onClick.AddListener(Hide);
                button3Text.text = details.button3Details.buttonTitle;
                button3.gameObject.SetActive(true);

                HorizontalLayoutControl.spacing = 20;
            }
            else
            {
                button3.gameObject.SetActive(false);
                HorizontalLayoutControl.spacing = 35;
            }
        }
    }
}