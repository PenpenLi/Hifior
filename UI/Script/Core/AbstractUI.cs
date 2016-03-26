using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.UI
{
    public class AbstractUI : UActor, IComparable<AbstractUI>
    {
        [Header("AbstructUI 基类参数")]
        private static AbstractUI m_UI;
        public int SortOrder = 0;
        protected virtual void Awake()
        {
        }
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 使其显示在最前方
        /// </summary>
        public void SetTop()
        {
            SortOrder = int.MaxValue;
            transform.SetAsFirstSibling();
        }
        /// <summary>
        /// 使其显示在最后面
        /// </summary>
        public void SetBottom()
        {
            SortOrder = int.MinValue;
            transform.SetAsLastSibling();
        }
        /// <summary>
        /// 设置显示的前后顺序
        /// </summary>
        /// <param name="index"></param>
        public void SetSortOrder(int index)
        {
            SortOrder = index;
            SortUIPosition();
        }
        public void SortUIPosition()
        {
            List<AbstractUI> UIs = Utils.MiscUtil.GetChildComponents<AbstractUI>(transform.parent);
            UIs.Sort();
            for (int i = 0; i < UIs.Count; i++)
            {
                UIs[i].transform.SetAsFirstSibling();
            }
        }
        public static T GetInstance<T>() where T : AbstractUI
        {
            if (!m_UI)
            {
                m_UI = FindObjectOfType(typeof(T)) as T;
                if (!m_UI)
                    Debug.LogError("场景中未找到类型为" + typeof(T).GetType().ToString() + "激活的物体");
            }

            return m_UI as T;
        }

        public int CompareTo(AbstractUI other)
        {
            return SortOrder.CompareTo(other.SortOrder);
        }
    }
}