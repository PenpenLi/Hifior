/* 
 * 作者：杨太彦
 * 更新：2015-11-28
 */
namespace TGE.Widget
{
    using UnityEngine;
    using UnityEngine.UI;
    
    [AddComponentMenu("TitanGameEngine/UI/Tween/Alpha(UGUI Only)")]
    [RequireComponent(typeof(MaskableGraphic))]
    public class TGEUITweenAlpha : TGEUITween
    {
        public float from;
        public float to; 

        private Color           selfColor; 
        private MaskableGraphic selfGraphic;

        protected override void Awake() 
        {
            base.Awake();
            selfGraphic = GetComponent<MaskableGraphic>();
        }

        protected override void UpdateTween(float _radio)
        {
            selfColor = selfGraphic.color;
            selfColor.a = from + (to - from) * _radio;
            selfGraphic.color = selfColor;
        }
    }
}
