/* 
 * 作者：杨太彦
 * 更新：2015-11-28
 */
namespace TGE.Widget
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("TitanGameEngine/UI/Tween/Color(UGUI Only)")]
    [RequireComponent(typeof(MaskableGraphic))]
    public class TGEUITweenColor : TGEUITween
    {
        public Color from = Color.white;
        public Color to   = Color.white;
        
        private MaskableGraphic selfGraphic;

        protected override void Awake()
        {
            base.Awake();
            selfGraphic = GetComponent<MaskableGraphic>();
        }

        protected override void UpdateTween(float _radio)
        { 
            selfGraphic.color = Color.LerpUnclamped(from, to, _radio);
        }
    }
}
