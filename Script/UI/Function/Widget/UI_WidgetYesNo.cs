using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RPG.UI
{
    public class UI_WidgetYesNo : IPanel
    {
        public Button button_yes;
        public Button button_no;
        public Text text_tip;
        public Image image_panel;
        
        public void InitCallBack(UnityAction callYes, UnityAction callNo)
        {
            setLayout(false);
            removeCallback();
            button_yes.onClick.AddListener(callYes);
            button_no.onClick.AddListener(callNo);
        }

        public void InitCallBack(UnityAction callYes, bool yesOnly)
        {
            setLayout(yesOnly);
            removeCallback();
            button_yes.onClick.AddListener(callYes);
            button_no.onClick.AddListener(delegate { Hide(); });
        }
        private void removeCallback()
        {
            button_yes.onClick.RemoveAllListeners();
            button_no.onClick.RemoveAllListeners();
        }
        private void setLayout(bool singleButton)
        {
            if (singleButton)
            {
                var rt = button_yes.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0, -50);
                button_no.gameObject.SetActive(false);
            }
            else
            {
                button_no.gameObject.SetActive(true);
                var rt_yes = button_yes.GetComponent<RectTransform>();
                var rt_no = button_no.GetComponent<RectTransform>();
                rt_yes.anchoredPosition = new Vector2(-100, -50);
                rt_no.anchoredPosition = new Vector2(100, -50);
            }
        }
        public void Show(string tip)
        {
            transform.SetAsLastSibling();
            text_tip.text = tip;
            gameObject.SetActive(true);
        }
        public void Show(string tip, Sprite sprite)
        {
            transform.SetAsLastSibling();
            text_tip.text = tip;
            image_panel.sprite = sprite;
            gameObject.SetActive(true);
        }
        private void Update()
        {
            if (gameMode.InputManager.GetNoInput())
            {
                button_no.onClick.Invoke();
            }
        }
    }
}