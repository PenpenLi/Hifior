using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
namespace RPG.UI
{
    public class EventButtonDetail
    {
        public string buttonTitle;
        public Sprite buttonBackground;  // Not implemented
        public UnityAction action;
    }

    public class ModalPanelDetail
    {
        public enum ModalMessageType
        {
            YesNo,
            YesNoCancel
        }
        public ModalMessageType MessageType;
        public string Title; // Not implemented
        public string Question;
        public Sprite IconImage;
        public Sprite PanelBackgroundImage; // Not implemented
        public EventButtonDetail button1Details;
        public EventButtonDetail button2Details;
        public EventButtonDetail button3Details;

        public ModalPanelDetail(EventButtonDetail ButtonYesDetail, EventButtonDetail ButtonNoDetail)
        {
            MessageType = ModalMessageType.YesNo;
            button1Details = ButtonYesDetail;
            button2Details = ButtonNoDetail;
        }

        public ModalPanelDetail(EventButtonDetail ButtonYesDetail, EventButtonDetail ButtonNoDetail, EventButtonDetail ButtonCancelDetail)
        {
            MessageType = ModalMessageType.YesNoCancel;
            button1Details = ButtonYesDetail;
            button2Details = ButtonNoDetail;
            button3Details = ButtonCancelDetail;
        }
    }
    
    public class ModalPanel : IPanel
    {
        public Text Question;
        public Image IconImage;
        public Button button1;
        public Button button2;
        public Button button3;

        public Text button1Text;
        public Text button2Text;
        public Text button3Text;

        public GameObject modalPanelObject;

        private static ModalPanel modalPanel;

        public void NewChoice(ModalPanelDetail details)
        {
            modalPanelObject.SetActive(true);

            this.IconImage.gameObject.SetActive(false);
            button1.gameObject.SetActive(false);
            button2.gameObject.SetActive(false);
            button3.gameObject.SetActive(false);

            this.Question.text = details.Question;

            if (details.IconImage)
            {
                this.IconImage.sprite = details.IconImage;
                this.IconImage.gameObject.SetActive(true);
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
            }
        }
    }
}