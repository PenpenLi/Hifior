using UnityEngine;

namespace RPG.UI
{
    public class ScreenMask : AbstractUI
    {
        public enum EnumUIMaskState
        {
            Normal,
            Black,
            White
        }
        public static EnumUIMaskState State=EnumUIMaskState.Normal;
        public bool bReverse;
        public float Duration=1.0f;
        public bool bDark;
        private Animator animator;
        public void Show(bool Reverse,bool Dark,float duration)
        {
            bReverse = Reverse;
            bDark = Dark;
            Duration = duration;
            Show();
            OnEnable();
        }
        public override void Show()
        {
            base.Show();
            transform.SetAsLastSibling();
            RectTransform rt = GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1920f);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,1080f);
            rt.localScale = Vector3.one;
        }
        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            Hide();
        }
        protected override void OnEnable()
        {
            animator.speed = 1.0f / Duration;
            if (bReverse) 
            {
                if (bDark)
                {
                    animator.Play("NormalFadeToDark");
                    ScreenMask.State = EnumUIMaskState.Black;
                }
                else
                {
                    animator.Play("NormalFadeToWhite");
                    ScreenMask.State = EnumUIMaskState.White;
                }

            }
            else
            {
                if (bDark)
                    animator.Play("DarkFadeToNormal");
                else
                    animator.Play("WhiteFadeToNormal");
                ScreenMask.State = EnumUIMaskState.Normal;
            }
        }
    }
}