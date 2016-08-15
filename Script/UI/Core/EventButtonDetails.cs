using UnityEngine;
using UnityEngine.Events;

namespace RPG.UI
{
    public class EventButtonDetail
    {
        public string buttonTitle;
        public Sprite buttonBackground;
        public UnityAction action;
        public EventButtonDetail(string Title, UnityAction Action, Sprite Icon = null)
        {
            buttonTitle = Title;
            action = Action;
            buttonBackground = Icon;
        }
    }

    public class ModalPanelDetail
    {
        public enum ModalMessageType
        {
            YesNo,
            YesNoCancel
        }
        public ModalMessageType MessageType;
        public string Question;
        public Sprite IconImage;
        public Sprite PanelBackgroundImage; // Not implemented
        public EventButtonDetail button1Details;
        public EventButtonDetail button2Details;
        public EventButtonDetail button3Details;
        public ModalPanelDetail(string Question, Sprite Icon, Sprite Background, EventButtonDetail ButtonYesDetail)
        {
            this.Question = Question;
            this.IconImage = Icon;
            this.PanelBackgroundImage = Background;
            MessageType = ModalMessageType.YesNo;
            button1Details = ButtonYesDetail;
        }
        public ModalPanelDetail(string Question, Sprite Icon, Sprite Background, EventButtonDetail ButtonYesDetail, EventButtonDetail ButtonNoDetail)
        {
            this.Question = Question;
            this.IconImage = Icon;
            this.PanelBackgroundImage = Background;
            MessageType = ModalMessageType.YesNo;
            button1Details = ButtonYesDetail;
            button2Details = ButtonNoDetail;
        }

        public ModalPanelDetail(string Question, Sprite Icon, Sprite Background, EventButtonDetail ButtonYesDetail, EventButtonDetail ButtonNoDetail, EventButtonDetail ButtonCancelDetail)
        {
            this.Question = Question;
            this.IconImage = Icon;
            this.PanelBackgroundImage = Background;
            MessageType = ModalMessageType.YesNoCancel;
            button1Details = ButtonYesDetail;
            button2Details = ButtonNoDetail;
            button3Details = ButtonCancelDetail;
        }
    }
}