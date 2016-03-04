/* 
 * 作者：杨太彦
 * 更新：2015-11-28
 */
namespace TGE.Widget
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("TitanGameEngine/UI/Tween/Size(UGUI Only)")]
    [RequireComponent(typeof(RectTransform))]
    public class TGEUITweenSize : TGEUITween
    {
        public Vector2 from;
        public Vector2 to;

        private RectTransform selfTransform;

        protected override void Awake()
        {
            base.Awake();
            selfTransform = transform as RectTransform; 
        }

        protected override void UpdateTween(float _radio)
        {
            selfTransform.sizeDelta = from + (to - from) * _radio;
        }
    }
}