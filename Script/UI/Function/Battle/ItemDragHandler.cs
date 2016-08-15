using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ItemDragHandler : AbstractUI, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public static GameObject itemBeingDragged;
        private Image bc;
        protected override void Awake()
        {
            base.Awake();

            bc = GetComponent<Image>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            bc.color = new Color(bc.color.r, bc.color.g, bc.color.b, 0.3f);
            itemBeingDragged = gameObject;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        public void OnDrag(PointerEventData eventData)
        {

        }
        public void OnEndDrag(PointerEventData eventData)
        {
            bc.color = new Color(bc.color.r, bc.color.g, bc.color.b, 0f);
            //将VerticalLayoutGroup关闭在开启
            GetComponentInParent<VerticalLayoutGroup>().enabled = false;
            GetComponentInParent<VerticalLayoutGroup>().enabled = true;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            itemBeingDragged = null;
        }

        public void OnDrop(PointerEventData eventData)//当有东西拖动时在此物件上释放时触发此事件 在Item上释放则调换位置，在panel空白处释放则添加到末尾
        {
            if (itemBeingDragged.transform.parent == transform.parent)
                return;
            TidyUpPanel.tidyUp.selectedItemIndex = int.Parse(itemBeingDragged.name);//设置需要置换的装备位置
            TidyUpPanel.tidyUp.exchangeItemIndex = int.Parse(transform.name);
            if (itemBeingDragged.transform.parent.parent.name == "Panel_ItemsBottom")
            {
                TidyUpPanel.tidyUp.exchangeItem(TidyUpPanel.tidyUp.selectedCharIndex, TidyUpPanel.tidyUp.selectedItemIndex, TidyUpPanel.tidyUp.exchangeCharIndex, TidyUpPanel.tidyUp.exchangeItemIndex);
            }
            else
            {
                TidyUpPanel.tidyUp.exchangeItem(TidyUpPanel.tidyUp.exchangeCharIndex, TidyUpPanel.tidyUp.selectedItemIndex, TidyUpPanel.tidyUp.selectedCharIndex, TidyUpPanel.tidyUp.exchangeItemIndex);
            }

            Debug.Log("Char:" + TidyUpPanel.tidyUp.selectedCharIndex.ToString() + " Item:" + itemBeingDragged.name + " drop to " + "Char:" + TidyUpPanel.tidyUp.exchangeCharIndex.ToString() + " Item:" + transform.name);
        }
    }
}