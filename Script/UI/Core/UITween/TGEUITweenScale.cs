/* 
 * 作者：杨太彦
 * 更新：2015-11-28
 */
namespace TGE.Widget
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("TitanGameEngine/UI/Tween/Scale")] 
    public class TGEUITweenScale : TGEUITween
    {
        public Vector3 from;
        public Vector3 to;  

        protected override void UpdateTween(float _radio)
        {
            transform.localScale = from + (to - from) * _radio;
        }
    }
}