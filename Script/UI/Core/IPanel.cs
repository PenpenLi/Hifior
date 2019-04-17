using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
namespace RPG.UI
{
    [RequireComponent(typeof(Image))]
    public abstract class IPanel : AbstractUI
    {
        [Header("IPanel 基类参数")]
        public Image Background;
        public bool ShowAtStart = false;

        protected override void Awake()
        {
            base.Awake();

            Background = GetComponent<Image>();
            if (!ShowAtStart)
                Hide();
        }

        protected IEnumerator IAnimatorAnchoredPos(RectTransform rt, Vector2 from, Vector2 to, float t, UnityAction onFinish = null)
        {
            rt.anchoredPosition = from;
            float endTime = Time.time + t;
            while (Time.time <= endTime)
            {
                float ratio = 1.0f - (endTime - Time.time) / t;
                rt.anchoredPosition = Vector2.Lerp(from, to, ratio);
                yield return null;
            }
            rt.anchoredPosition = to;
            onFinish?.Invoke();
        }
    }
}
