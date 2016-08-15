using UnityEngine;

using UnityEngine.EventSystems;
//将这个类放到某个物体上可以添加点击显示ToolTip的功能

namespace RPG.UI
{
    public class OnClickedTipShow : AbstractUI, IPointerDownHandler, IPointerUpHandler
    {
        public GameObject smallTipUp;
        public string Content;
        private SmallTipUp tipPanel;
        protected override void Awake()
        {
            base.Awake();

            tipPanel = smallTipUp.GetComponent<SmallTipUp>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            tipPanel.Show(eventData.position, Content);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            tipPanel.Hide();
        }
    }
}