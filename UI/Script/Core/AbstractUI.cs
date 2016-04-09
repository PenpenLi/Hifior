using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

namespace RPG.UI
{
    public class AbstractUI : UActor, IComparable<AbstractUI>
    {
        [Header("AbstructUI 基类参数")]
        private static AbstractUI m_UI;
        public int SortOrder = 0;
        protected UnityAction OnHideDelegate;
        protected virtual void Awake()
        {
            Init();
        }
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 关闭窗口，如果不执行Delegate事件 使用Hide(false)
        /// </summary>
        public virtual void Hide()
        {
            Hide(true);
        }
        /// <summary>
        /// 关闭窗口，如果不执行Delegates事件，传入false
        /// </summary>
        /// <param name="InvokeDelegate"></param>
        public virtual void Hide(bool InvokeDelegate)
        {
            gameObject.SetActive(false);
            if (OnHideDelegate != null && InvokeDelegate)
                OnHideDelegate.Invoke();
        }
        public void RegisterOnHide(UnityAction OnHide)
        {
            OnHideDelegate = OnHide;
        }
        public bool Visible
        {
            get
            {
                return gameObject.activeSelf;
            }
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

        #region Tween
        /// <summary>
        /// 多长时间完成
        /// </summary>
        [Header("UI Tween")]
        public float tweenTimeConsuming = 1f;

        /// <summary>
        /// 在执行完成后调用此事件,该事件只在Once时有效
        /// </summary>
        public UnityEvent onFinish = null;

        /// <summary>
        /// 当前的时间
        /// </summary>
        private float timeCurrent = 0f;
        public enum FadeType
        {
            None,
            Color,
            Alpha
        }
        public FadeType fadeType;
        public Color fromColor = Color.white;
        public Color toColor = Color.white;
        public float fromAlpha;
        public float toAlpha;
        [Tooltip("建议仅当Fade Alpha时勾选为true，这项会更改最终的颜色")]
        public bool fadeChild = true;
        private MaskableGraphic selfGraphic;
        private MaskableGraphic[] childGraphics;
        private Color selfColor;

        IEnumerator Tween()
        {

            bool isOver = false;
            while (!isOver)
            {
                timeCurrent += Time.deltaTime;

                if (timeCurrent > tweenTimeConsuming)
                {
                    timeCurrent = tweenTimeConsuming;
                    isOver = true;
                }

                UpdateTween(timeCurrent / tweenTimeConsuming);
                yield return null;
            }
            timeCurrent = 0;
            if (onFinish != null)
                onFinish.Invoke();
        }

        public void ShowFadeAlpha(bool fadeChild = true)
        {
            fadeType = FadeType.Alpha;
            ShowFade(fadeChild);
        }
        public void ShowFadeColor(bool fadeChild = true)
        {
            fadeType = FadeType.Color;
            ShowFade(fadeChild);
        }
        public void ShowFade(bool bFadeChild = true)
        {
            fadeChild = bFadeChild;
            Show();
            StartCoroutine(Tween());
        }
        public void Init()
        {
            tweenTimeConsuming = tweenTimeConsuming <= 0 ? 1 : tweenTimeConsuming;
            selfGraphic = GetComponent<MaskableGraphic>();
            if (fadeChild)
                childGraphics = GetComponentsInChildren<MaskableGraphic>();
        }
        public void UpdateTween(float ratio)
        {
            if (!selfGraphic)
                return;
            if (fadeType == FadeType.Alpha)
            {
                selfColor = selfGraphic.color;
                selfColor.a = fromAlpha + (toAlpha - fromAlpha) * ratio;
                selfGraphic.color = selfColor;
                if (fadeChild && childGraphics != null)
                {
                    for (int i = 0; i < childGraphics.Length; i++)
                    {
                        Color c = childGraphics[i].color;
                        c.a = fromAlpha + (toAlpha - fromAlpha) * ratio;
                        childGraphics[i].color = c;
                    }
                }
            }
            if (fadeType == FadeType.Color)
            {
                selfGraphic.color = Color.LerpUnclamped(fromColor, toColor, ratio);
                if (fadeChild && childGraphics != null)
                {
                    for (int i = 0; i < childGraphics.Length; i++)
                    {
                        childGraphics[i].color = Color.LerpUnclamped(fromColor, toColor, ratio);
                    }
                }
            }
        }

        #endregion
    }
}