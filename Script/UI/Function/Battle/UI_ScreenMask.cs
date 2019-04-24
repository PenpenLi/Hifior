using UnityEngine;

namespace RPG.UI
{
    public class UI_ScreenMask : IPanel
    {
        public enum EnumUIMaskState
        {
            Normal,
            Black,
            White
        }
        public static EnumUIMaskState State = EnumUIMaskState.Normal;
        public bool bReverse;
        public float Duration = 1.0f;
        public bool bDark;
        private Animator animator;
        public void Show(bool Reverse, bool Dark, float duration)
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
                    UI_ScreenMask.State = EnumUIMaskState.Black;
                }
                else
                {
                    animator.Play("NormalFadeToWhite");
                    UI_ScreenMask.State = EnumUIMaskState.White;
                }

            }
            else
            {
                if (bDark)
                    animator.Play("DarkFadeToNormal");
                else
                    animator.Play("WhiteFadeToNormal");
                UI_ScreenMask.State = EnumUIMaskState.Normal;
            }
        }

    }
}